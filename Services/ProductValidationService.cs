using APIPedidos.Data;
using APIPedidos.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPedidos.Services;

public class ProductValidationService : IProductValidationService
{
    private readonly ApplicationDbContext _context;

    public ProductValidationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CanDeleteProductAsync(int productId)
    {
        var isInAnyOrder = await _context.OrderItems
            .AnyAsync(item => item.ProductId == productId);
        
        // Retorna true si el producto NO está en ninguna orden (se puede eliminar)
        // Retorna false si el producto está en alguna orden (no se puede eliminar)
        return !isInAnyOrder;
    }
} 