using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorldCupStats.Data.Models;
using WorldCupStats.WpfApp.ViewModels;


namespace WorldCupStats.WpfApp.Views
{
    /// <summary>
    /// Lógica de interacción para PlayerControl.xaml
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        public PlayerControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ClickCommandProperty =
        DependencyProperty.Register(
            "ClickCommand",
            typeof(ICommand),
            typeof(PlayerControl));

        public ICommand ClickCommand
        {
            get => (ICommand)GetValue(ClickCommandProperty);
            set => SetValue(ClickCommandProperty, value);
        }

        private void PlayerControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is PlayerViewModel vm && ClickCommand?.CanExecute(vm) == true)
            {
                ClickCommand.Execute(vm);
            }
        }
        private void PlayerControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is PlayerViewModel playerViewModel)
            {
                var window = new PlayerWindow
                {
                    DataContext = playerViewModel,
                    Owner = Application.Current.MainWindow
                };
                window.ShowDialog();
            }
        }
    }
}
