using AutoMapper;
using Moq;
using PMS.DB.Model.EF.Models;
using PMS.Repositories.Interfaces;
using PMS.Services.ServiceModels;
using PMS.Services.Services;
using System.Linq.Expressions;

namespace PMS.Test.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ProductService _productService;
    private readonly Mock<IMapper> _mapperMock;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();
        _productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
    }

    #region GetProducts & GetProductByID 

    [Fact]
    public void GetProductById_ValidProductId_ReturnsProduct()
    {
        var productId = 1;
        var productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
        var productEntity = new PmsProduct
        {
            ProductId = productId,
            ProductName = "Test Product"
        };

        var productSM = new ProductSM
        {
            ProductId = productId,
            ProductName = "Test Product"
        };

        // Setup the repository mock to return a specific product entity based on the provided predicate
        _productRepositoryMock
            .Setup(x => x.GetT(
                It.IsAny<Expression<Func<PmsProduct, bool>>>(),
                It.IsAny<string?>() // IncludeProperties setup
            ))
            .Returns<Expression<Func<PmsProduct, bool>>, string?>((predicate, includeProperties) =>
                GetMatchingProduct(predicate) 
            );

        // Setup the mapper mock to map the product entity to product SM
        _mapperMock
            .Setup(x => x.Map<ProductSM>(It.IsAny<PmsProduct>()))
            .Returns(productSM); 

        var mockedProduct = _productRepositoryMock.Object.GetT(x => x.ProductId == productId);
        Assert.NotNull(mockedProduct);
        Assert.Equal(productId, mockedProduct.ProductId);

        var result = productService.GetProductById(productId);

        Assert.NotNull(result); 
        Assert.Equal(productId, result.ProductId); 
        Assert.Equal("Test Product", result.ProductName);
    }

    [Fact]
    public void GetProducts_ValidParameters_ReturnsProductList()
    {
        // Arrange
        var pageSize = 10;
        var pageNumber = 1;
        var searchText = "Test";
        var totalCount = 5;
        var productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
        var productEntities = new List<PmsProduct>
        {
            new PmsProduct { ProductId = 1, ProductName = "Product 1" },
            new PmsProduct { ProductId = 2, ProductName = "Product 2" }
        };
        var productSMs = new List<ProductSM>
        {
            new ProductSM { ProductId = 1, ProductName = "Product 1" },
            new ProductSM { ProductId = 2, ProductName = "Product 2" }
        };

        _productRepositoryMock.Setup(x => x.GetProducts(pageSize, pageNumber, searchText, out totalCount)).Returns(productEntities);
        _mapperMock.Setup(x => x.Map<List<ProductSM>>(productEntities)).Returns(productSMs);

        // Act
        var result = productService.GetProducts(pageSize, pageNumber, searchText, out int total);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(totalCount, total);
        Assert.Equal(2, result.Count());
    }

    private PmsProduct GetMatchingProduct(Expression<Func<PmsProduct, bool>> predicate)
    {
        var productList = new List<PmsProduct>
    {
        new PmsProduct { ProductId = 1, ProductName = "Test Product" ,CategoryLkpId = 1},
        new PmsProduct { ProductId = 2, ProductName = "Another Product", CategoryLkpId = 1}
    };

        return productList.FirstOrDefault(predicate.Compile());
    }
    #endregion

    #region CreatePRoduct

    [Fact]
    public void CreateProduct_ValidProduct_ReturnsNonZeroProductId()
    {
        // Arrange
        var productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
        var productSM = new ProductSM
        {
            ProductName = "Test Product",
            CategoryLkpId = 1,
            ActiveFlag = true,
            ImageName = "image.jpg",
            Price = 100,
            ProductCode = "PROD-001",
            Uom = "ml",
            Description = "Test Product"
        };

        var productEntity = new PmsProduct
        {
            ProductId = 1, // Assuming a non-zero ProductId upon creation
            ProductName = "Test Product",
            CategoryLkpId = 1,
            ActiveFlag = true,
            Price = 100,
            ProductCode = "PROD-001",
            Uom = "ml",
            Description = "Test Product"
        };

        _mapperMock.Setup(x => x.Map<PmsProduct>(productSM)).Returns(productEntity);

        _productRepositoryMock.Setup(x => x.SaveChanges()).Returns(1);

        var result = productService.CreateProduct(productSM);

        Assert.NotEqual(0, result); // Ensure the returned ProductId is not zero
    }

    [Fact]
    public void CreateProduct_NullProduct_ThrowsException()
    {
        // Arrange
        var productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => productService.CreateProduct(null));
    }
    #endregion

    #region UpdatePRroduct

    [Fact]
    public void UpdateProduct_ExistingProduct_ReturnsNonZeroSaveChanges()
    {
        // Arrange
        var productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
        var updatedProductSM = new ProductSM
        {
            ProductId = 1, 
            ProductName = "Updated Product Name",
            CategoryLkpId = 2,
            ActiveFlag = true,
            ImageName = "updated_image.jpg",
            Price = 200,
            ProductCode = "UPDATED-001",
            Uom = "ounce",
            Description = "Updated Product Description"
        };

        var existingProductEntity = new PmsProduct
        {
            ProductId = 1,
            ProductName = "Original Product Name",
            CategoryLkpId = 1,
            ActiveFlag = true,
            Price = 100,
            ProductCode = "PROD-001",
            Uom = "ml",
            Description = "Original Product Description"
        };

        _productRepositoryMock
            .Setup(x => x.GetT(
                It.IsAny<Expression<Func<PmsProduct, bool>>>(),
                It.IsAny<string>()
            ))
            .Returns<Expression<Func<PmsProduct, bool>>, string>((predicate, includeProperties) => existingProductEntity);

        _productRepositoryMock.Setup(x => x.SaveChanges()).Returns(1);

        var result = productService.UpdateProduct(updatedProductSM);

        Assert.NotEqual(0, result);
    }

    #endregion

}
