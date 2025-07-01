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
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WorldCupStats.WpfApp.ViewModels;
using System.ComponentModel;


namespace WorldCupStats.WpfApp.Views
{

    /// <summary>
    /// Lógica de interacción para StartupWindow.xaml
    /// </summary>
    /// 
    public partial class StartupWindow : Window
    {
        public event EventHandler<PreferencesSavedEventArgs>? PreferencesSaved;
        private bool _isSaveInitiated = false;

        public StartupWindow()
        {
            InitializeComponent();
            var vm = new StartupViewModel();
            vm.PreferencesSaved += (s, e) => PreferencesSaved?.Invoke(this, e);
            DataContext = vm;
            // Suscribirse al evento para actuar localmente
            this.PreferencesSaved += StartupWindow_PreferencesSaved;

            this.Closing += StartupWindow_Closing;
        }



        private void StartupWindow_PreferencesSaved(object? sender, PreferencesSavedEventArgs e)
        {
            _isSaveInitiated = true;  // Para no preguntar al cerrar
            this.Close();
        }
        public void MarkSaveInitiated()
        {
            _isSaveInitiated = true;
        }

        private void StartupWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isSaveInitiated)
            {
                var result = MessageBox.Show(
                    "Do you really want to close the application?",
                    "Confirm Close",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

    }
}

