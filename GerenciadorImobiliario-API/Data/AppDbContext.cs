using GerenciadorImobiliario_API.Data.Mappings;
using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorImobiliario_API.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}
