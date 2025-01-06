namespace GerenciadorImobiliario_API.Models
{
    public class Lead
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string InitialNotes { get; set; } = string.Empty;
        public DateTime DateContacted { get; set; }
        public long LeadStatusId { get; set; }
        public LeadStatus LeadStatus { get; set; } = null!;
        public long CurrentPipelineStageId { get; set; }
        public PipelineStage CurrentPipelineStage { get; set; } = null!;
        public long UserId { get; set; }
        public User User { get; set; } = null!;
        public long? ClientId { get; set; }
        public Client? Client { get; set; }
        public DateTime? LastInteractionDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
