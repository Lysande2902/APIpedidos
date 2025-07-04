using APIPedidos.Models;

namespace APIPedidos.Services;

public interface IAuthService
{
    Task<LoginResponse?> AuthenticateAsync(LoginRequest request);
} 