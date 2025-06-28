using System.Text.Json.Serialization;

namespace WorldCupStats.Data.Models
{
    public class TeamResult
    {
        [JsonPropertyName("fifa_code")]
        public string FifaCode { get; set; } = "";

        [JsonPropertyName("country")]
        public string Country { get; set; } = "";

        [JsonPropertyName("games_played")]
        public int GamesPlayed { get; set; }

        [JsonPropertyName("wins")]
        public int Wins { get; set; }

        [JsonPropertyName("draws")]
        public int Draws { get; set; }

        [JsonPropertyName("losses")]
        public int Losses { get; set; }

        [JsonPropertyName("goals_for")]
        public int GoalsFor { get; set; }

        [JsonPropertyName("goals_against")]
        public int GoalsAgainst { get; set; }

        [JsonPropertyName("goal_differential")]
        public int GoalDifferential { get; set; }
    }
}
