using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;
using static WorldCupStats.Data.Services.FavoritesManager;

namespace WorldCupStats.WinFormsApp.Forms
{
    public partial class SettingsForm : Form
    {
       
        public Genre SelectedGenre { get; private set; }
        public string SelectedLanguage { get; private set; }
        public SettingsForm()
        {
            InitializeComponent();
            // Add these lines
            this.KeyPreview = true;
            this.KeyDown += SettingsForm_KeyDown;

            // Load current settings
            cmbChampionship.Items.AddRange(new[] { "Men", "Women" });
            cmbLanguage.Items.AddRange(new[] { "English", "Croatian" });

            cmbChampionship.SelectedIndex = 0;
            cmbLanguage.SelectedIndex = 0;

            LoadExistingPreferences();
        }

        private void LoadExistingPreferences()
        {
            if (PreferencesManager.TryLoadPreferences(out string language, out Genre genre, out string size))
            {
                cmbLanguage.SelectedItem = language == "hr" ? "Croatian" : "English";
                cmbChampionship.SelectedItem = genre == Genre.Women ? "Women" : "Men";
            }
        }
        private void btnOk_Click_1(object sender, EventArgs e)
        {
            SelectedGenre = cmbChampionship.SelectedItem.ToString() == "Women" ? Genre.Women : Genre.Men;
            SelectedLanguage = cmbLanguage.SelectedItem.ToString() == "Croatian" ? "hr" : "en";
            try
            {
                PreferencesManager.SavePreferences(SelectedLanguage, SelectedGenre, "");
                if (SelectedGenre.ToString() != cmbChampionship.SelectedItem.ToString())
                {
                    var dat = new FavoritesData();
                    FavoritesManager.SaveFavorites(dat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save preferences: {ex.Message}");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {

            DialogResult = DialogResult.Cancel;
            Close();

        }


        private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk.PerformClick();
                e.Handled = e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel.PerformClick();
                e.Handled = e.SuppressKeyPress = true;
            }
        }


    }
}
