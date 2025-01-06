namespace GerenciadorImobiliario_API.ViewModels.Property
{
    public class CreatePropertyViewModel
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public long OwnerId { get; set; }
        public string Type { get; set; } = string.Empty;
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
        public string Status { get; set; } = string.Empty;
        public AddressViewModel Address { get; set; } = null!;
    }
}
