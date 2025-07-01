using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Windows;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;
using WorldCupStats.WpfApp.Views;
namespace WorldCupStats.WpfApp
{

    public partial class App : Application
    {
        private StartupWindow _setupWindow;
        private DataSourceSettings _dataSourceSettings;

        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);
           
            // Cargar configuración appsettings.json (solo 1 vez)
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();


            _dataSourceSettings = new DataSourceSettings();
            configuration.GetSection(DataSourceSettings.SectionName).Bind(_dataSourceSettings);


            // Cargar preferencias de usuario desde txt
            if (!PreferencesManager.TryLoadPreferences(out string language, out Genre genre, out string size))
            {
                _setupWindow = new StartupWindow();
                _setupWindow.PreferencesSaved += SetupWindow_PreferencesSaved; // Suscribirse aquí

                _setupWindow.Show();
            }
            else
            {
                OpenMainWindow(genre, size);
            }
        }


        
        private void SetupWindow_PreferencesSaved(object? sender, PreferencesSavedEventArgs e)
        {
            _setupWindow.PreferencesSaved -= SetupWindow_PreferencesSaved;

            MessageBox.Show($"Abrir MainWindow con género: {e.SelectedGenre}, tamaño: {e.WindowSize}");

            OpenMainWindow(e.SelectedGenre, e.WindowSize);
            _setupWindow.MarkSaveInitiated();  // Evitar mensaje al cerrar
            _setupWindow.Close();
        }



        private void OpenMainWindow(Genre genre, string size)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(_dataSourceSettings.ApiBaseUrl) };
            var apiService = new ApiService(httpClient, _dataSourceSettings);
            var dialogService = new DialogService();

            var mainViewModel = new MainViewModel(apiService, genre, dialogService);

            var mainWindow = new MainWindow(mainViewModel);

            if (size == "Fullscreen")
                mainWindow.WindowState = WindowState.Maximized;
            else if (size == "1280x720")
            {
                mainWindow.Width = 1280;
                mainWindow.Height = 720;
            }
            else if (size == "1920x1080")
            {
                mainWindow.Width = 1920;
                mainWindow.Height = 1080;
            }

            mainWindow.Show();
        }
    }
}
