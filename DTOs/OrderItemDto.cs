namespace APIPedidos.DTOs;

public class AddOrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderItemResponseDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
} 