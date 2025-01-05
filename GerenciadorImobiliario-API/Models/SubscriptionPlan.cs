using GerenciadorImobiliario_API.Enums;
using System.Text.Json.Serialization;

namespace GerenciadorImobiliario_API.Models
{
    public class SubscriptionPlan
    {
        public long Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ESubscriptionPlan Name { get; set; }
        public decimal Price { get; set; }
        public string[] Features { get; set; } = Array.Empty<string>();
    }
}
