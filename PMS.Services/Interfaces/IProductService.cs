using PMS.Services.ServiceModels;

namespace PMS.Services.Interfaces;

public interface IProductService
{
    ProductSM GetProductById(int productId);
    IEnumerable<ProductSM> GetProducts(int pageSize, int pageNumber, string searchText, out int totalCount);
    int CreateProduct(ProductSM sm);
    int UpdateProduct(ProductSM sm);
    void DeleteProduct(int productId);
}
