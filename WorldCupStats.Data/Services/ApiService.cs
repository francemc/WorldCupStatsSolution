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

            // Buscar la carpeta raíz de la solución (ajusta si tu estructura es distinta)
            while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "WorldCupStats.Data")))
            {
                dir = dir.Parent;
            }

            if (dir == null)
                throw new DirectoryNotFoundException("No se pudo encontrar la carpeta WorldCupStats.Data desde el directorio actual.");

            string jsonFolderPath = Path.Combine(dir.FullName, @"WorldCupStats.Data\Data\Json");
            return Path.Combine(jsonFolderPath, genreFolder, filename);
        }
        public async Task<List<Team>> GetTeamsAsync(Genre genre)
        {
            if (_settings.UseApi)
            {
                try
                {
                    // API call
                    var endpoint = $"{ BaseApiUrl}/{genre.ToString().ToLower()}/teams";
                    var response = await _httpClient.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var apiResults = JsonSerializer.Deserialize<List<Team>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (apiResults?.Count > 0)
                    {
                        return apiResults;
                    }
                }
                catch (HttpRequestException apiEx)
                {
                    // Log API failure if needed
                    Console.WriteLine($"API request failed: {apiEx.Message}");
                    
                }
            }
           

                // JSON fallback
                try
                {
                    var genreFolder = genre == Genre.Men ? "men" : "women";
                    var jsonPath = GetJsonPath(genreFolder, "teams.json");

                if (!File.Exists(jsonPath))
                    {
                        throw new FileNotFoundException($"File {jsonPath} not found");
                    }

                    var jsonContent = await File.ReadAllTextAsync(jsonPath);
                    return JsonSerializer.Deserialize<List<Team>>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<Team>();
                }
                catch (Exception jsonEx)
                {
                    Console.WriteLine($"JSON fallback failed: {jsonEx.Message}");
                    return new List<Team>();
                }
            
        }

        public async Task<List<Match>> GetMatchesAsync(Genre genre)
        {
            if( _settings.UseApi)
            {
                try
                {
                    // API call
                    var endpoint = $"{BaseApiUrl}/{genre.ToString().ToLower()}/matches";
                    var response = await _httpClient.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var apiMatches = JsonSerializer.Deserialize<List<Match>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (apiMatches?.Count > 0)
                    {
                        return apiMatches;
                    }
                }
                catch (HttpRequestException apiEx)
                {
                    Console.WriteLine($"API request failed: {apiEx.Message}");
                }
            }

            // JSON fallback
            try
            {
                var genreFolder = genre == Genre.Men ? "men" : "women";
                var jsonPath = GetJsonPath(genreFolder, "matches.json");

                if (!File.Exists(jsonPath))
                    throw new FileNotFoundException($"File {jsonPath} not found");

                var jsonContent = await File.ReadAllTextAsync(jsonPath);
                return JsonSerializer.Deserialize<List<Match>>(jsonContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                       ?? new List<Match>();
            }
            catch (Exception jsonEx)
            {
                Console.WriteLine($"JSON fallback failed: {jsonEx.Message}");
                return new List<Match>();
            }
        }

        public async Task<List<Match>> GetMatchesByCountryAsync(Genre genre, string fifaCode)
        {
            if (_settings.UseApi)
            {
                try
                {
                    // API call
                    var endpoint = $"{BaseApiUrl}/{genre.ToString().ToLower()}/matches/country?fifa_code={fifaCode}";
                    var response = await _httpClient.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var apiMatches = JsonSerializer.Deserialize<List<Match>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (apiMatches?.Count > 0)
                    {
                        return apiMatches;
                    }
                }
                catch (HttpRequestException apiEx)
                {
                    Console.WriteLine($"API request failed: {apiEx.Message}");
                }
            }

            // JSON fallback - Filter from all matches
            try
            {
                var allMatches = await GetMatchesAsync(genre);
                return allMatches.Where(m =>
                    m.HomeTeam?.Code?.Equals(fifaCode, StringComparison.OrdinalIgnoreCase) == true ||
                    m.AwayTeam?.Code?.Equals(fifaCode, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();
            }
            catch (Exception jsonEx)
            {
                Console.WriteLine($"JSON fallback failed: {jsonEx.Message}");
                return new List<Match>();
            }
        }
        // mirar
        public async Task<List<TeamResult>> GetTeamsResultsAsync(Genre genre)
        {
            if (_settings.UseApi)
            {
                try
                {
                    // API call
                    var endpoint = $"{BaseApiUrl}/{genre.ToString().ToLower()}/teams/results";
                    var response = await _httpClient.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var apiResults = JsonSerializer.Deserialize<List<TeamResult>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (apiResults?.Count > 0)
                    {
                        return apiResults;
                    }
                }
                catch (HttpRequestException apiEx)
                {
                    // Log API failure if needed
                    Console.WriteLine($"API request failed: {apiEx.Message}");

                }
            }


            // JSON fallback
            try
            {
                var genreFolder = genre == Genre.Men ? "men" : "women";
                var jsonPath = GetJsonPath(genreFolder, "results.json");

                if (!File.Exists(jsonPath))
                {
                    throw new FileNotFoundException($"File {jsonPath} not found");
                }

                var jsonContent = await File.ReadAllTextAsync(jsonPath);
                return JsonSerializer.Deserialize<List<TeamResult>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<TeamResult>();
            }
            catch (Exception jsonEx)
            {
                Console.WriteLine($"JSON fallback failed: {jsonEx.Message}");
                return new List<TeamResult>();
            }

        }
        public async Task<TeamResult?> GetTeamResultAsync(Genre genre, string fifaCode)
        {
            try
            {
                var results = await GetTeamsResultsAsync(genre);
                return results.FirstOrDefault(r =>
                    r.FifaCode?.Equals(fifaCode, StringComparison.OrdinalIgnoreCase) == true);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Player>> GetPlayersByTeamAsync(Genre genre, string fifaCode)
        {
            var matches = await GetMatchesByCountryAsync(genre, fifaCode);

            if (matches == null || matches.Count == 0)
                return new List<Player>();

            var firstMatch = matches.OrderBy(m => m.DateTime).First();

            List<Player> players;

            if (firstMatch.HomeTeam.Code.Equals(fifaCode, StringComparison.OrdinalIgnoreCase))
                players = firstMatch.HomeTeamDetail.StartingEleven.Concat(firstMatch.HomeTeamDetail.Substitutes).ToList();
            else if (firstMatch.AwayTeam.Code.Equals(fifaCode, StringComparison.OrdinalIgnoreCase))
                players = firstMatch.AwayTeamDetail.StartingEleven.Concat(firstMatch.AwayTeamDetail.Substitutes).ToList();
            else
                players = new List<Player>();

            return players;
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
                    team.Points = (result.Wins * 3) + (result.Draws);  // Puedes calcular los puntos si quieres
                }
            }

            return teams;
        }


    }
}