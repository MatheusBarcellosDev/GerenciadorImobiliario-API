namespace GerenciadorImobiliario_API.Models
{
    public class PropertyImage
    {
        public long Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public long PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }
}
