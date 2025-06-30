using WorldCupStats.Data.Models;

public class PreferencesSavedEventArgs : EventArgs
{
    public Genre SelectedGenre { get; }
    public string WindowSize { get; }

    public PreferencesSavedEventArgs(Genre genre, string windowSize)
    {
        SelectedGenre = genre;
        WindowSize = windowSize;
    }
}
