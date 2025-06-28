using System.Windows;

namespace WorldCupStats.WpfApp.ViewModels.Models
{
    public enum WindowMode
    {
        Fullscreen,
        Windowed720,
        Windowed1080
    }
    public class WindowModeOption
    {
        public string DisplayName { get; set; }
        public WindowMode Mode { get; set; }

        public WindowModeOption(string displayName, WindowMode mode)
        {
            DisplayName = displayName;
            Mode = mode;
        }

        public Size GetWindowSize()
        {
            return Mode switch
            {
                WindowMode.Windowed720 => new Size(1280, 720),
                WindowMode.Windowed1080 => new Size(1920, 1080),
                _ => new Size(1920, 1080),
            };
        }

        public override string ToString() => DisplayName;
    }
}
