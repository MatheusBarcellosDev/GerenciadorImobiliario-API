using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GerenciadorImobiliario_API.Data.Mappings
{
    public class PipelineStageMapping : IEntityTypeConfiguration<PipelineStage>
    {
        public void Configure(EntityTypeBuilder<PipelineStage> builder)
        {
            builder.ToTable("PipelineStage");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Stage)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<Enums.EPipelineStageEnum>());

            builder.Property(x => x.Name) 
            .IsRequired()
            .HasMaxLength(50);

            builder.HasMany(ps => ps.Leads)
                   .WithOne(l => l.CurrentPipelineStage)
                   .HasForeignKey(l => l.CurrentPipelineStageId);
        }
    }
}
