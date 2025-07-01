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
        private HttpClient _httpClient;
        private ApiService _apiService;
        private DialogService _dialogService;  // <- Aqu
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
            _httpClient = new HttpClient { BaseAddress = new Uri(_dataSourceSettings.ApiBaseUrl) };

            _apiService = new ApiService(_httpClient, _dataSourceSettings);
            _dialogService = new DialogService();  // <- Aquí


            if (!PreferencesManager.TryLoadPreferences(out string language, out Genre genre, out string size))
            {
                _setupWindow = new StartupWindow();

                // Suscribirse al evento para cuando el usuario guarde las preferencias
                _setupWindow.PreferencesSaved += (sender, args) =>
                {
                    if (args != null)
                    {
                        // Abrir la ventana principal con las preferencias recién guardadas
                        OpenMainWindow(args.SelectedGenre, args.WindowSize);

                    }
                };
                bool? dialogResult = _setupWindow.ShowDialog();

                // Si el usuario canceló, tal vez cerrar la app aquí, o hacer lo que necesites
                if (dialogResult != true)
                {
                    Shutdown();
                    return;
                }

            }
            else
            {

                // Abrir la ventana principal con las preferencias recién guardadas
                OpenMainWindow(genre, size);
                ;
            }

        }






        public void OpenMainWindow(Genre genre, string size)
        {

            var mainViewModel = new MainViewModel(_apiService, genre, _dialogService);
            mainViewModel.OnPreferencesUpdated += (s, e) => HandlePreferencesUpdated();

            var mainWindow = new MainWindow(mainViewModel, _dialogService);
            UpdateWindowSize(mainWindow, size);

            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }
        private void HandlePreferencesUpdated()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    if (PreferencesManager.TryLoadPreferences(out string lang, out Genre newGenre, out string newSize))
                    {
                        var newViewModel = new MainViewModel(_apiService, newGenre, _dialogService);

                        // Suscribir el evento solo una vez
                        newViewModel.OnPreferencesUpdated += (s, e) => HandlePreferencesUpdated();

                        mainWindow.DataContext = newViewModel;
                        UpdateWindowSize(mainWindow, newSize);
                    }
                }
            });
        }

        // En la inicialización
        
        private void UpdateWindowSize(MainWindow window, string size)
        {
            if (size == "Fullscreen")
            {
                window.WindowState = WindowState.Maximized;
                window.WindowStyle = WindowStyle.SingleBorderWindow;

            }
            else if (size == "1280x720")
            {
                window.WindowState = WindowState.Normal;
                window.Width = 1280;
                window.Height = 720;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.MinWidth = 800; // opcional
                window.MinHeight = 600; // opcional
            }
            else if (size == "1920x1080")
            {
                window.WindowState = WindowState.Normal;
                window.Width = 1920;
                window.Height = 1080;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.MinWidth = 800;
                window.MinHeight = 600;
            }
            else
            {
                window.WindowState = WindowState.Normal;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.WindowStyle = WindowStyle.SingleBorderWindow;
            }

        }
    }

}
