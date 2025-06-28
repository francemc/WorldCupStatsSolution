using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorldCupStats.Data.Models
{
    public class TeamEvent
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("type_of_event")]
        public string TypeOfEvent { get; set; } = string.Empty;  // "goal", "substitution-in", etc.

        [JsonPropertyName("player")]
        public string Player { get; set; } = string.Empty;

        [JsonPropertyName("time")]
        public string Time { get; set; } = string.Empty;
    }
}
