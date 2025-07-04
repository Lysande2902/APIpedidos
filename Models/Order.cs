namespace APIPedidos.Models;

public enum OrderState
{
    Pendiente,
    Pagado,
    Enviado,
}

public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public OrderState State { get; set; } = OrderState.Pendiente;
    public decimal Total { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}
