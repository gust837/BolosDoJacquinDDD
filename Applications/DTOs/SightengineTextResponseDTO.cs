using System.Text.Json.Serialization;

namespace BolosDoJacquin.Applications.DTOs
{
    public class SightengineTextResponseDTO
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("profanity")]
        public SightengineCategoryDTO? Profanity { get; set; }
        [JsonPropertyName("personal")]
        public SightengineCategoryDTO? Personal { get; set; }
        [JsonPropertyName("link")]
        public SightengineCategoryDTO? Link { get; set; }
        [JsonPropertyName("extremism")]
        public SightengineCategoryDTO? Extremism { get; set; }
        [JsonPropertyName("weapon")]
        public SightengineCategoryDTO? Weapon { get; set; }
        [JsonPropertyName("drug")]
        public SightengineCategoryDTO? Drug { get; set; }
        [JsonPropertyName("medical")]
        public SightengineCategoryDTO? Medical { get; set; }
        [JsonPropertyName("self-harm")]
        public SightengineCategoryDTO? SelfHarm { get; set; }
        [JsonPropertyName("violence")]
        public SightengineCategoryDTO? Violence { get; set; }
        [JsonPropertyName("spam")]
        public SightengineCategoryDTO? Spam { get; set; }
        [JsonIgnore]
        public bool ContemViolacao =>
            (Profanity?.Matches?.Count ?? 0) > 0 ||
            (Personal?.Matches?.Count ?? 0) > 0 ||
            (Link?.Matches?.Count ?? 0) > 0 ||
            (Extremism?.Matches?.Count ?? 0) > 0 ||
            (Weapon?.Matches?.Count ?? 0) > 0 ||
            (Drug?.Matches?.Count ?? 0) > 0 ||
            (Medical?.Matches?.Count ?? 0) > 0 ||
            (SelfHarm?.Matches?.Count ?? 0) > 0 ||
            (Violence?.Matches?.Count ?? 0) > 0 ||
            (Spam?.Matches?.Count ?? 0) > 0;
    }
}
