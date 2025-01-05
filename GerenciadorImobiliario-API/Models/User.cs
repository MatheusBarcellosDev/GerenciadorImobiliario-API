using Microsoft.AspNetCore.Identity;

namespace GerenciadorImobiliario_API.Models
{
    public class User : IdentityUser<long>
    {
        // Informações Pessoais
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public string Slug { get; set; } = string.Empty;

        // Informações de Contato
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }

        // Informações Profissionais
        public string? Creci { get; set; } // Número de registro no CRECI
        public string? Specialties { get; set; } // Áreas de especialidade
        public int YearsOfExperience { get; set; } // Anos de experiência

        // Informações Adicionais
        public string? Description { get; set; } // Breve descrição ou biografia
        public string? LinkedIn { get; set; } // Link para o perfil do LinkedIn
        public string? Instagram { get; set; } // Link para o perfil do Instagram

        // Assinatura
        public long? SubscriptionPlanId { get; set; } // Chave estrangeira para SubscriptionPlan
        public SubscriptionPlan? SubscriptionPlan { get; set; } // Propriedade de navegação
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool IsActive { get; set; } = true; // Indica se a assinatura está ativa

        // Período de Teste
        public DateTime? TrialStartDate { get; set; }
        public DateTime? TrialEndDate { get; set; }

        // Propriedade adicional para controlar se o período de teste foi usado
        public bool TrialUsed { get; set; } = false;

        // Propriedades de Auditoria
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
