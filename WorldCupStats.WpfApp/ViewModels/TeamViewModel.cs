using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorldCupStats.Data.Models;

namespace WorldCupStats.WpfApp.ViewModels
{
    public class TeamViewModel : INotifyPropertyChanged
    {
        public string Country { get; }
        public string FifaCode { get; }

        public int GamesPlayed { get; }
        public int Wins { get; }
        public int Losses { get; }
        public int Draws { get; }
        public int GoalsFor { get; }
        public int GoalsAgainst { get; }
        public int GoalDifferential { get; }

        public TeamViewModel(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));

            Country = team.Country ?? "Unknown";
            FifaCode = team.FifaCode ?? "N/A";

            GamesPlayed = team.GamesPlayed ?? 0;
            Wins = team.Wins ?? 0;
            Losses = team.Losses ?? 0;
            Draws = team.Draws ?? 0;
            GoalsFor = team.GoalsFor ?? 0;
            GoalsAgainst = team.GoalsAgainst ?? 0;
            GoalDifferential = team.GoalDifferential ?? (GoalsFor - GoalsAgainst);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
