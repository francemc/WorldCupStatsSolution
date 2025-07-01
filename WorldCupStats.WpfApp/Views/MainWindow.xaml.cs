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

namespace WorldCupStats.WpfApp.Views
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            // Abrir StartupWindow
            var startupWindow = new StartupWindow();

            // Suscribirse al evento PreferencesSaved para recibir configuración y aplicar cambios si quieres

            startupWindow.Show();

            // Cerrar esta ventana (MainWindow)
            this.Close();
        }

        

    }

}
