using APIPedidos.Models;

namespace APIPedidos.Services;

public class ProductValidationService : IProductValidationService
{
    private readonly List<Order> _orders = new();

    public ProductValidationService()
    {
        // En una implementación real, esto vendría de una base de datos
        // Por ahora, usamos una lista compartida
    }

    public void SetOrders(List<Order> orders)
    {
        _orders.Clear();
        _orders.AddRange(orders);
    }

    public Task<bool> CanDeleteProductAsync(int productId)
    {
        var isInAnyOrder = _orders.Any(order => order.Items.Any(item => item.ProductId == productId));
        // Retorna true si el producto NO está en ninguna orden (se puede eliminar)
        // Retorna false si el producto está en alguna orden (no se puede eliminar)
        return Task.FromResult(!isInAnyOrder);
    }
} 