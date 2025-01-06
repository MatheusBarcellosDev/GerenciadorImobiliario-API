using GerenciadorImobiliario_API.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GerenciadorImobiliario_API.Models
{
    public class Client
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public long? AddressId { get; set; }
        public Address? Address { get; set; }
        public ClientType Type { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public long UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<PropertyPreference> PropertyPreferences { get; set; } = new List<PropertyPreference>();
        public ICollection<Property> PropertiesForSale { get; set; } = new List<Property>();
    }
}
