using GerenciadorImobiliario_API.Enums;
using System.Text.Json.Serialization;

namespace GerenciadorImobiliario_API.Models
{
    public class LeadStatus
    {
        public long Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ELeadStatusEnum Status { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}
