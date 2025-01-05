using GerenciadorImobiliario_API.Enums;

namespace GerenciadorImobiliario_API.ViewModels.SubscriptionPlans
{
    public class SubscriptionPlanViewModel
    {
        public long Id { get; set; }
        public ESubscriptionPlan Name { get; set; }
        public decimal Price { get; set; }
        public string[] Features { get; set; }
    }
}
