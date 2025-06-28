using System.Text.Json.Serialization;

namespace WorldCupStats.Data.Models
{
    public class TeamInfo
    {
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("goals")]
        public int Goals { get; set; }

        [JsonPropertyName("penalties")]
        public int? Penalties { get; set; }
    }
}