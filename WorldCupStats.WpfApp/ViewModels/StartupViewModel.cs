using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;
using WorldCupStats.WpfApp.ViewModels.Helpers;
using WorldCupStats.WpfApp.ViewModels.Models;
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
            set { _selectedWindowMode = value; OnPropertyChanged(nameof(SelectedWindowMode)); }
        }

        public ICommand SaveCommand { get; }

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
        }

        // Evento que notifica que se guardaron preferencias
        public event EventHandler<PreferencesSavedEventArgs>? PreferencesSaved;

        private void SaveAndContinue()
        {
            PreferencesManager.SavePreferences(SelectedLanguage.Code, SelectedGenre, SelectedWindowMode.DisplayName);

            // Lanzar evento para avisar que ya guardó preferencias
            PreferencesSaved?.Invoke(this, new PreferencesSavedEventArgs(SelectedGenre, SelectedWindowMode.DisplayName));
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

       

    }
}
