using Microsoft.EntityFrameworkCore;
using FeedbackFlow.Models;

namespace FeedbackFlow
{
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<User> Users => Set<User>();
        public DbSet<Request> Requests => Set<Request>();
    }
}