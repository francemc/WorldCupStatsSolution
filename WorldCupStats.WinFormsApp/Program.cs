using System;
using System.Configuration;
using System.Windows.Forms;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;
using WorldCupStats.WinFormsApp.Forms;

namespace WorldCupStats.WinFormsApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Load preferences or ask user if missing
            if (!PreferencesManager.TryLoadPreferences(out string language, out Genre genre, out _))
            {
                using var settingsForm = new SettingsForm();
                if (settingsForm.ShowDialog() != DialogResult.OK)
                    return; // Exit app if user cancels

                language = settingsForm.SelectedLanguage;
                genre = settingsForm.SelectedGenre;

                PreferencesManager.SavePreferences(language, genre, "");
            }

           
            var dataSourceSettings = new DataSourceSettings
            {
                UseApi = bool.TryParse(ConfigurationManager.AppSettings["UseApi"], out bool useApi) && useApi,
                ApiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "https://worldcup-vua.nullbit.hr",
                JsonDataPath = ConfigurationManager.AppSettings["JsonDataPath"] ?? "Data/Json"
            };
            var httpClient = new HttpClient();
            var apiService = new ApiService(httpClient, dataSourceSettings);



            // 5. Finalmente abrir MainForm con los datos
            Application.Run(new MainForm(apiService, genre, language)); 
        }
    }
}
