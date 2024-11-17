using Microsoft.AspNetCore.Mvc;
using ExcursionTickets.Persistence.Interfaces.Repositories;

namespace ExcursionTickets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcursionController : ControllerBase
    {
        private readonly IExcursionRepository _excursionRepository;

        public ExcursionController(IExcursionRepository excursionRepository)
        {
            _excursionRepository = excursionRepository;
        }

        [HttpGet("get-all-excursions")]
        public async Task<IActionResult> GetAllExcursions()
        {
            try
            {
                var excursions = await _excursionRepository.GetAllExcursions();

                return Ok(excursions);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("get-excursion-details/{excursionId}")]
        public async Task<IActionResult> GetExcursionDetails(int excursionId)
        {
            try
            {
                var excursion = await _excursionRepository.GetExcursionDetails(excursionId);

                return Ok(excursion);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
