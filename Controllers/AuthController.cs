using APIPedidos.Models;
using APIPedidos.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIPedidos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidationService _validationService;

    public AuthController(IAuthService authService, IValidationService validationService)
    {
        _authService = authService;
        _validationService = validationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login(
        [FromBody] LoginRequest request
    )
    {
        // Validar request
        var (isValid, errors) = _validationService.ValidateLoginRequest(request);
        if (!isValid)
        {
            return BadRequest(
                ApiResponse<LoginResponse>.ErrorResponse(ValidationMessages.InvalidRequest, errors)
            );
        }

        var response = await _authService.AuthenticateAsync(request);

        if (response == null)
        {
            return Unauthorized(
                ApiResponse<LoginResponse>.ErrorResponse(ValidationMessages.InvalidCredentials)
            );
        }

        return Ok(ApiResponse<LoginResponse>.SuccessResponse(response, "Autenticación exitosa"));
    }
}
