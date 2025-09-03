using BarraTour.Api.Features.Auth.DTOs;
using BarraTour.Api.Features.Auth.Services;
using BarraTour.Api.Features.Logs.Services;
using BarraTour.Api.Responses;
using BarraTour.Api.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarraTour.Api.Features.Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, ILogService logService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto requestDto)
    {
        try
        {
            var result = await authService.LoginAsync(requestDto);
            
            if (result == null)
            {
                await logService.LogActionAsync(
                    Guid.Empty,
                    "Tentativa de login falhou: credenciais inválidas",
                    "Authentication",
                    null,
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Request.Headers.UserAgent.ToString()
                );
                
                return Unauthorized(ApiResponse<LoginResponseDto>.Fail("Credenciais inválidas"));
            }

            await logService.LogActionAsync(
                result.UserId,
                "Login realizado com sucesso",
                "Authentication",
                null,
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                Request.Headers.UserAgent.ToString()
            );

            return Ok(ApiResponse<LoginResponseDto>.Ok(result, "Login realizado com sucesso"));
        }
        catch (Exception ex)
        {
            await logService.LogActionAsync(
                Guid.Empty,
                $"Erro durante o login: {ex.Message}",
                "Authentication",
                null,
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                Request.Headers.UserAgent.ToString()
            );
            
            return BadRequest(ApiResponse<LoginResponseDto>.Fail("Erro ao realizar login", new[] { ex.Message }));
        }
    }
}