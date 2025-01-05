using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorImobiliario_API.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Slug).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Image).HasMaxLength(255);
            builder.Property(u => u.IsActive).IsRequired();
            builder.Property(u => u.CreatedAt).IsRequired();
            builder.Property(u => u.UpdatedAt);
            builder.Property(u => u.TrialUsed).IsRequired();

            
            builder.Property(u => u.PhoneNumber).HasMaxLength(15); 
            builder.Property(u => u.Address).HasMaxLength(255);
            builder.Property(u => u.City).HasMaxLength(100);
            builder.Property(u => u.State).HasMaxLength(100);
            builder.Property(u => u.PostalCode).HasMaxLength(10); 
            builder.Property(u => u.Creci).HasMaxLength(20); 
            builder.Property(u => u.Specialties).HasMaxLength(255);
            builder.Property(u => u.YearsOfExperience);
            builder.Property(u => u.Description).HasMaxLength(500); 
            builder.Property(u => u.LinkedIn).HasMaxLength(255);
            builder.Property(u => u.Instagram).HasMaxLength(255);

            // Relacionamento com SubscriptionPlan
            builder.HasOne(u => u.SubscriptionPlan)
                   .WithMany()
                   .HasForeignKey(u => u.SubscriptionPlanId)
                   .OnDelete(DeleteBehavior.SetNull); 

            builder.HasIndex(u => u.Slug).IsUnique();
        }
    }
}
