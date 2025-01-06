using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorImobiliario_API.Data.Mappings
{
    public class PropertyMapping : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.ToTable("Property");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Description)
                .HasColumnType("TEXT");

            builder.Property(p => p.Type)
                .HasMaxLength(100);

            builder.Property(p => p.Status)
                .HasMaxLength(50);

            builder.HasOne(p => p.Address)
                .WithMany()
                .HasForeignKey(p => p.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Owner)
                .WithMany(c => c.PropertiesForSale)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(p => p.Images)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            builder.Navigation(p => p.Documents)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            builder.Navigation(p => p.ClientPropertyInterests)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
        }
    }
}
