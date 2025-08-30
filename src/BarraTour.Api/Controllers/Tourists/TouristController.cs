using BarraTour.Api.DTOs.Tourists;
using BarraTour.Api.Responses;
using BarraTour.Api.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace BarraTour.Api.Controllers.Tourists;

[ApiController]
[Route("api/tourist")]
public class TouristController(ITouristServices touristService, ILogger<TouristController> logger)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<ReadTouristDto>>> CreateTourist([FromBody] CreateTouristDto dto)
    {
        try
        {
            var tourist = await touristService.CreateTouristAsync(dto);
            return Ok(ApiResponse<ReadTouristDto>.Ok(tourist, "Turista criado com sucesso"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao criar turista");
            return BadRequest(ApiResponse<ReadTouristDto>.Fail("Erro ao criar turista", [ex.Message]));
        }
    }


    // GET: api/tourist/{id}
    // [HttpGet("{id}")]
    // public async Task<ActionResult<ReadTouristDto>> GetTouristById(Guid id)
    // {
    //     var tourist = await touristService.GetTouristByIdAsync(id);
    //     if (tourist == null)
    //         return NotFound();
    //
    //     return Ok(tourist);
    // }
    //
    // // GET: api/tourist/email/{email} (opcional)
    // [HttpGet("email/{email}")]
    // public async Task<ActionResult<ReadTouristDto>> GetTouristByEmail(string email)
    // {
    //     var tourist = await touristService.GetTouristByEmailAsync(email);
    //     if (tourist == null)
    //         return NotFound();
    //
    //     return Ok(tourist);
    // }
}
