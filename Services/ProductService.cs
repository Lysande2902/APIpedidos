using APIPedidos.Models;

namespace APIPedidos.Services;

public class ProductService : IProductService
{
    private readonly IProductValidationService _validationService;
    private readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop HP Pavilion", Description = "Laptop de 15 pulgadas con procesador Intel i5", Price = 899.99m, StockQuantity = 10 },
        new Product { Id = 2, Name = "Mouse Inalámbrico", Description = "Mouse óptico inalámbrico con sensor de 1200 DPI", Price = 29.99m, StockQuantity = 50 },
        new Product { Id = 3, Name = "Teclado Mecánico", Description = "Teclado mecánico con switches Cherry MX Blue", Price = 89.99m, StockQuantity = 25 },
        new Product { Id = 4, Name = "Monitor 24\"", Description = "Monitor LED de 24 pulgadas Full HD", Price = 199.99m, StockQuantity = 15 },
        new Product { Id = 5, Name = "Auriculares Gaming", Description = "Auriculares con micrófono y cancelación de ruido", Price = 79.99m, StockQuantity = 30 }
    };

    private int _nextProductId = 6;

    public ProductService(IProductValidationService validationService)
    {
        _validationService = validationService;
    }

    public Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return Task.FromResult(_products.AsEnumerable());
    }

    public Task<Product?> GetProductByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<Product> CreateProductAsync(Product product)
    {
        // Regla de Negocio: No permitir agregar productos con nombre repetido
        if (_products.Any(p => p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("A product with this name already exists");
        }

        product.Id = _nextProductId++;
        _products.Add(product);
        return Task.FromResult(product);
    }

    public Task<Product?> UpdateProductAsync(int id, Product product)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Id == id);
        if (existingProduct == null)
            return Task.FromResult<Product?>(null);

        // Regla de Negocio: No permitir cambiar el nombre a uno que ya existe (excepto el mismo producto)
        if (_products.Any(p => p.Id != id && p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("A product with this name already exists");
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.StockQuantity = product.StockQuantity;

        return Task.FromResult<Product?>(existingProduct);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return false;

        // Regla de Negocio: No permitir eliminar un producto que ya se encuentre dentro de una orden
        var canDelete = await _validationService.CanDeleteProductAsync(id);
        if (!canDelete)
        {
            throw new InvalidOperationException("Cannot delete product that is already in existing orders");
        }

        _products.Remove(product);
        return true;
    }
} 