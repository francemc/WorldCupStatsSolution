using System.Text.Json.Serialization;

namespace WorldCupStats.Data.Models
{
    public class TeamPlayers
    {
        [JsonPropertyName("starting_eleven")]
        public List<Player> StartingEleven { get; set; } = new();

        [JsonPropertyName("substitutes")]
        public List<Player> Substitutes { get; set; } = new();
    }
}