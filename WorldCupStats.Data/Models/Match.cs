using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text.Json.Serialization;
using WorldCupStats.Data.Models; 

namespace WorldCupStats.Data.Models
{
    public class Match
    {
        [JsonPropertyName("fifa_id")]
        public string FifaId { get; set; } = string.Empty;

        [JsonPropertyName("venue")]
        public string Venue { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("time")]
        public string Time { get; set; } = string.Empty;

        [JsonPropertyName("attendance")]
        public string Attendance { get; set; } = string.Empty;

        [JsonPropertyName("home_team_country")]
        public string HomeTeamCountry { get; set; } = string.Empty;

        [JsonPropertyName("away_team_country")]
        public string AwayTeamCountry { get; set; } = string.Empty;

        [JsonPropertyName("datetime")]
        public DateTime DateTime { get; set; }

        [JsonPropertyName("winner")]
        public string Winner { get; set; } = string.Empty;

        [JsonPropertyName("winner_code")]
        public string WinnerCode { get; set; } = string.Empty;

        [JsonPropertyName("home_team")]
        public TeamInfo HomeTeam { get; set; } = new TeamInfo();

        [JsonPropertyName("away_team")]
        public TeamInfo AwayTeam { get; set; } = new TeamInfo();

        [JsonPropertyName("home_team_events")]
        public List<TeamEvent> HomeTeamEvents { get; set; } = new List<TeamEvent>();

        [JsonPropertyName("away_team_events")]
        public List<TeamEvent> AwayTeamEvents { get; set; } = new List<TeamEvent>();

        [JsonPropertyName("home_team_statistics")]
        public TeamPlayers HomeTeamDetail { get; set; } = new TeamPlayers();

        [JsonPropertyName("away_team_statistics")]
        public TeamPlayers AwayTeamDetail { get; set; } = new TeamPlayers();
    }


}