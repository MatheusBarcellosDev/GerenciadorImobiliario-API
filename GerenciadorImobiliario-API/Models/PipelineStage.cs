using GerenciadorImobiliario_API.Enums;
using System.Text.Json.Serialization;

namespace GerenciadorImobiliario_API.Models
{
    public class PipelineStage
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EPipelineStageEnum Stage { get; set; }
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}
