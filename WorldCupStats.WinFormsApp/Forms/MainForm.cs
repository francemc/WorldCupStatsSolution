using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;

namespace WorldCupStats.WinFormsApp.Forms
{
    public partial class MainForm : Form
    {
        private readonly ApiService _api;
        private  Genre _genre;
        private readonly string _language;
        

        public MainForm(ApiService apiService, Genre genre, string language)
        {
            InitializeComponent();
            ;  // Initialize the service
            _api = apiService;
            _genre = genre;
            _language = language;
          

        }

        private async void MainForm_LoadAsync(object sender, EventArgs e)
        {
            
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm())
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    _genre = settingsForm.SelectedGenre; 
                    MessageBox.Show("Configuración guardada correctamente.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnFavorites_Click(object sender, EventArgs e)
        {
            using (var favoritesForm = new FavoriteForm(_api, _genre))
            {
                if (favoritesForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Favoritos seleccionados correctamente.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        // Add this method to MainForm.cs
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to exit the application?",
                    "Confirm Exit",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

                if (result != DialogResult.OK)
                {
                    e.Cancel = true;
                }
            }
        }
        private void btnRankings_Click(object sender, EventArgs e)
        {
            if (FavoritesManager.TryLoadFavorites(out var favoritesData))
            {
                if (!string.IsNullOrEmpty(favoritesData.FavoriteTeamCode))
                {
                    using var rankingsForm = new RankingsForm(_api, _genre, favoritesData.FavoriteTeamCode);
                    rankingsForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No favorite team selected. Please select favorites first.");
                }
            }
            else
            {
                MessageBox.Show("No favorites found. Please select favorites first.");
            }
        }
    }
}
