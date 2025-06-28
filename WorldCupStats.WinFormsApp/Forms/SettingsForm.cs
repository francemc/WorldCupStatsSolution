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

namespace WorldCupStats.WinFormsApp.Forms
{
    public partial class SettingsForm : Form
    {
        private const string PreferencesPath = "userprefs.txt";
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
            if (!File.Exists(PreferencesPath)) return;

            try
            {
                var content = File.ReadAllText(PreferencesPath);
                var parts = content.Split('|');

                if (parts.Length == 2)
                {
                    cmbLanguage.SelectedItem = parts[0] == "hr" ? "Croatian" : "English";
                    cmbChampionship.SelectedItem = parts[1];
                }
            }
            catch
            {
                // Ignore load errors
            }
        }
        private void btnOk_Click_1(object sender, EventArgs e)
        {
            SelectedGenre = cmbChampionship.SelectedItem.ToString() == "Women" ? Genre.Women : Genre.Men;
            SelectedLanguage = cmbLanguage.SelectedItem.ToString() == "Croatian" ? "hr" : "en";
            try
            {
                PreferencesManager.SavePreferences(SelectedLanguage, SelectedGenre);
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

        // Add these methods to SettingsForm.cs
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                btnOk.PerformClick();
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                btnCancel.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
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
