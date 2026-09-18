using System.Text.Json.Serialization;

namespace BolosDoJacquin.Applications.DTOs
{
    public class SightengineCategoryDTO
    {
        [JsonPropertyName("matches")]
        public List<SightengineMatchDTO> Matches { get; set; } = new();
    }
}
