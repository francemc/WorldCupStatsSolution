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
        private bool _isSaving = false;

        public StartupWindow()
        {
            InitializeComponent();
            var vm = new StartupViewModel();
            vm.PreferencesSaved += (s, e) => PreferencesSaved?.Invoke(this, e);
            DataContext = vm;
            this.KeyDown += StartupWindow_KeyDown;
        }

        private void StartupWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ConfirmAndSave();
            }
            else if (e.Key == Key.Escape)
            {
                CancelAndClose();
            }
        }

        private void ConfirmAndSave()
        {
            if (DataContext is StartupViewModel vm)
            {
                if (vm.SaveCommand.CanExecute(null))
                    vm.SaveCommand.Execute(null);

                _isSaving = true;
                Close();
            }
        }

        private void CancelAndClose()
        {
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Only show confirmation if user is trying to close the window (not when saving)
            if (!_isSaving)
            {
                var result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
            base.OnClosing(e);
        }
    }
}

