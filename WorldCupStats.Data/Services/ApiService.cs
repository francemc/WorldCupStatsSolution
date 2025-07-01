using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WorldCupStats.Data.Models;

namespace WorldCupStats.Data.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly DataSourceSettings _settings;
        private const string BaseApiUrl = "https://worldcup-vua.nullbit.hr";

        public ApiService(HttpClient httpClient, DataSourceSettings settings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "WorldCupStatsApp");
        }

        private string GetJsonPath(string genreFolder, string filename)
        {
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo? dir = new DirectoryInfo(currentDir);

            while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "WorldCupStats.Data")))
            {
                dir = dir.Parent;
            }

            if (dir == null)
                throw new DirectoryNotFoundException("No se pudo encontrar la carpeta WorldCupStats.Data desde el directorio actual.");

            string jsonFolderPath = Path.Combine(dir.FullName, @"WorldCupStats.Data\Data\Json");
            return Path.Combine(jsonFolderPath, genreFolder, filename);
        }

        private async Task<T?> TryFetchFromApi<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine($"API Error: {response.StatusCode} for endpoint: {endpoint}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Exception for endpoint {endpoint}: {ex.Message}");
            }

            return default;
        }

        private async Task<T> LoadFromJson<T>(string filePath) where T : new()
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"JSON file not found: {filePath}");

                var jsonContent = await File.ReadAllTextAsync(filePath);
                var data = JsonSerializer.Deserialize<T>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new T();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON fallback failed for {filePath}: {ex.Message}");
                return new T();
            }
        }

        public async Task<List<Team>> GetTeamsAsync(Genre genre)
        {
            List<Team>? apiResults = null;

            if (_settings.UseApi)
            {
                var endpoint = $"{BaseApiUrl}/{genre.ToString().ToLower()}/teams";
                apiResults = await TryFetchFromApi<List<Team>>(endpoint);
            }

            if (apiResults != null && apiResults.Count > 0)
                return apiResults;

            var genreFolder = genre == Genre.Men ? "men" : "women";
            var jsonPath = GetJsonPath(genreFolder, "teams.json");
            return await LoadFromJson<List<Team>>(jsonPath);
        }

        public async Task<List<Match>> GetMatchesAsync(Genre genre)
        {
            List<Match>? apiResults = null;

            if (_settings.UseApi)
            {
                var endpoint = $"{BaseApiUrl}/{genre.ToString().ToLower()}/matches";
                apiResults = await TryFetchFromApi<List<Match>>(endpoint);
            }

            if (apiResults != null && apiResults.Count > 0)
                return apiResults;

            var genreFolder = genre == Genre.Men ? "men" : "women";
            var jsonPath = GetJsonPath(genreFolder, "matches.json");
            return await LoadFromJson<List<Match>>(jsonPath);
        }

        public async Task<List<Match>> GetMatchesByCountryAsync(Genre genre, string fifaCode)
        {
            var allMatches = await GetMatchesAsync(genre);

            return allMatches
                .Where(m =>
                    m.HomeTeam?.Code?.Equals(fifaCode, StringComparison.OrdinalIgnoreCase) == true ||
                    m.AwayTeam?.Code?.Equals(fifaCode, StringComparison.OrdinalIgnoreCase) == true)
                .ToList();
        }

        public async Task<List<TeamResult>> GetTeamsResultsAsync(Genre genre)
        {
            List<TeamResult>? apiResults = null;

            if (_settings.UseApi)
            {
                var endpoint = $"{BaseApiUrl}/{genre.ToString().ToLower()}/teams/results";
                apiResults = await TryFetchFromApi<List<TeamResult>>(endpoint);
            }

            if (apiResults != null && apiResults.Count > 0)
                return apiResults;

            var genreFolder = genre == Genre.Men ? "men" : "women";
            var jsonPath = GetJsonPath(genreFolder, "results.json");
            return await LoadFromJson<List<TeamResult>>(jsonPath);
        }

        public async Task<TeamResult?> GetTeamResultAsync(Genre genre, string fifaCode)
        {
            var results = await GetTeamsResultsAsync(genre);

            return results.FirstOrDefault(r =>
                r.FifaCode?.Equals(fifaCode, StringComparison.OrdinalIgnoreCase) == true);
        }

        public async Task<List<Player>> GetPlayersByTeamAsync(Genre genre, string fifaCode)
        {
            var matches = await GetMatchesByCountryAsync(genre, fifaCode);

            if (matches == null || matches.Count == 0)
                return new List<Player>();

            var firstMatch = matches.OrderBy(m => m.DateTime).FirstOrDefault();
            if (firstMatch == null)
                return new List<Player>();

            if (firstMatch.HomeTeam.Code.Equals(fifaCode, StringComparison.OrdinalIgnoreCase))
            {
                return firstMatch.HomeTeamDetail.StartingEleven.Concat(firstMatch.HomeTeamDetail.Substitutes).ToList();
            }
            else if (firstMatch.AwayTeam.Code.Equals(fifaCode, StringComparison.OrdinalIgnoreCase))
            {
                return firstMatch.AwayTeamDetail.StartingEleven.Concat(firstMatch.AwayTeamDetail.Substitutes).ToList();
            }

            return new List<Player>();
        }

        public async Task<List<Team>> GetTeamsWithResultsAsync(Genre genre)
        {
            var teams = await GetTeamsAsync(genre);
            var results = await GetTeamsResultsAsync(genre);

            foreach (var team in teams)
            {
                var result = results.FirstOrDefault(r => r.FifaCode.Equals(team.FifaCode, StringComparison.OrdinalIgnoreCase));
                if (result != null)
                {
                    team.GamesPlayed = result.GamesPlayed;
                    team.Wins = result.Wins;
                    team.Draws = result.Draws;
                    team.Losses = result.Losses;
                    team.GoalsFor = result.GoalsFor;
                    team.GoalsAgainst = result.GoalsAgainst;
                    team.GoalDifferential = result.GoalDifferential;
                    team.Points = (result.Wins * 3) + (result.Draws);
                }
            }

            return teams;
        }
    }
}
