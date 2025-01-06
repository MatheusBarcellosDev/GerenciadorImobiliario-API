using Microsoft.AspNetCore.Identity;
using System.Net;

namespace GerenciadorImobiliario_API.Models
{
    public class User : IdentityUser<long>
    {
        // Informações Pessoais
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public string Slug { get; set; } = string.Empty;

        // Informações de Contato
        public new string? PhoneNumber { get; set; } 
        public long? AddressId { get; set; }
        public Address? Address { get; set; }

        // Informações Profissionais
        public string? Creci { get; set; }
        public string? Specialties { get; set; }
        public int YearsOfExperience { get; set; }

        // Informações Adicionais
        public string? Description { get; set; }
        public string? LinkedIn { get; set; }
        public string? Instagram { get; set; }

        // Assinatura
        public long? SubscriptionPlanId { get; set; }
        public SubscriptionPlan? SubscriptionPlan { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Período de Teste
        public DateTime? TrialStartDate { get; set; }
        public DateTime? TrialEndDate { get; set; }
        public bool TrialUsed { get; set; } = false;

        // Propriedades de Auditoria
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Relacionamentos
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }

}
