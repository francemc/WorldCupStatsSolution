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
using System.Windows.Shapes;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;
using WorldCupStats.WpfApp.ViewModels.Helpers;

namespace WorldCupStats.WpfApp.Views
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDialogService _dialogService;
        public event EventHandler OnPreferencesUpdated;

        public MainWindow(MainViewModel viewModel, IDialogService dialogService)
        {
            InitializeComponent();
            DataContext = viewModel;
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

        }

        
        private void OpenSettings()
        {
            bool? result = _dialogService.ShowStartupDialog();

            if (result == true)
            {
                OnPreferencesUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
        private void OpenSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenSettings();
        }
    }
}

