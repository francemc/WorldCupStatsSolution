using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;
using WorldCupStats.WpfApp.ViewModels.Helpers;
using WorldCupStats.WpfApp.ViewModels.Models;
using WorldCupStats.WpfApp.ViewModels.Models.WorldCupStats.WpfApp.ViewModels.Models;
using WorldCupStats.WpfApp.Views;

namespace WorldCupStats.WpfApp.ViewModels
{
    public class StartupViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<LanguageOption> AvailableLanguages { get; }
        public ObservableCollection<Genre> Genres { get; }
        public ObservableCollection<WindowModeOption> WindowModes { get; }

      
        private LanguageOption _selectedLanguage; 

        public LanguageOption SelectedLanguage
        {
            get => _selectedLanguage;
            set { _selectedLanguage = value; OnPropertyChanged(nameof(SelectedLanguage)); }
        }

        private Genre _selectedGenre;
        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set { _selectedGenre = value; OnPropertyChanged(nameof(SelectedGenre)); }
        }

        private WindowModeOption _selectedWindowMode;
        public WindowModeOption SelectedWindowMode
        {
            get => _selectedWindowMode;
            set
            {
                if (_selectedWindowMode != value)
                {
                    if (_selectedWindowMode != null)
                        _selectedWindowMode.IsSelected = false;

                    _selectedWindowMode = value;

                    if (_selectedWindowMode != null)
                        _selectedWindowMode.IsSelected = true;

                    OnPropertyChanged(nameof(SelectedWindowMode));
                }
            }
        }


        public ICommand SaveCommand { get; }

        public ICommand CancelCommand { get; }

        public StartupViewModel()
        {
            AvailableLanguages = new ObservableCollection<LanguageOption>
            {
                new LanguageOption("English", "en"),
                new LanguageOption("Croatian", "hr")
            };

            Genres = new ObservableCollection<Genre> { Genre.Men, Genre.Women };

            WindowModes = new ObservableCollection<WindowModeOption>
            {
                new WindowModeOption("Fullscreen", WindowMode.Fullscreen),
                new WindowModeOption("1280x720", WindowMode.Windowed720),
                new WindowModeOption("1920x1080", WindowMode.Windowed1080)
            };

            SelectedLanguage = AvailableLanguages[0];
            SelectedGenre = Genre.Men;
            SelectedWindowMode = WindowModes[0];

            SaveCommand = new RelayCommand(SaveAndContinue);
            CancelCommand = new RelayCommand(Cancel);
        }

        // Evento que notifica que se guardaron preferencias
        public event EventHandler<PreferencesSavedEventArgs>? PreferencesSaved;

        private void SaveAndContinue()
        {
            var result = MessageBox.Show(
                "Do you want to save changes?",
                "Confirm Settings",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                PreferencesManager.SavePreferences(SelectedLanguage.Code, SelectedGenre, SelectedWindowMode.DisplayName);

                // Levantamos el evento PreferencesSaved para que la ventana lo propague
                PreferencesSaved?.Invoke(this, new PreferencesSavedEventArgs(SelectedGenre, SelectedWindowMode.DisplayName));
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        private void Cancel()
        {
            // Cerrar la ventana sin guardar
            var window = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

    }
}
