namespace APIPedidos.Services;

public interface IProductValidationService
{
    Task<bool> CanDeleteProductAsync(int productId);
} 