using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorImobiliario_API.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnName("Email")
                .HasColumnType("VARCHAR")
                .HasMaxLength(100);

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Image)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Slug)
                .IsUnique();

            builder.Property(x => x.SubscriptionPlan)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.SubscriptionStartDate)
                .IsRequired();

            builder.Property(x => x.SubscriptionEndDate)
                .IsRequired();

            builder.Property(x => x.TrialStartDate)
                .IsRequired();

            builder.Property(x => x.TrialEndDate)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder
               .HasIndex(x => x.Slug, "IX_User_Slug")
               .IsUnique();
        }
    }
}
