using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;
using WorldCupStats.WpfApp.ViewModels.Helpers;
using WorldCupStats.WpfApp.ViewModels;
using WorldCupStats.WpfApp.Views;
using static WorldCupStats.Data.Services.FavoritesManager;
using WorldCupStats.WpfApp;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
   
    private readonly IDialogService _dialogService;

    private Genre _selectedGenre;

    public Genre SelectedGenre
    {
        get => _selectedGenre;
        private set
        {
            if (_selectedGenre != value)
            {
                _selectedGenre = value;
                OnPropertyChanged(nameof(SelectedGenre));
                OnPropertyChanged(nameof(GenreDisplayText));  // Si usas un texto derivado
            }
        }
    }

    public string GenreDisplayText =>
        _selectedGenre == Genre.Men ? "Men's" : "Women's";


    public ObservableCollection<Team> Teams { get; } = new ObservableCollection<Team>();
    public ObservableCollection<Team> OpponentTeams { get; } = new ObservableCollection<Team>();

    private Team _selectedTeam1;
    public Team SelectedTeam1
    {
        get => _selectedTeam1;
        set
        {
            if (_selectedTeam1 != value)
            {
                _selectedTeam1 = value;
                OnPropertyChanged();
                ((RelayCommand)ShowTeam1DetailsCommand).RaiseCanExecuteChanged();
                LoadOpponentsAsync();
                LoadMatchResultAsync();
            }
        }
    }

    private Team _selectedTeam2;
    public Team SelectedTeam2
    {
        get => _selectedTeam2;
        set
        {
            if (_selectedTeam2 != value)
            {
                _selectedTeam2 = value;
                OnPropertyChanged();
                ((RelayCommand)ShowTeam2DetailsCommand).RaiseCanExecuteChanged();
                LoadMatchResultAsync();
            }
        }
    }

    private string _matchResult;
    public string MatchResult
    {
        get => _matchResult;
        set
        {
            _matchResult = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<PlayerViewModel> _team1Players = new();
    private ObservableCollection<PlayerViewModel> _team2Players = new();

    public ObservableCollection<PlayerViewModel> Team1Players
    {
        get => _team1Players;
        set
        {
            _team1Players = value;
            OnPropertyChanged();
            // Aquí debes agregar las notificaciones a las propiedades filtradas
            UpdateFilteredPlayers();
        }
    }

    public ObservableCollection<PlayerViewModel> Team2Players
    {
        get => _team2Players;
        set
        {
            _team2Players = value;
            OnPropertyChanged();
            // Aquí también
            UpdateFilteredPlayers();
        }
    }

    private ObservableCollection<PlayerViewModel> _team1Goalkeepers = new();
    public ObservableCollection<PlayerViewModel> Team1Goalkeepers
    {
        get => _team1Goalkeepers;
        private set
        {
            _team1Goalkeepers = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<PlayerViewModel> _team1Defenders = new();
    public ObservableCollection<PlayerViewModel> Team1Defenders
    {
        get => _team1Defenders;
        private set
        {
            _team1Defenders = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<PlayerViewModel> _team1Midfielders = new();
    public ObservableCollection<PlayerViewModel> Team1Midfielders
    {
        get => _team1Midfielders;
        private set
        {
            _team1Midfielders = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<PlayerViewModel> _team1Forwards = new();
    public ObservableCollection<PlayerViewModel> Team1Forwards
    {
        get => _team1Forwards;
        private set
        {
            _team1Forwards = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<PlayerViewModel> _team2Goalkeepers = new();
    public ObservableCollection<PlayerViewModel> Team2Goalkeepers
    {
        get => _team2Goalkeepers;
        private set
        {
            _team2Goalkeepers = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<PlayerViewModel> _team2Defenders = new();
    public ObservableCollection<PlayerViewModel> Team2Defenders
    {
        get => _team2Defenders;
        private set
        {
            _team2Defenders = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<PlayerViewModel> _team2Midfielders = new();
    public ObservableCollection<PlayerViewModel> Team2Midfielders
    {
        get => _team2Midfielders;
        private set
        {
            _team2Midfielders = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<PlayerViewModel> _team2Forwards = new();
    public ObservableCollection<PlayerViewModel> Team2Forwards
    {
        get => _team2Forwards;
        private set
        {
            _team2Forwards = value;
            OnPropertyChanged();
        }
    }
    // Comandos para abrir ventana de detalles con animación (simplificada aquí)
    public ICommand ShowTeam1DetailsCommand { get; }
    public ICommand ShowTeam2DetailsCommand { get; }

    public MainViewModel(ApiService apiService, Genre selectedGenre, IDialogService dialogService)
    {
        _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        _selectedGenre = selectedGenre;
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

        ShowTeam1DetailsCommand = new RelayCommand(ShowTeam1Details, () => SelectedTeam1 != null);
        ShowTeam2DetailsCommand = new RelayCommand(ShowTeam2Details, () => SelectedTeam2 != null);

        LoadTeamsAsync();
    }

    private async void LoadTeamsAsync()
    {
        Teams.Clear();
        var teams = await _apiService.GetTeamsWithResultsAsync(_selectedGenre);

        foreach (var team in teams)
            Teams.Add(team);

        if (Teams.Count == 0) return;

        // Intenta cargar favorito
        if (FavoritesManager.TryLoadFavorites(out var favData) && favData != null && !string.IsNullOrEmpty(favData.FavoriteTeamCode))
        {
            var favoriteTeam = Teams.FirstOrDefault(t => t.FifaCode == favData.FavoriteTeamCode);
            if (favoriteTeam != null)
            {
                SelectedTeam1 = favoriteTeam;
                return;
            }
        }

        SelectedTeam1 = Teams[0];
    }

    private async void LoadOpponentsAsync()
    {
        OpponentTeams.Clear();

        if (SelectedTeam1 == null)
            return;

        var matches = await _apiService.GetMatchesByCountryAsync(_selectedGenre, SelectedTeam1.FifaCode);
        var opponentCodes = matches
            .SelectMany(m => new[] { m.HomeTeam.Code, m.AwayTeam.Code })
            .Where(code => code != SelectedTeam1.FifaCode)
            .Distinct();

        foreach (var opponent in Teams.Where(t => opponentCodes.Contains(t.FifaCode)))
            OpponentTeams.Add(opponent);

        // Opcional: preseleccionar el primero si existe
        if (OpponentTeams.Count > 0)
            SelectedTeam2 = OpponentTeams[0];
    }
    private async void LoadMatchResultAsync()
    {
        if (SelectedTeam1 == null || SelectedTeam2 == null) return;

        var matches = await _apiService.GetMatchesAsync(_selectedGenre);
        var match = matches.FirstOrDefault(m =>
            (m.HomeTeam.Code == SelectedTeam1.FifaCode && m.AwayTeam.Code == SelectedTeam2.FifaCode) ||
            (m.HomeTeam.Code == SelectedTeam2.FifaCode && m.AwayTeam.Code == SelectedTeam1.FifaCode));

        if (match != null)
        {
            MatchResult = $"{match.HomeTeam.Goals} : {match.AwayTeam.Goals}";

            Team1Players.Clear();
            Team2Players.Clear();

            if (match.HomeTeam.Code == SelectedTeam1.FifaCode)
            {
                PositionPlayers(match.HomeTeamDetail.StartingEleven, Team1Players, true, match);
                PositionPlayers(match.AwayTeamDetail.StartingEleven, Team2Players, false, match);
            }
            else
            {
                PositionPlayers(match.AwayTeamDetail.StartingEleven, Team1Players, true, match);
                PositionPlayers(match.HomeTeamDetail.StartingEleven, Team2Players, false, match);
            }

            UpdateFilteredPlayers(); // <-- Llama aquí para refrescar las colecciones filtradas
        }

    }
    private void PositionPlayers(List<Player> players, ObservableCollection<PlayerViewModel> collection, bool isLeftSide, Match match)
    {
        int maxX = 800; // ancho del campo (ajústalo a tu layout)
        int maxY = 450; // alto del campo

        foreach (var player in players)
        {
            // Contar goles y tarjetas para este jugador
            int goals = CountPlayerEvents(match, player.Name, "goal");
            int yellowCards = CountPlayerEvents(match, player.Name, "yellow-card");

            // Ajustar posición
            switch (player.Position)
            {
                case "Goalie":
                    player.XPosition = isLeftSide ? 0 : maxX;
                    player.YPosition = 0; // esquina superior
                    break;
                case "Defender":
                    player.XPosition = isLeftSide ? 200 : 600;
                    player.YPosition = 100 + (collection.Count % 4 * 70);
                    break;
                case "Midfield":
                    player.XPosition = isLeftSide ? 350 : 450;
                    player.YPosition = 80 + (collection.Count % 4 * 90);
                    break;
                case "Forward":
                    player.XPosition = isLeftSide ? 500 : 300;
                    player.YPosition = 120 + (collection.Count % 3 * 90);
                    break;
                default:
                    player.XPosition = 400;
                    player.YPosition = 225;
                    break;
            }

            // Agregar el PlayerViewModel
            collection.Add(new PlayerViewModel(player, goals, yellowCards));
        }
    }

    private int CountPlayerEvents(Match match, string playerName, string eventType)
    {
        int count = 0;

        count += match.HomeTeamEvents.Count(e =>
            e.TypeOfEvent == eventType && e.Player.Equals(playerName, StringComparison.OrdinalIgnoreCase));

        count += match.AwayTeamEvents.Count(e =>
            e.TypeOfEvent == eventType && e.Player.Equals(playerName, StringComparison.OrdinalIgnoreCase));

        return count;
    }

    private void ShowTeam1Details()
    {
        if (SelectedTeam1 != null)
            _dialogService.ShowTeamDetails(SelectedTeam1);
    }

    private void ShowTeam2Details()
    {
        if (SelectedTeam2 != null)
            _dialogService.ShowTeamDetails(SelectedTeam2);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private void UpdateFilteredPlayers()
    {
        Team1Goalkeepers = new ObservableCollection<PlayerViewModel>(Team1Players.Where(p => p.Player.Position == "Goalie"));
        Team1Defenders = new ObservableCollection<PlayerViewModel>(Team1Players.Where(p => p.Player.Position == "Defender"));
        Team1Midfielders = new ObservableCollection<PlayerViewModel>(Team1Players.Where(p => p.Player.Position == "Midfield"));
        Team1Forwards = new ObservableCollection<PlayerViewModel>(Team1Players.Where(p => p.Player.Position == "Forward"));

        Team2Goalkeepers = new ObservableCollection<PlayerViewModel>(Team2Players.Where(p => p.Player.Position == "Goalie"));
        Team2Defenders = new ObservableCollection<PlayerViewModel>(Team2Players.Where(p => p.Player.Position == "Defender"));
        Team2Midfielders = new ObservableCollection<PlayerViewModel>(Team2Players.Where(p => p.Player.Position == "Midfield"));
        Team2Forwards = new ObservableCollection<PlayerViewModel>(Team2Players.Where(p => p.Player.Position == "Forward"));
    }
 


}
