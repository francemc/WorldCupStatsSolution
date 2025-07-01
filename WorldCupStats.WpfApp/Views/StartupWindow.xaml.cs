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
        public event EventHandler<PreferencesSavedEventArgs> PreferencesSaved;

        private bool _isSaveInitiated = false;

        public StartupWindow()
        {
            InitializeComponent();
            var vm = new StartupViewModel();
            vm.PreferencesSaved += Vm_PreferencesSaved;
            DataContext = vm;
        }

        private void Vm_PreferencesSaved(object? sender, PreferencesSavedEventArgs e)
        {
            PreferencesSaved?.Invoke(this, e);
            Dispatcher.Invoke(() =>
            {
                DialogResult = true;
                this.Close();
            });
        }

    }
}

