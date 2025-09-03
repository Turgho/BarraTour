using BarraTour.Api.Features.Admins.Services;
using BarraTour.Api.Features.Users.DTOs;
using BarraTour.Api.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarraTour.Api.Features.Admins.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var usersDto = await adminService.GetAllUsersAsync();

            return Ok(ApiResponse<IEnumerable<UserResponseDto>>.Ok(usersDto!, "Lista de usuários carregada com sucesso"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<IEnumerable<UserResponseDto>>.Fail("Erro ao buscar usuários", [ex.Message]));
        }
    }
}
