namespace GerenciadorImobiliario_API.Models
{
    public class PropertyPreference
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public Client Client { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public int MinBedrooms { get; set; }
        public int MaxBedrooms { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string AdditionalPreferences { get; set; }
    }
}
