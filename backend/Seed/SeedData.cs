using FeedbackFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackFlow.Seed
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext db)
        {
            // Применяем миграции (на всякий случай)
            db.Database.Migrate();

            // Если уже есть пользователи — ничего не делаем
            if (db.Users.Any()) return;

            var admin = new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin",
                Email = "admin@example.com"
            };

            var user = new User
            {
                Username = "user",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                Role = "User",
                Email = "user@example.com"
            };

            db.Users.AddRange(admin, user);
            db.SaveChanges();
        }
    }
}
