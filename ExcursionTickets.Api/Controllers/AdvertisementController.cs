using ExcursionTickets.Persistence.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExcursionTickets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementRepository _advertisementRepository;

        public AdvertisementController(IAdvertisementRepository advertisementRepository)
        {
            _advertisementRepository = advertisementRepository;
        }

        [HttpGet("get-advertisement/advertisement")]
        public async Task<IActionResult> GetAdvertisements()
        {
            try
            {
                var advertisements = await _advertisementRepository.GetAdvertisements();

                return Ok(advertisements);
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
