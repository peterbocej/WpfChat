using Microsoft.EntityFrameworkCore;

using WpfChat.Domain.Model;

namespace WpfChat.WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().HasData(
                new Message()
                {
                    MessageId = 1,
                    From = "Admin",
                    Body = "Initialize."
                });
        }
    }
}
