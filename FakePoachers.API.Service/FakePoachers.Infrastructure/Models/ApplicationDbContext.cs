using Microsoft.EntityFrameworkCore;

namespace FakePoachers.Infrastructure.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}
