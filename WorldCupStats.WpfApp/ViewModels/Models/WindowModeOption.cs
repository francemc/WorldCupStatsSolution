using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WorldCupStats.WpfApp.ViewModels.Models
{
    public enum WindowMode
    {
        Fullscreen,
        Windowed720,
        Windowed1080
    }
 

namespace WorldCupStats.WpfApp.ViewModels.Models
    {
        public class WindowModeOption : INotifyPropertyChanged
        {
            public string DisplayName { get; set; }
            public WindowMode Mode { get; set; }

            private bool _isSelected;
            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    if (_isSelected != value)
                    {
                        _isSelected = value;
                        OnPropertyChanged();
                    }
                }
            }

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

            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
