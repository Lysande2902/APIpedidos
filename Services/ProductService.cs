using APIPedidos.Data;
using APIPedidos.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPedidos.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IProductValidationService _validationService;

    public ProductService(ApplicationDbContext context, IProductValidationService validationService)
    {
        _context = context;
        _validationService = validationService;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        // Validar que no exista un producto con el mismo nombre
        var existingProduct = await _context.Products
            .FirstOrDefaultAsync(p => p.Name.ToLower() == product.Name.ToLower());

        if (existingProduct != null)
        {
            throw new InvalidOperationException(ValidationMessages.ProductNameDuplicate);
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
            return null;

        // Validar que no exista otro producto con el mismo nombre (excluyendo el actual)
        var duplicateProduct = await _context.Products
            .FirstOrDefaultAsync(p => p.Name.ToLower() == product.Name.ToLower() && p.Id != id);

        if (duplicateProduct != null)
        {
            throw new InvalidOperationException(ValidationMessages.ProductNameDuplicate);
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.StockQuantity = product.StockQuantity;

        await _context.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        // Regla de Negocio: No permitir eliminar un producto que ya se encuentre dentro de una orden
        var canDelete = await _validationService.CanDeleteProductAsync(id);
        if (!canDelete)
        {
            throw new InvalidOperationException(ValidationMessages.ProductInOrders);
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PaginatedResult<Product>> GetProductsPaginatedAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Products.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var products = await _context.Products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<Product>
        {
            Items = products,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }
} 