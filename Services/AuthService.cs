using APIPedidos.Models;

namespace APIPedidos.Services;

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;

    public AuthService(IJwtService jwtService, IConfiguration configuration)
    {
        _jwtService = jwtService;
        _configuration = configuration;
    }

    public Task<LoginResponse?> AuthenticateAsync(LoginRequest request)
    {
        var fixedUsername = _configuration["FixedCredentials:Username"];
        var fixedPassword = _configuration["FixedCredentials:Password"];

        if (request.Username == fixedUsername && request.Password == fixedPassword)
        {
            var token = _jwtService.GenerateToken(request.Username);
            var expirationHours = int.Parse(_configuration["JwtSettings:ExpirationHours"] ?? "24");

            return Task.FromResult<LoginResponse?>(
                new LoginResponse
                {
                    Token = token,
                    Username = request.Username,
                    ExpiresAt = DateTime.UtcNow.AddHours(expirationHours),
                }
            );
        }

        return Task.FromResult<LoginResponse?>(null);
    }
}
