using AutoMapper;
using PMS.DB.Model.EF.Models;
using PMS.Repositories.Interfaces;
using PMS.Services.Interfaces;
using PMS.Services.ServiceModels;

namespace PMS.Services.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public ProductSM GetProductById(int productId)
    {
        if (productId <= 0)
        {
            throw new ArgumentException("Invalid productId. Must be greater than zero.", nameof(productId));
        }

        var product = _productRepository.GetT(x => x.ProductId == productId, "CategoryLkp");
        var data = _mapper.Map<ProductSM>(product);
        data.CategoryLkpName = data.CategoryLkp != null? data.CategoryLkp.VisibleValue:string.Empty;
        return data;
    }

    public IEnumerable<ProductSM> GetProducts(int pageSize, int pageNumber, string searchText, out int totalCount)
    {
        if (pageSize <= 0 || pageNumber < 0)
        {
            throw new ArgumentException("Invalid pageSize or pageNumber. It cannot be negative", nameof(pageSize));
        }

        var products = _productRepository.GetProducts(pageSize, pageNumber, searchText, out totalCount);
        var data = _mapper.Map<List<ProductSM>>(products);
        data.ForEach(x =>  x.CategoryLkpName = x.CategoryLkp != null? x.CategoryLkp.VisibleValue: string.Empty);

        return data;
    }

    public int CreateProduct(ProductSM product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");
        }

        var entity = _mapper.Map<PmsProduct>(product);
        _productRepository.Add(entity);
        int rv = _productRepository.SaveChanges();
        return entity.ProductId;
    }

    public int UpdateProduct(ProductSM updatedProduct)
    {
        var existingProduct = _productRepository.GetT(x=>x.ProductId == updatedProduct.ProductId);
        if (existingProduct == null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        existingProduct.ProductName = updatedProduct.ProductName;
        existingProduct.ProductCode = updatedProduct.ProductCode;
        existingProduct.Description = updatedProduct.Description;
        existingProduct.Price = updatedProduct.Price;
        existingProduct.Uom = updatedProduct.Uom;
        existingProduct.CategoryLkpId = updatedProduct.CategoryLkpId;

        _productRepository.Update(existingProduct);
        return _productRepository.SaveChanges(); 
    }

    public void DeleteProduct(int productId)
    {
        // Check if the product exists
        var existingProduct = _productRepository.GetT(x => x.ProductId == productId);
        if (existingProduct == null)
        {
            throw new InvalidOperationException("Product not found."); 
        }
        existingProduct.ActiveFlag = false;
        _productRepository.Update(existingProduct);
        _productRepository.SaveChanges();
    }

}

