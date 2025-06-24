using Microsoft.AspNetCore.Mvc;
using FeedbackFlow.Models;
using FeedbackFlow.DTOs;
using FeedbackFlow.Services;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace FeedbackFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext db, JwtService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(new { error = "User already exists" });

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user.Username, user.Role);
            return Ok(new { token, role = user.Role });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return BadRequest(new { error = "Invalid credentials" });

            var token = _jwtService.GenerateToken(user.Username, user.Role);
            return Ok(new { token, role = user.Role });
        }
    }
}
