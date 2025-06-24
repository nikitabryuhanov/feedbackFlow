using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FeedbackFlow.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FeedbackFlow.DTOs;

namespace FeedbackFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return Unauthorized();
            return Ok(new { user.Id, user.Username, user.Email, user.Role });
        }

        [HttpPatch("me")]
        public async Task<IActionResult> UpdateMe([FromBody] UpdateUserDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return Unauthorized();
            if (!string.IsNullOrWhiteSpace(dto.Username))
                user.Username = dto.Username;
            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;
            await _db.SaveChangesAsync();
            return Ok(new { user.Id, user.Username, user.Email, user.Role });
        }
    }
} 