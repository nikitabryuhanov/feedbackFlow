using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        public class StatusDto { public string Status { get; set; } }

        // Получить все заявки всех пользователей
        [HttpGet("requests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _db.Requests
                .Include(r => r.User)
                .ToListAsync();

            return Ok(requests);
        }

        // Обновить статус заявки
        [HttpPatch("requests/{id}")]
        public async Task<IActionResult> PatchStatus(int id, [FromBody] StatusDto dto)
        {
            var request = await _db.Requests.FindAsync(id);
            if (request == null)
                return NotFound();
            if (string.IsNullOrEmpty(dto.Status))
                return BadRequest();
            request.Status = dto.Status;
            await _db.SaveChangesAsync();
            return Ok(request);
        }

        // Удалить заявку
        [HttpDelete("requests/{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _db.Requests.FindAsync(id);
            if (request == null)
                return NotFound();
            _db.Requests.Remove(request);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
