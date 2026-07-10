using Microsoft.EntityFrameworkCore;

using Serilog;

using WpfChat.WebApi.Model;

namespace WpfChat.WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
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
