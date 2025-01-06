using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorImobiliario_API.Data.Mappings
{
    public class LeadMapping : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> builder)
        {
            builder.ToTable("Lead");
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Name).IsRequired().HasMaxLength(100);
            builder.Property(l => l.Email).IsRequired().HasMaxLength(100);
            builder.Property(l => l.Telephone).HasMaxLength(15);
            builder.Property(l => l.InitialNotes).HasMaxLength(500);
            builder.Property(l => l.DateContacted).IsRequired();
            builder.Property(l => l.UserId).IsRequired();
            builder.Property(l => l.LeadStatusId).IsRequired();
            builder.Property(l => l.CurrentPipelineStageId).IsRequired();
            builder.Property(l => l.LastInteractionDate);
            builder.Property(l => l.IsActive).IsRequired();

            builder.HasOne(l => l.User)
                .WithMany(u => u.Leads)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.LeadStatus)
                .WithMany()
                .HasForeignKey(l => l.LeadStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.CurrentPipelineStage)
                .WithMany()
                .HasForeignKey(l => l.CurrentPipelineStageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Client)
                .WithOne()
                .HasForeignKey<Lead>(l => l.ClientId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
