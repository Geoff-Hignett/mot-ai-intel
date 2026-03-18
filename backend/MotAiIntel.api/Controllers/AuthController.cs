using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotAiIntel.api.Data;
using MotAiIntel.api.Models;
using MotAiIntel.api.Services;
using MotAiIntel.api.Dtos;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly AuthService _auth;

    public AuthController(AppDbContext db, AuthService auth)
    {
        _db = db;
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existing = await _db.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (existing != null)
            return BadRequest("User already exists");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = _auth.HashPassword(request.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = _auth.GenerateToken(user);

        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var hash = _auth.HashPassword(request.Password);

        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email && x.PasswordHash == hash);

        if (user == null)
            return Unauthorized();

        var token = _auth.GenerateToken(user);

        return Ok(new { token });
    }
}