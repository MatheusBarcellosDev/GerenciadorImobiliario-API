namespace GerenciadorImobiliario_API.Models
{
    public class ClientPropertyInterest
    {
        public long ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public long PropertyId { get; set; }
        public Property Property { get; set; } = null!;
        public DateTime InterestDate { get; set; }
    }
}
