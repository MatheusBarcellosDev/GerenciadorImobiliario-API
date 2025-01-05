using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorImobiliario_API.Data.Mappings
{
    public class UserSubscriptionMapping : IEntityTypeConfiguration<UserSubscription>
    {
        public void Configure(EntityTypeBuilder<UserSubscription> builder)
        {
            builder.ToTable("UserSubscription");
            builder.HasKey(us => us.Id);

            builder.Property(us => us.StartDate).IsRequired();
            builder.Property(us => us.IsActive).IsRequired();

            builder.HasOne(us => us.User)
                   .WithMany()
                   .HasForeignKey(us => us.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(us => us.SubscriptionPlan)
                   .WithMany()
                   .HasForeignKey(us => us.SubscriptionPlanId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
