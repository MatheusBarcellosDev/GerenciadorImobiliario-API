namespace GerenciadorImobiliario_API.Models
{
    public class PropertyDocument
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
        public long PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }
}
