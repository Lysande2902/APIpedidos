using APIPedidos.Models;

namespace APIPedidos.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<Product?> UpdateProductAsync(int id, Product product);
    Task<bool> DeleteProductAsync(int id);
    Task<PaginatedResult<Product>> GetProductsPaginatedAsync(int pageNumber, int pageSize);
} 