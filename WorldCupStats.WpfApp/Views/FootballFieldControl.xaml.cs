using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using WorldCupStats.WpfApp.ViewModels;
using System.Windows.Input;

namespace WorldCupStats.WpfApp.Views
{
    public partial class FootballFieldControl : UserControl
    {
        public FootballFieldControl()
        {
            InitializeComponent();
        }

        // Propiedad Team1Players (sin cambios)
        public static readonly DependencyProperty Team1PlayersProperty =
            DependencyProperty.Register("Team1Players", typeof(ObservableCollection<PlayerViewModel>),
                typeof(FootballFieldControl), new PropertyMetadata(null, OnPlayersChanged));

        public ObservableCollection<PlayerViewModel> Team1Players
        {
            get => (ObservableCollection<PlayerViewModel>)GetValue(Team1PlayersProperty);
            set => SetValue(Team1PlayersProperty, value);
        }

        // Propiedad Team2Players (sin cambios)
        public static readonly DependencyProperty Team2PlayersProperty =
            DependencyProperty.Register("Team2Players", typeof(ObservableCollection<PlayerViewModel>),
                typeof(FootballFieldControl), new PropertyMetadata(null, OnPlayersChanged));

        public ObservableCollection<PlayerViewModel> Team2Players
        {
            get => (ObservableCollection<PlayerViewModel>)GetValue(Team2PlayersProperty);
            set => SetValue(Team2PlayersProperty, value);
        }

        // Propiedades filtradas COMO DEPENDENCY PROPERTIES
        public static readonly DependencyProperty Team1GoalkeepersProperty =
            DependencyProperty.Register("Team1Goalkeepers", typeof(ObservableCollection<PlayerViewModel>),
                typeof(FootballFieldControl));

        public ObservableCollection<PlayerViewModel> Team1Goalkeepers
        {
            get => (ObservableCollection<PlayerViewModel>)GetValue(Team1GoalkeepersProperty);
            set => SetValue(Team1GoalkeepersProperty, value);
        }

        public static readonly DependencyProperty Team1DefendersProperty =
            DependencyProperty.Register("Team1Defenders", typeof(ObservableCollection<PlayerViewModel>),
                typeof(FootballFieldControl));

        public ObservableCollection<PlayerViewModel> Team1Defenders
        {
            get => (ObservableCollection<PlayerViewModel>)GetValue(Team1DefendersProperty);
            set => SetValue(Team1DefendersProperty, value);
        }
        public static readonly DependencyProperty Team1MidfieldersProperty =
            DependencyProperty.Register("Team1Midfielders", typeof(ObservableCollection<PlayerViewModel>),
                typeof(FootballFieldControl));

        public ObservableCollection<PlayerViewModel> Team1Midfielders
        {
            get => (ObservableCollection<PlayerViewModel>)GetValue(Team1MidfieldersProperty);
            set => SetValue(Team1MidfieldersProperty, value);
        }
        public static readonly DependencyProperty Team1ForwardsProperty =
            DependencyProperty.Register("Team1Forwards", typeof(ObservableCollection<PlayerViewModel>),
                typeof(FootballFieldControl));

        public ObservableCollection<PlayerViewModel> Team1Forwards
        {
            get => (ObservableCollection<PlayerViewModel>)GetValue(Team1ForwardsProperty);
            set => SetValue(Team1ForwardsProperty, value);
        }
        // Propiedades filtradas COMO DEPENDENCY PROPERTIES
        public static readonly DependencyProperty Team2GoalkeepersProperty =
            DependencyProperty.Register("Team2Goalkeepers", typeof(ObservableCollection<PlayerViewModel>),
                typeof(FootballFieldControl));

        public ObservableCollection<PlayerViewModel> Team2Goalkeepers
        {
            get => (ObservableCollection<PlayerViewModel>)GetValue(Team2GoalkeepersProperty);
            set => SetValue(Team2GoalkeepersProperty, value);
        }

        public static readonly DependencyProperty Team2DefendersProperty =
            DependencyProperty.Register("Team2Defenders", typeof(ObservableCollection<PlayerViewModel>),
                typeof(FootballFieldControl));

        public ObservableCollection<PlayerViewModel> Team2Defenders
        {
            get => (ObservableCollection<PlayerViewModel>)GetValue(Team2DefendersProperty);
            set => SetValue(Team2DefendersProperty, value);
        }
        public static readonly DependencyProperty Team2MidfieldersProperty =
            DependencyProperty.Register("Team2Midfielders", typeof(ObservableCollection<PlayerViewModel>),
                typeof(FootballFieldControl));

        public ObservableCollection<PlayerViewModel> Team2Midfielders
        {
            get => (ObservableCollection<PlayerViewModel>)GetValue(Team2MidfieldersProperty);
            set => SetValue(Team2MidfieldersProperty, value);
        }
        public static readonly DependencyProperty Team2ForwardsProperty =
            DependencyProperty.Register("Team2Forwards", typeof(ObservableCollection<PlayerViewModel>),
                typeof(FootballFieldControl));

        public ObservableCollection<PlayerViewModel> Team2Forwards
        {
            get => (ObservableCollection<PlayerViewModel>)GetValue(Team2ForwardsProperty);
            set => SetValue(Team2ForwardsProperty, value);
        }
     

        private static void OnPlayersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (FootballFieldControl)d;
            control.UpdateFilteredCollections();
        }

        private void UpdateFilteredCollections()
        {
            // Crear NUEVAS colecciones cada vez
            Team1Goalkeepers = FilterPlayers(Team1Players, "Goalkeeper", "GK");
            Team1Defenders = FilterPlayers(Team1Players, "Defender", "DF");
            Team1Midfielders = FilterPlayers(Team1Players, "Midfielder", "MF");
            Team1Forwards = FilterPlayers(Team1Players, "Forward", "FW");

            Team2Goalkeepers = FilterPlayers(Team2Players, "Goalkeeper", "GK");
            Team2Defenders = FilterPlayers(Team2Players, "Defender", "DF");
            Team2Midfielders = FilterPlayers(Team2Players, "Midfielder", "MF");
            Team2Forwards = FilterPlayers(Team2Players, "Forward", "FW");
        }

        private static ObservableCollection<PlayerViewModel> FilterPlayers(
            IEnumerable<PlayerViewModel> source, params string[] positions)
        {
            var result = new ObservableCollection<PlayerViewModel>();
            if (source == null) return result;

            foreach (var player in source)
            {
                if (player?.Player?.Position != null &&
                    positions.Any(p => player.Player.Position.Contains(p)))
                {
                    result.Add(player);
                }
            }
            return result;
        }
        public static readonly DependencyProperty PlayerClickCommandProperty =
        DependencyProperty.Register(
            "PlayerClickCommand",
            typeof(ICommand),
            typeof(FootballFieldControl));

        public ICommand PlayerClickCommand
        {
            get => (ICommand)GetValue(PlayerClickCommandProperty);
            set => SetValue(PlayerClickCommandProperty, value);
        }
    }
}
