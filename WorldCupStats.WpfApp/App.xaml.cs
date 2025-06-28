using System.Configuration;
using System.Data;
using System.Globalization;
using System.Net.Http;
using System.Windows;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;
using WorldCupStats.WpfApp.ViewModels;
using WorldCupStats.WpfApp.Views;

namespace WorldCupStats.WpfApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        if (!PreferencesManager.TryLoadPreferences(out string language, out Genre genre))
        {
           var setupWindow = new StartupWindow();
            setupWindow.Show();
        }
        else
        {
            var main = new MainWindow();
            main.Show();
        }
    }

           

}

