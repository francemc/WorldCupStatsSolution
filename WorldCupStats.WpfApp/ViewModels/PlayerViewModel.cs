using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorldCupStats.Data.Models;

namespace WorldCupStats.WpfApp.ViewModels
{
    public class PlayerViewModel : INotifyPropertyChanged
    {
        public Player Player { get; }

        public int GoalsInMatch { get; }
        public int YellowCards { get; }

        public PlayerViewModel(Player player, int goalsInMatch, int yellowCards)
        {
            Player = player;
            GoalsInMatch = goalsInMatch;
            YellowCards = yellowCards;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
