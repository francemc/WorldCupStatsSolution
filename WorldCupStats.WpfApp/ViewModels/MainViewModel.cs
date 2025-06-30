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

public class MainViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private readonly Genre _selectedGenre;

    public Genre SelectedGenre => _selectedGenre; // Expuesto como solo lectura

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
                LoadOpponentsAsync();
                // Aquí podrías cargar jugadores si quieres
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

    private ObservableCollection<Player> _team1Players = new();
    private ObservableCollection<Player> _team2Players = new();

    public ObservableCollection<Player> Team1Players
    {
        get => _team1Players;
        set { _team1Players = value; OnPropertyChanged(); }
    }

    public ObservableCollection<Player> Team2Players
    {
        get => _team2Players;
        set { _team2Players = value; OnPropertyChanged(); }
    }

    // Comandos para abrir ventana de detalles con animación (simplificada aquí)
    public ICommand ShowTeam1DetailsCommand { get; }
    public ICommand ShowTeam2DetailsCommand { get; }

    public MainViewModel(ApiService apiService, Genre selectedGenre)
    {
        _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        _selectedGenre = selectedGenre;

        ShowTeam1DetailsCommand = new RelayCommand(ShowTeam1Details, () => SelectedTeam1 != null);
        ShowTeam2DetailsCommand = new RelayCommand(ShowTeam2Details, () => SelectedTeam2 != null);

        LoadTeamsAsync();
    }

    private async void LoadTeamsAsync()
    {
        Teams.Clear();
        var teams = await _apiService.GetTeamsAsync(_selectedGenre);
        foreach (var team in teams)
            Teams.Add(team);

        // Opcional: preseleccionar el primer equipo para evitar nulos
        if (Teams.Count > 0)
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

            // Position players on field
            Team1Players.Clear();
            Team2Players.Clear();

            if (match.HomeTeam.Code == SelectedTeam1.FifaCode)
            {
                PositionPlayers(match.HomeTeamDetail.StartingEleven, Team1Players, isLeftSide: true);
                PositionPlayers(match.AwayTeamDetail.StartingEleven, Team2Players, isLeftSide: false);
            }
            else
            {
                PositionPlayers(match.AwayTeamDetail.StartingEleven, Team1Players, isLeftSide: true);
                PositionPlayers(match.HomeTeamDetail.StartingEleven, Team2Players, isLeftSide: false);
            }
        }
    }
    private void PositionPlayers(List<Player> players, ObservableCollection<Player> collection, bool isLeftSide)
    {
        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];

            switch (player.Position)
            {
                case "Goalie":
                    player.XPosition = isLeftSide ? 100 : 700;
                    player.YPosition = 225;
                    break;
                case "Defender":
                    player.XPosition = isLeftSide ? 200 : 600;
                    player.YPosition = 100 + (i % 4 * 70);
                    break;
                case "Midfield":
                    player.XPosition = isLeftSide ? 350 : 450;
                    player.YPosition = 80 + (i % 4 * 90);
                    break;
                case "Forward":
                    player.XPosition = isLeftSide ? 500 : 300;
                    player.YPosition = 120 + (i % 3 * 90);
                    break;
                default:
                    player.XPosition = 400;
                    player.YPosition = 225;
                    break;
            }

            collection.Add(player);
        }
    }

    private void ShowTeam1Details()
    {
        if (SelectedTeam1 == null) return;
        OpenTeamDetailsWindow(SelectedTeam1);
    }

    private void ShowTeam2Details()
    {
        if (SelectedTeam2 == null) return;
        OpenTeamDetailsWindow(SelectedTeam2);
    }

    private void OpenTeamDetailsWindow(Team team)
    {
        var window = new TeamWindow();
        var vm = new TeamViewModel(team);
        window.DataContext = vm;

        // Animación simple: fade in (puedes mejorar con Storyboards)
        window.Opacity = 0;
        window.Show();

        var animation = new System.Windows.Media.Animation.DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
        window.BeginAnimation(Window.OpacityProperty, animation);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
