using GerenciadorImobiliario_API.Data.Mappings;
using GerenciadorImobiliario_API.Enums;
using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GerenciadorImobiliario_API.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new SubscriptionPlanMapping());
            modelBuilder.ApplyConfiguration(new UserSubscriptionMapping());

            modelBuilder.Entity<SubscriptionPlan>()
                        .Property(x => x.Name)
                        .HasConversion(new EnumToStringConverter<ESubscriptionPlan>());

        }
    }
}
