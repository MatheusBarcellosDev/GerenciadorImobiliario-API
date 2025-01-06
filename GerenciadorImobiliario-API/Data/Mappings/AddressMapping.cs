using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorImobiliario_API.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Street).HasMaxLength(255);
            builder.Property(a => a.City).HasMaxLength(100);
            builder.Property(a => a.State).HasMaxLength(100);
            builder.Property(a => a.Country).HasMaxLength(100);
            builder.Property(a => a.ZipCode).HasMaxLength(10);
        }
    }
}
