using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorImobiliario_API.Data.Mappings
{
    public class ClientPropertyInterestMapping : IEntityTypeConfiguration<ClientPropertyInterest>
    {
        public void Configure(EntityTypeBuilder<ClientPropertyInterest> builder)
        {
            builder.ToTable("ClientPropertyInterest");

            builder.HasKey(cpi => new { cpi.ClientId, cpi.PropertyId });

            builder.HasOne(cpi => cpi.Client)
                .WithMany()
                .HasForeignKey(cpi => cpi.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cpi => cpi.Property)
                .WithMany(p => p.ClientPropertyInterests)
                .HasForeignKey(cpi => cpi.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
