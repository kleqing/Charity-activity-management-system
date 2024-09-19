using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("f5f3b3b3-3b3b-4b3b-8b3b-3b3b3b3b3b3b"),
                    Name = "John Doe",
                    Email = "testing@gmail.com",
                    Address = "1234 Main St",
                    Phone = "1234567890",
                    Dob = new DateTime(1990, 1, 1),
                    RoleId = 1,
                    Avatar = "https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50"
                });
        }
    }
}
