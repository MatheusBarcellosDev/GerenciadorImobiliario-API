using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GerenciadorImobiliario_API.Data.Mappings
{
    public class LeadStatusMapping : IEntityTypeConfiguration<LeadStatus>
    {
        public void Configure(EntityTypeBuilder<LeadStatus> builder)
        {
            builder.ToTable("LeadStatus");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<Enums.ELeadStatusEnum>());

            builder.Property(x => x.Name) 
           .IsRequired()
           .HasMaxLength(50);

            builder.HasMany(ls => ls.Leads)
                   .WithOne(l => l.LeadStatus)
                   .HasForeignKey(l => l.LeadStatusId);
        }
    }
}
