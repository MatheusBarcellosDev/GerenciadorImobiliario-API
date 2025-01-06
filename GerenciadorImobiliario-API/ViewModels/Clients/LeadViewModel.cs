using GerenciadorImobiliario_API.Models;

namespace GerenciadorImobiliario_API.ViewModels.Clients
{
    public class LeadViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string InitialNotes { get; set; } = string.Empty;
        public DateTime DateContacted { get; set; }
        public string? LeadStatus { get; set; }
        public string? CurrentPipelineStage { get; set; }
        public Address? Address { get; set; } 
        public List<PropertyPreferenceViewModel>? PropertyPreferences { get; set; }
    }

}
