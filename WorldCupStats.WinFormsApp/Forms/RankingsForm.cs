using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;

namespace WorldCupStats.WinFormsApp.Forms
{
    public partial class RankingsForm : Form
    {
        private readonly ApiService _api;
        private readonly Genre _genre;
        private readonly string _fifaCode;
        private List<Player> _playerStats = new();
        private List<Match> _matches = new();

        public RankingsForm(ApiService api, Genre genre, string fifaCode)
        {
            InitializeComponent();
            _api = api;
            _genre = genre;
            _fifaCode = fifaCode;
        }

        private async void RankingsForm_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
            BindPlayerData();
            BindMatchData();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                _matches = await _api.GetMatchesByCountryAsync(_genre, _fifaCode);
                CalculatePlayerStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculatePlayerStats()
        {
            var playerStatsDict = new Dictionary<string, Player>();

            foreach (var match in _matches)
            {
                var isHomeTeam = match.HomeTeam.Code.Equals(_fifaCode, StringComparison.OrdinalIgnoreCase);
                var teamPlayers = isHomeTeam ?
                    match.HomeTeamDetail.StartingEleven.Concat(match.HomeTeamDetail.Substitutes).ToList() :
                    match.AwayTeamDetail.StartingEleven.Concat(match.AwayTeamDetail.Substitutes).ToList();

                var teamEvents = isHomeTeam ? match.HomeTeamEvents : match.AwayTeamEvents;

                var appearedPlayers = new HashSet<string>();

                // Asignar datos base de jugador (nombre, dorsal, posición)
                foreach (var player in teamPlayers)
                {
                    if (!playerStatsDict.ContainsKey(player.Name))
                    {
                        playerStatsDict[player.Name] = new Player
                        {
                            Name = player.Name,
                            ShirtNumber = player.ShirtNumber,
                            Position = player.Position
                        };
                    }
                    appearedPlayers.Add(player.Name);
                }

                // Actualizar stats según eventos
                foreach (var ev in teamEvents)
                {
                    if (playerStatsDict.TryGetValue(ev.Player, out var stats))
                    {
                        switch (ev.TypeOfEvent)
                        {
                            case "goal":
                            case "goal-penalty":
                                stats.GoalsScored++;
                                break;
                            case "yellow-card":
                            case "yellow-card-second":
                                stats.YellowCards++;
                                break;
                        }
                        appearedPlayers.Add(ev.Player);
                    }
                    else
                    {
                        // Si jugador no está en lista base, crear mínimo con nombre y stats
                        playerStatsDict[ev.Player] = new Player
                        {
                            Name = ev.Player,
                            GoalsScored = ev.TypeOfEvent.Contains("goal") ? 1 : 0,
                            YellowCards = ev.TypeOfEvent.Contains("yellow-card") ? 1 : 0,
                            Apparences = 1
                        };
                        appearedPlayers.Add(ev.Player);
                    }
                }

                // Contar apariciones
                foreach (var playerName in appearedPlayers)
                {
                    playerStatsDict[playerName].Apparences++;
                }
            }

            _playerStats = playerStatsDict.Values
                .OrderByDescending(p => p.GoalsScored)
                .ThenBy(p => p.YellowCards)
                .ToList();
        }

        private void BindPlayerData()
        {
            dgvPlayers.AutoGenerateColumns = false;
            dgvPlayers.Columns.Clear();

            dgvPlayers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ShirtNumber",
                HeaderText = "Shirt Number",
                DataPropertyName = "ShirtNumber",
                Width = 80
            });
            dgvPlayers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                HeaderText = "Name",
                DataPropertyName = "Name",
                Width = 150
            });
            dgvPlayers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Position",
                HeaderText = "Position",
                DataPropertyName = "Position",
                Width = 100
            });
            dgvPlayers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "GoalsScored",
                HeaderText = "Goals",
                DataPropertyName = "GoalsScored",
                Width = 60
            });
            dgvPlayers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "YellowCards",
                HeaderText = "Yellow Cards",
                DataPropertyName = "YellowCards",
                Width = 90
            });
            dgvPlayers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Apparences",
                HeaderText = "Appearances",
                DataPropertyName = "Apparences",
                Width = 90
            });

            dgvPlayers.DataSource = _playerStats;
            dgvPlayers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void BindMatchData()
        {
            var matchData = _matches
                .Where(m => m.HomeTeam.Code.Equals(_fifaCode, StringComparison.OrdinalIgnoreCase) ||
                            m.AwayTeam.Code.Equals(_fifaCode, StringComparison.OrdinalIgnoreCase))
                .Select(m => new
                {
                    Location = m.Location,
                    Attendance = m.Attendance,
                    HomeTeam = m.HomeTeam.Country,
                    AwayTeam = m.AwayTeam.Country
                })
                .OrderByDescending(m => m.Attendance)
                .ToList();

            dgvMatches.DataSource = matchData;
            dgvMatches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            var printForm = new PrintSelectionForm(_playerStats, _matches);
            printForm.ShowDialog();
        }
    }
}
