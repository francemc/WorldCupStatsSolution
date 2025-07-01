using System;
using System.Windows;
using WorldCupStats.WpfApp.Views;
using WorldCupStats.WpfApp.ViewModels;
using WorldCupStats.Data.Models;
using WorldCupStats.WpfApp;

public class DialogService : IDialogService
{
    public void ShowTeamDetails(Team team)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var window = new TeamWindow
            {
                DataContext = new TeamViewModel(team)
            };

            // Animación fade-in opcional
            window.Opacity = 0;
            window.Show();
            var animation = new System.Windows.Media.Animation.DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            window.BeginAnimation(Window.OpacityProperty, animation);
        });
    }

    public void ShowPlayerDetails(Player player)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var vm = new PlayerViewModel(player, player.GoalsScored, player.YellowCards);
            var window = new PlayerWindow
            {
                DataContext = vm,
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        });
    }
    public bool ShowStartupDialog()
    {
        bool? result = null;

        Application.Current.Dispatcher.Invoke(() =>
        {
            var startupWindow = new StartupWindow();
            result = startupWindow.ShowDialog();
        });

        return result ==true ;
    }

   
}
