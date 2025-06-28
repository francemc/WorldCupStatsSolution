using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;

namespace WorldCupStats.WinFormsApp.Forms
{
    public partial class FavoriteForm : Form
    {
        private readonly ApiService _api;
        private readonly Genre _genre;

        private List<Player> _allPlayers = new();
        private List<Player> _favoritePlayers = new();
        private List<PlayerControl> selectedControls = new();

        public string ConfirmedFavoriteTeam { get; private set; }

        public FavoriteForm(ApiService apiService, Genre genre)
        {
            InitializeComponent();


            _api = apiService;
            _genre = genre;

            SetupEventHandlers();
            SetupDragDrop();
        }

        private void SetupEventHandlers()
        {
            cmbTeams.SelectedIndexChanged += async (_, __) => await LoadPlayersForSelectedTeam();
         
        }

        private void SetupDragDrop()
        {
            pnlFavoritePlayers.AllowDrop = true;
            pnlOtherPlayers.AllowDrop = true;

            pnlFavoritePlayers.DragEnter += Panel_DragEnter;
            pnlOtherPlayers.DragEnter += Panel_DragEnter;

            pnlFavoritePlayers.DragDrop += (s, e) => HandleDrop(pnlFavoritePlayers, e, true);
            pnlOtherPlayers.DragDrop += (s, e) => HandleDrop(pnlOtherPlayers, e, false);
        }

        private void Panel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PlayerControl)) || e.Data.GetDataPresent(typeof(List<PlayerControl>)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private async void FavoriteForm_LoadAsync(object sender, EventArgs e)
        {
           
            lblStatus.Visible = true;
            cmbTeams.Enabled = false;
            pnlFavoritePlayers.Enabled = false;
            pnlOtherPlayers.Enabled = false;
            btnConfirm.Enabled = false;

            try
            {
                var teams = await _api.GetTeamsAsync(_genre);
                cmbTeams.Items.Clear();
                foreach (var team in teams)
                    cmbTeams.Items.Add($"{team.Country} ({team.FifaCode})");

                cmbTeams.Enabled = true;

                string savedTeam = LoadFavoriteTeamFromFile();
                if (!string.IsNullOrEmpty(savedTeam))
                {
                    int idx = cmbTeams.Items.IndexOf(savedTeam);
                    if (idx >= 0)
                    {
                        cmbTeams.SelectedIndex = idx;
                        await LoadPlayersForSelectedTeam();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed loading teams: {ex.Message}");
                lblStatus.Text = "Failed to load teams.";
            }
        }

        private async Task LoadPlayersForSelectedTeam()
        {
            if (cmbTeams.SelectedItem == null) return;

            
            pnlFavoritePlayers.Enabled = false;
            pnlOtherPlayers.Enabled = false;
            btnConfirm.Enabled = false;

            try
            {
                string selectedTeam = cmbTeams.SelectedItem.ToString();
                string fifaCode = ExtractFifaCode(selectedTeam);

                _allPlayers = await _api.GetPlayersByTeamAsync(_genre, fifaCode);

                LoadFavoritePlayersFromFile();

                PopulatePlayerPanels();

                lblStatus.Text = "Choose your Favorite Players";
                pnlFavoritePlayers.Enabled = true;
                pnlOtherPlayers.Enabled = true;
                btnConfirm.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading players: {ex.Message}");
                lblStatus.Text = "Error loading players.";
            }
        }

        private void PopulatePlayerPanels()
        {
            pnlFavoritePlayers.Controls.Clear();
            pnlOtherPlayers.Controls.Clear();


            foreach (var player in _allPlayers)
            {
                bool isFavorite = _favoritePlayers.Any(fp => IsSamePlayer(fp, player));
                var playerControl = new PlayerControl(player, isFavorite);
                

                playerControl.FavoriteToggled += PlayerControl_FavoriteToggled;
                playerControl.SelectedChanged += PlayerControl_SelectedChanged;

                playerControl.MouseDown += (s, e) =>
                {
                    bool ctrlPressed = (ModifierKeys & Keys.Control) == Keys.Control;
                    if (e.Button == MouseButtons.Left)
                        HandlePlayerControlSelection(playerControl, ctrlPressed);
                };



                if (isFavorite)
                    pnlFavoritePlayers.Controls.Add(playerControl);
                else
                    pnlOtherPlayers.Controls.Add(playerControl);
            }
        }
        private void HandleDrop(Control targetPanel, DragEventArgs e, bool makeFavorite)
        {
            // Handle single control drop
            if (e.Data.GetData(typeof(PlayerControl)) is PlayerControl singleControl)
            {
                MovePlayerControl(singleControl, targetPanel, makeFavorite);
                singleControl.Deselect();
            }
            // Handle multiple controls drop
            else if (e.Data.GetData(typeof(List<PlayerControl>)) is List<PlayerControl> controls)
            {
                // Process in reverse order to maintain original order when adding to new panel
                foreach (var control in controls.OrderByDescending(c => c.Parent.Controls.IndexOf(c)))
                {
                    MovePlayerControl(control, targetPanel, makeFavorite);
                }
                ClearAllSelections();
            }
        }


        private void MovePlayerControl(PlayerControl control, Control targetPanel, bool makeFavorite)
        {
            var sourcePanel = control.Parent as Panel;
            if (sourcePanel != null && sourcePanel != targetPanel)
            {
                sourcePanel.Controls.Remove(control);
                targetPanel.Controls.Add(control);
            }
            control.SetFavorite(makeFavorite);

            if (makeFavorite)
            {
                if (!_favoritePlayers.Any(p => IsSamePlayer(p, control.Player)))
                    _favoritePlayers.Add(control.Player);
            }
            else
            {
                _favoritePlayers.RemoveAll(p => IsSamePlayer(p, control.Player));
            }
            ClearAllSelections();
        }

        private void HandlePlayerControlSelection(PlayerControl control, bool ctrlPressed)
        {
            if (ctrlPressed)
            {
                // Toggle selection for this control only
                control.ToggleSelection();
                if (control.IsSelected)
                {
                    if (!selectedControls.Contains(control))
                        selectedControls.Add(control);
                }
                else
                {
                    selectedControls.Remove(control);
                }
            }
            else
            {
                // If this control is already the only selected one, deselect it
                if (selectedControls.Count == 1 && selectedControls.Contains(control))
                {
                    control.Deselect();
                    selectedControls.Clear();
                    return;
                }

                // Clear all other selections and select this one
                ClearAllSelections();
                control.ToggleSelection();
                selectedControls.Add(control);
            }
        }


        private void PlayerControl_FavoriteToggled(object sender, EventArgs e)
        {
            if (sender is PlayerControl control)
            {
                if (control.IsFavorite && _favoritePlayers.Count >= 3)
                {
                    MessageBox.Show("You can only select up to 3 favorite players.");
                    control.ToggleFavorite(); // revert
                    return;
                }

                var currentPanel = control.Parent as Panel;
                var targetPanel = control.IsFavorite ? pnlFavoritePlayers : pnlOtherPlayers;

                if (currentPanel != null && currentPanel != targetPanel)
                {
                    currentPanel.Controls.Remove(control);
                    targetPanel.Controls.Add(control);
                }

                if (control.IsFavorite)
                {
                    if (!_favoritePlayers.Any(p => IsSamePlayer(p, control.Player)))
                        _favoritePlayers.Add(control.Player);
                }
                else
                {
                    _favoritePlayers.RemoveAll(p => IsSamePlayer(p, control.Player));
                }
            }
        }

        private void PlayerControl_SelectedChanged(object sender, EventArgs e)
        {
            if (sender is PlayerControl pc)
            {
                if (pc.IsSelected && !selectedControls.Contains(pc))
                    selectedControls.Add(pc);
                else if (!pc.IsSelected)
                    selectedControls.Remove(pc);
            }
        }

        private async void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (cmbTeams.SelectedItem == null)
            {
                MessageBox.Show("Please select a favorite team.");
                return;
            }
            if (_favoritePlayers.Count != 3)
            {
                MessageBox.Show("Please select exactly 3 favorite players.");
                return;
            }

            ConfirmedFavoriteTeam = cmbTeams.SelectedItem.ToString();

            SaveFavoriteTeamToFile(ConfirmedFavoriteTeam);
            SaveFavoritePlayersToFile();

            MessageBox.Show("Favorites saved!");
            this.Close();
        }

        #region File IO Helpers

        private string LoadFavoriteTeamFromFile()
        {
            const string path = "favorite_team.txt";
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        private void LoadFavoritePlayersFromFile()
        {
            _favoritePlayers.Clear();
            const string path = "favorite_players.txt";
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2 && int.TryParse(parts[1], out int shirtNumber))
                {
                    var player = _allPlayers.FirstOrDefault(p => p.Name == parts[0] && p.ShirtNumber == shirtNumber);
                    if (player != null) _favoritePlayers.Add(player);
                }
            }
        }

        private void SaveFavoriteTeamToFile(string teamString)
        {
            File.WriteAllText("favorite_team.txt", teamString);
        }

        private void SaveFavoritePlayersToFile()
        {
            var lines = _favoritePlayers.Select(p => $"{p.Name}|{p.ShirtNumber}");
            File.WriteAllLines("favorite_players.txt", lines);
        }

        #endregion

        private string ExtractFifaCode(string teamString)
        {
            int start = teamString.IndexOf('(');
            int end = teamString.IndexOf(')');
            if (start >= 0 && end > start)
                return teamString.Substring(start + 1, end - start - 1);
            return string.Empty;
        }

        private bool IsSamePlayer(Player a, Player b)
        {
            return a?.Name == b?.Name && a?.ShirtNumber == b?.ShirtNumber;
        }
        private void ClearAllSelections()
        {
            foreach (var control in selectedControls.ToList())
            {
                control.Deselect();
            }
            selectedControls.Clear();
        }


    }
}
