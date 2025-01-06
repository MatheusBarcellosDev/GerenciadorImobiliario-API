namespace GerenciadorImobiliario_API.ViewModels.Clients
{
    public class CreateUpdateLeadViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string InitialNotes { get; set; }
        public DateTime DateContacted { get; set; }
        public int LeadStatusId { get; set; }
        public int CurrentPipelineStageId { get; set; }
    }
}
