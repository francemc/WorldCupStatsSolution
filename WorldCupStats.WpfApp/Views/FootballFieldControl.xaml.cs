using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorldCupStats.Data.Models;
using WorldCupStats.WpfApp.ViewModels;
namespace WorldCupStats.WpfApp
{
    /// <summary>
    /// Lógica de interacción para FootballFieldControl.xaml
    /// </summary>
    public partial class FootballFieldControl : UserControl
    {
        public static readonly DependencyProperty Team1PlayersProperty =
            DependencyProperty.Register("Team1Players", typeof(IEnumerable<Player>), typeof(FootballFieldControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty Team2PlayersProperty =
            DependencyProperty.Register("Team2Players", typeof(IEnumerable<Player>), typeof(FootballFieldControl),
                new PropertyMetadata(null));

        public IEnumerable<Player> Team1Players
        {
            get => (IEnumerable<Player>)GetValue(Team1PlayersProperty);
            set => SetValue(Team1PlayersProperty, value);
        }

        public IEnumerable<Player> Team2Players
        {
            get => (IEnumerable<Player>)GetValue(Team2PlayersProperty);
            set => SetValue(Team2PlayersProperty, value);
        }

        public FootballFieldControl()
        {
            InitializeComponent();
        }
    }
}
