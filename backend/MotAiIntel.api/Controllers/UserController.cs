using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotAiIntel.api.Data;
using MotAiIntel.api.Dtos;

namespace MotAiIntel.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UserController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserId();

            var user = await _db.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Email,
                user.YearlyMileage,
                user.DrivingType,
                user.MechanicalKnowledge
            });
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateUserProfileRequest request)
        {
            var userId = GetUserId();

            var user = await _db.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            user.YearlyMileage = request.YearlyMileage;
            user.DrivingType = request.DrivingType;
            user.MechanicalKnowledge = request.MechanicalKnowledge;

            await _db.SaveChangesAsync();

            return Ok();
        }

        private int GetUserId()
        {
            var claim = User.FindFirst("id");
            return int.Parse(claim!.Value);
        }
    }
}
