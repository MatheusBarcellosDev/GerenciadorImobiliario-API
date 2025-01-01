using GerenciadorImobiliario_API.Enums;

namespace GerenciadorImobiliario_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; } // Ex: "Básico", "Premium", "Enterprise"
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public DateTime TrialStartDate { get; set; }
        public DateTime TrialEndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
