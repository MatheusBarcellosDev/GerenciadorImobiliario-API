using GerenciadorImobiliario_API.Enums;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorImobiliario_API.ViewModels.SubscriptionPlans
{
    public class CreateUpdateSubscriptionPlanViewModel
    {
        [Required(ErrorMessage = "O nome do plano é obrigatório")]
        public ESubscriptionPlan Name { get; set; }

        [Required(ErrorMessage = "O preço do plano é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "O preço deve ser maior ou igual a zero")]
        public decimal Price { get; set; }

        public string[] Features { get; set; } = Array.Empty<string>();
    }
}

