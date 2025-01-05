using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GerenciadorImobiliario_API.Models;

namespace GerenciadorImobiliario_API.Data.Mappings
{
    public class SubscriptionPlanMapping : IEntityTypeConfiguration<SubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
        {
            builder.ToTable("SubscriptionPlan");
            builder.HasKey(sp => sp.Id);

            builder.Property(sp => sp.Name).IsRequired().HasMaxLength(100);
            builder.Property(sp => sp.Price).IsRequired();
            builder.Property(sp => sp.Features).IsRequired().HasConversion(
                v => string.Join(";", v),
                v => v.Split(";", StringSplitOptions.RemoveEmptyEntries)
            );
        }
    }
}
