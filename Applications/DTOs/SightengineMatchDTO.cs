using System.Text.Json.Serialization;

namespace BolosDoJacquin.Applications.DTOs
{
    public class SightengineMatchDTO
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("intensity")]
        public string? Intensity { get; set; }
        [JsonPropertyName("match")]
        public string? Match { get; set; }
    }
}
