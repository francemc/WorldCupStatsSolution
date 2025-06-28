using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;
using WorldCupStats.WpfApp.ViewModels.Helpers;
using WorldCupStats.WpfApp.Views;

namespace WorldCupStats.WpfApp.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;


        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private Genre _selectedGenre;
        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                if (_selectedGenre != value)
                {
                    _selectedGenre = value;
                    OnPropertyChanged();
                    LoadTeamsAsync();
                }
            }
        }

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
                    LoadPlayersAsync();
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

        public ObservableCollection<Team> Teams { get; } = new ObservableCollection<Team>();
        public ObservableCollection<Team> OpponentTeams { get; } = new ObservableCollection<Team>();
        public ObservableCollection<Player> SelectedTeam1Players { get; } = new ObservableCollection<Player>();

        public ICommand OpenSettingsCommand { get; }
        public ICommand ViewTeamDetailsCommand { get; }

        public MainViewModel(ApiService apiService)
        {
            _apiService = apiService;
           

             OpenSettingsCommand = new RelayCommand(OpenSettings);
          //5  ViewTeamDetailsCommand = new RelayCommand<Team>(ViewTeamDetails);
          
            // Load initial preferences
            PreferencesManager.TryLoadPreferences(out _, out var genre);
            SelectedGenre = genre;
        }

        private async void LoadTeamsAsync()
        {
            IsLoading = true;
            try
            {
                Teams.Clear();
                var teams = await _apiService.GetTeamsAsync(SelectedGenre);
                foreach (var team in teams)
                {
                    Teams.Add(team);
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void LoadOpponentsAsync()
        {
            if (SelectedTeam1 == null) return;

            IsLoading = true;
            try
            {
                OpponentTeams.Clear();
                var matches = await _apiService.GetMatchesByCountryAsync(SelectedGenre, SelectedTeam1.FifaCode);
                var opponentCodes = matches
                    .SelectMany(m => new[] { m.HomeTeam.Code, m.AwayTeam.Code })
                    .Where(code => code != SelectedTeam1.FifaCode)
                    .Distinct();

                foreach (var team in Teams.Where(t => opponentCodes.Contains(t.FifaCode)))
                {
                    OpponentTeams.Add(team);
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void LoadPlayersAsync()
        {
            if (SelectedTeam1 == null) return;

            IsLoading = true;
            try
            {
                SelectedTeam1Players.Clear();
                var players = await _apiService.GetPlayersByTeamAsync(SelectedGenre, SelectedTeam1.FifaCode);
                foreach (var player in players)
                {
                    SelectedTeam1Players.Add(player);
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void LoadMatchResultAsync()
        {
            if (SelectedTeam1 == null || SelectedTeam2 == null) return;

            IsLoading = true;
            try
            {
                var matches = await _apiService.GetMatchesAsync(SelectedGenre);
                var match = matches.FirstOrDefault(m =>
                    (m.HomeTeam.Code == SelectedTeam1.FifaCode && m.AwayTeam.Code == SelectedTeam2.FifaCode) ||
                    (m.HomeTeam.Code == SelectedTeam2.FifaCode && m.AwayTeam.Code == SelectedTeam1.FifaCode));

                if (match != null)
                {
                   // MatchResult = $"{match.HomeTeam.Goals} : {match.AwayTeam.Goals}";
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OpenSettings()
        {
           // var settingsWindow = new SettingsWindow();
           // settingsWindow.ShowDialog();
        }

        private void ViewTeamDetails(Team team)
        {
            if (team == null) return;

            /*var teamWindow = new TeamWindow();
            var viewModel = new TeamViewModel(team);
            teamWindow.DataContext = viewModel;
            teamWindow.Show();*/
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
