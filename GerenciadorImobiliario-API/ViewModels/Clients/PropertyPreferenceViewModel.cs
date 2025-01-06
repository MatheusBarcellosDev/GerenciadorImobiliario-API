namespace GerenciadorImobiliario_API.ViewModels.Clients
{
    public class PropertyPreferenceViewModel
    {
        public string Type { get; set; }
        public string Location { get; set; }
        public int MinBedrooms { get; set; }
        public int MaxBedrooms { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string AdditionalPreferences { get; set; }
    }
}
