using APIPedidos.Models;

namespace APIPedidos.Services;

public interface IValidationService
{
    (bool IsValid, List<string> Errors) ValidateProduct(Product product);
    (bool IsValid, List<string> Errors) ValidateOrderItem(int productId, int quantity);
    (bool IsValid, List<string> Errors) ValidateOrderState(string state);
    (bool IsValid, List<string> Errors) ValidatePagination(int pageNumber, int pageSize);
    (bool IsValid, List<string> Errors) ValidateLoginRequest(LoginRequest request);
    bool IsValidProductName(string name);
    bool IsValidProductDescription(string description);
} 