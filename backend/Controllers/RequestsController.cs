using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FeedbackFlow.Models;
using FeedbackFlow.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FeedbackFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RequestsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public RequestsController(AppDbContext db)
        {
            _db = db;
        }

        // Получить все заявки текущего пользователя
        [HttpGet]
        public async Task<IActionResult> GetMyRequests()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return Unauthorized();

            var requests = await _db.Requests
                .Where(r => r.UserId == user.Id)
                .ToListAsync();

            return Ok(requests);
        }

        // Создать новую заявку
        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] RequestDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return Unauthorized();

            var request = new Request
            {
                Title = dto.Title,
                Description = dto.Description,
                Type = dto.Type,
                UserId = user.Id
            };

            _db.Requests.Add(request);
            await _db.SaveChangesAsync();

            return Ok(request);
        }

        // Удалить свою заявку
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMyRequest(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return Unauthorized();

            var request = await _db.Requests.FirstOrDefaultAsync(r => r.Id == id && r.UserId == user.Id);
            if (request == null)
                return NotFound();

            _db.Requests.Remove(request);
            await _db.SaveChangesAsync();
            return Ok();
        }

        // Получить одну заявку по id (только если она принадлежит пользователю)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMyRequest(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return Unauthorized();

            var request = await _db.Requests.FirstOrDefaultAsync(r => r.Id == id && r.UserId == user.Id);
            if (request == null)
                return NotFound();

            return Ok(request);
        }
    }
}
