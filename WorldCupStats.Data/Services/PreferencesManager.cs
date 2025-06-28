using WorldCupStats.Data.Models;

public static class PreferencesManager
{
    private const string PreferencesPath = "userprefs.txt";

    // Define default values here
    public static readonly string DefaultLanguage = "en";
    public static readonly Genre DefaultGenre = Genre.Men;

    public static bool TryLoadPreferences(out string language, out Genre genre)
    {
        language = DefaultLanguage;
        genre = DefaultGenre;

        if (!File.Exists(PreferencesPath))
            return false;

        try
        {
            string[] parts = File.ReadAllText(PreferencesPath).Split('|');
            language = parts[0];
            genre = parts[1].Equals("Women", StringComparison.OrdinalIgnoreCase) ? Genre.Women : Genre.Men;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static void SavePreferences(string language, Genre genre)
    {
        string line = $"{language}|{genre}";
        File.WriteAllText(PreferencesPath, line);
    }
}
