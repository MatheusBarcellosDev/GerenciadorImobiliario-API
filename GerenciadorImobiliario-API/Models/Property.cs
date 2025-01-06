namespace GerenciadorImobiliario_API.Models
{
    public class Property
    {
        public long Id { get; set; }
        public long? AddressId { get; set; }
        public Address? Address { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public long OwnerId { get; set; }
        public Client Owner { get; set; } = null!;
        public string? Type { get; set; } 
        public int NumberOfBedrooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int NumberOfParkingSpaces { get; set; }
        public double TotalArea { get; set; }
        public double UsableArea { get; set; }
        public bool IsFurnished { get; set; }
        public bool HasBuiltInWardrobes { get; set; }
        public bool HasPool { get; set; }
        public bool HasBarbecueGrill { get; set; }
        public bool HasBalcony { get; set; }
        public bool HasGarden { get; set; }
        public decimal? RentPrice { get; set; }
        public decimal? CondominiumFee { get; set; }
        public decimal? Iptu { get; set; }
        public string? Status { get; set; } 
        public List<PropertyImage> Images { get; set; } = new List<PropertyImage>();
        public List<PropertyDocument> Documents { get; set; } = new List<PropertyDocument>();
        public long UserId { get; set; } 
        public User User { get; set; } = null!;
        public List<ClientPropertyInterest> ClientPropertyInterests { get; set; } = new List<ClientPropertyInterest>();

    }
}
