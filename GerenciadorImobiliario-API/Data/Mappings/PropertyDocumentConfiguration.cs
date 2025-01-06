using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorImobiliario_API.Data.Mappings
{
    public class PropertyDocumentConfiguration : IEntityTypeConfiguration<PropertyDocument>
    {
        public void Configure(EntityTypeBuilder<PropertyDocument> builder)
        {
            builder.ToTable("PropertyDocument");

            builder.HasKey(pd => pd.Id);

            builder.Property(pd => pd.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(pd => pd.Url)
                .IsRequired();

            builder.HasOne(pd => pd.Property)
                .WithMany(p => p.Documents)
                .HasForeignKey(pd => pd.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
