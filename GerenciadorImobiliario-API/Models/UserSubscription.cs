using System;
using GerenciadorImobiliario_API.Models;

namespace GerenciadorImobiliario_API.Models
{
    public class UserSubscription
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long SubscriptionPlanId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

