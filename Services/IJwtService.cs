using APIPedidos.Models;

namespace APIPedidos.Services;

public interface IJwtService
{
    string GenerateToken(string username);
    bool ValidateToken(string token);
} 