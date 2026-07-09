using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

using Serilog;

using WpfChat.Model;

namespace WpfChat.Data
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
            Message[] messages = [
                new Message()
                {
                    MessageId = 1,
                    From = Properties.Settings.Default.UserName,
                    To = Properties.Settings.Default.FriendName,
                    Body = "Majstrovstvá sveta vo futbale 2026 prebiehajú v USA, Kanade a Mexiku a sú historicky najväčším turnajom – prvýkrát sa ho zúčastňuje 48 tímov, ktoré odohrajú 104 zápasov. Šampionát začal 11. júna a finále je naplánované na 19. júla."
                },
                new Message()
                {
                    MessageId = 2,
                    From = Properties.Settings.Default.FriendName,
                    To = Properties.Settings.Default.UserName,
                    Body = "Do play-off postupujú prví dvaja z každej skupiny + 8 najlepších tretích tímov, spolu 32 mužstiev."
                }];
            modelBuilder.Entity<Message>().HasData(messages);
        }
    }
}
