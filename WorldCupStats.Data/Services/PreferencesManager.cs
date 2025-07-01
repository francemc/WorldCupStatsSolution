using WorldCupStats.Data.Models;
namespace WorldCupStats.Data.Services
{
    public static class PreferencesManager
    {
        private static readonly string PreferencesPath = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
      "WorldCupStats",
      "userprefs.txt");

        // Define default values here
        public static readonly string DefaultLanguage = "en";
        public static readonly Genre DefaultGenre = Genre.Men;
        public static readonly string DefaultSize = "500,500";

        public static bool TryLoadPreferences(out string language, out Genre genre, out string size)
        {
            language = DefaultLanguage;
            genre = DefaultGenre;
            size = DefaultSize;

            if (!File.Exists(PreferencesPath))
                return false;

            try
            {
                string[] parts = File.ReadAllText(PreferencesPath).Split('|');
                language = parts[0];
                genre = parts[1].Equals("Women", StringComparison.OrdinalIgnoreCase) ? Genre.Women : Genre.Men;
                if (parts.Length > 2) size = parts[2];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void SavePreferences(string language, Genre genre, string size)
        {
            string line = $"{language}|{genre}";
            if (!string.IsNullOrEmpty(size))
            {
                line += $"|{size}";
            }
            else
            {
                line += "|Fullscreen";
            }

                string? directory = Path.GetDirectoryName(PreferencesPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(PreferencesPath, line);
        }

    }
}