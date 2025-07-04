using System.Text.RegularExpressions;
using APIPedidos.Models;

namespace APIPedidos.Services;

public class ValidationService : IValidationService
{
    public (bool IsValid, List<string> Errors) ValidateProduct(Product product)
    {
        var errors = new List<string>();

        // Validar nombre
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            errors.Add(ValidationMessages.ProductNameRequired);
        }
        else if (product.Name.Length < 3 || product.Name.Length > 100)
        {
            errors.Add(ValidationMessages.ProductNameLength);
        }
        else if (!IsValidProductName(product.Name))
        {
            errors.Add(ValidationMessages.ProductNameInvalid);
        }

        // Validar descripción
        if (string.IsNullOrWhiteSpace(product.Description))
        {
            errors.Add(ValidationMessages.ProductDescriptionRequired);
        }
        else if (product.Description.Length < 10 || product.Description.Length > 500)
        {
            errors.Add(ValidationMessages.ProductDescriptionLength);
        }
        else if (!IsValidProductDescription(product.Description))
        {
            errors.Add(ValidationMessages.ProductDescriptionLength);
        }

        // Validar precio
        if (product.Price <= 0)
        {
            errors.Add(ValidationMessages.ProductPricePositive);
        }
        else if (product.Price > 999999.99m)
        {
            errors.Add(ValidationMessages.ProductPriceMax);
        }

        // Validar stock
        if (product.StockQuantity < 0)
        {
            errors.Add(ValidationMessages.ProductStockPositive);
        }
        else if (product.StockQuantity > 999999)
        {
            errors.Add(ValidationMessages.ProductStockMax);
        }

        return (errors.Count == 0, errors);
    }

    public (bool IsValid, List<string> Errors) ValidateOrderItem(int productId, int quantity)
    {
        var errors = new List<string>();

        // Validar ID del producto
        if (productId <= 0)
        {
            errors.Add(ValidationMessages.ProductIdInvalid);
        }

        // Validar cantidad
        if (quantity <= 0)
        {
            errors.Add(ValidationMessages.OrderItemQuantityPositive);
        }
        else if (quantity > 999)
        {
            errors.Add(ValidationMessages.OrderItemQuantityMax);
        }

        return (errors.Count == 0, errors);
    }

    public (bool IsValid, List<string> Errors) ValidateOrderState(string state)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(state))
        {
            errors.Add(ValidationMessages.OrderStateInvalid);
        }
        else if (!Enum.TryParse<OrderState>(state, true, out _))
        {
            errors.Add(ValidationMessages.OrderStateInvalid);
        }

        return (errors.Count == 0, errors);
    }

    public (bool IsValid, List<string> Errors) ValidatePagination(int pageNumber, int pageSize)
    {
        var errors = new List<string>();

        if (pageNumber <= 0)
        {
            errors.Add(ValidationMessages.PageNumberInvalid);
        }
        else if (pageNumber > 10000)
        {
            errors.Add(ValidationMessages.PageNumberMax);
        }

        if (pageSize < 1 || pageSize > 100)
        {
            errors.Add(ValidationMessages.PageSizeInvalid);
        }

        return (errors.Count == 0, errors);
    }

    public (bool IsValid, List<string> Errors) ValidateLoginRequest(LoginRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Username))
        {
            errors.Add(ValidationMessages.UsernameRequired);
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            errors.Add(ValidationMessages.PasswordRequired);
        }

        return (errors.Count == 0, errors);
    }

    public bool IsValidProductName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        
        // Permitir letras, números, espacios, guiones y puntos
        var regex = new Regex(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\-\.]+$");
        return regex.IsMatch(name);
    }

    public bool IsValidProductDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description)) return false;
        
        // Permitir letras, números, espacios, puntuación básica
        var regex = new Regex(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\-\.\,\!\?\(\)]+$");
        return regex.IsMatch(description);
    }
} 