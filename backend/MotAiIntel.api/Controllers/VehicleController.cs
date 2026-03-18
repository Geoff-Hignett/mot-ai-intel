using Microsoft.AspNetCore.Mvc;
using MotAiIntel.api.Services;

namespace MotAiIntel.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly VehicleService _service;

        public VehicleController(VehicleService service)
        {
            _service = service;
        }

        [HttpGet("{reg}")]
        public async Task<IActionResult> GetVehicle(string reg)
        {
            var userId = GetUserId();
            var result = await _service.GetVehicle(reg, userId);
            return Ok(result);
        }

        private int? GetUserId()
        {
            var claim = User.FindFirst("id");
            return claim != null ? int.Parse(claim.Value) : null;
        }
    }
}
