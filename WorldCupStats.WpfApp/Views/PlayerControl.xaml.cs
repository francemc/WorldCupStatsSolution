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
using WorldCupStats.WpfApp.Views;

namespace WorldCupStats.WpfApp
{
    /// <summary>
    /// Lógica de interacción para PlayerControl.xaml
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        public PlayerControl()
        {
            InitializeComponent();
        }

        private void PlayerControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is Player player)
            {
                // Aquí puedes abrir la ventana de detalles del jugador
                var detailsWindow = new PlayerDetailsWindows();
                // Suponiendo que tienes un ViewModel para detalles (PlayerDetailsViewModel)
                detailsWindow.DataContext = new PlayerDetailsViewModel(
                    player,
                    goals: player.GoalsScored,       // Ajusta según datos disponibles
                    yellowCards: player.YellowCards
                );

                detailsWindow.Owner = Window.GetWindow(this);
                detailsWindow.Opacity = 0;
                detailsWindow.Show();

                var animation = new System.Windows.Media.Animation.DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
                detailsWindow.BeginAnimation(Window.OpacityProperty, animation);
            }
        }
    }

}
