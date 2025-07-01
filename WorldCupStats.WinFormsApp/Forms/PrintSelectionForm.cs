using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using WorldCupStats.Data.Models;

namespace WorldCupStats.WinFormsApp.Forms
{
    public partial class PrintSelectionForm : Form
    {
        private readonly List<Player> _playerStats;
        private readonly List<Match> _matches;

        private int _currentPrintIndex;

        public PrintSelectionForm(List<Player> playerStats, List<Match> matches)
        {
            _playerStats = playerStats;
            _matches = matches;
            InitializeComponent();
        }

        private void PrintPlayerRankings()
        {
            _currentPrintIndex = 0;
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += PrintPlayers_PrintPage;

            using (PrintPreviewDialog preview = new PrintPreviewDialog())
            {
                preview.Document = printDoc;
                preview.ShowDialog();
            }
        }

        private void PrintPlayers_PrintPage(object sender, PrintPageEventArgs e)
        {
            int startX = 50, startY = 50;
            int lineHeight = 30;
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font font = new Font("Arial", 12);

            e.Graphics.DrawString("Player Rankings", titleFont, Brushes.Black, startX, startY);
            startY += 40;

            // Encabezados (sin columna de imagen)
            e.Graphics.DrawString("Name", font, Brushes.Black, startX, startY);
            e.Graphics.DrawString("Goals", font, Brushes.Black, startX + 250, startY);
            e.Graphics.DrawString("Yellow Cards", font, Brushes.Black, startX + 350, startY);
            e.Graphics.DrawString("Appearances", font, Brushes.Black, startX + 470, startY);
            startY += lineHeight;

            while (_currentPrintIndex < _playerStats.Count)
            {
                var p = _playerStats[_currentPrintIndex];

                // Dibuja solo texto
                e.Graphics.DrawString(p.Name, font, Brushes.Black, startX, startY);
                e.Graphics.DrawString(p.GoalsScored.ToString(), font, Brushes.Black, startX + 250, startY);
                e.Graphics.DrawString(p.YellowCards.ToString(), font, Brushes.Black, startX + 350, startY);
                e.Graphics.DrawString(p.Apparences.ToString(), font, Brushes.Black, startX + 470, startY);

                startY += lineHeight;
                _currentPrintIndex++;

                if (startY > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            e.HasMorePages = false;
        }

        private void PrintMatchRankings()
        {
            _currentPrintIndex = 0;
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += PrintMatches_PrintPage;

            using (PrintPreviewDialog preview = new PrintPreviewDialog())
            {
                preview.Document = printDoc;
                preview.ShowDialog();
            }
        }

        private void PrintMatches_PrintPage(object sender, PrintPageEventArgs e)
        {
            int startX = 50, startY = 50;
            int lineHeight = 25;
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font font = new Font("Arial", 12);

            e.Graphics.DrawString("Match Attendance Rankings", titleFont, Brushes.Black, startX, startY);
            startY += 40;

            e.Graphics.DrawString("Location", font, Brushes.Black, startX, startY);
            e.Graphics.DrawString("Attendance", font, Brushes.Black, startX + 200, startY);
            e.Graphics.DrawString("Home Team", font, Brushes.Black, startX + 350, startY);
            e.Graphics.DrawString("Away Team", font, Brushes.Black, startX + 500, startY);
            startY += lineHeight;

            while (_currentPrintIndex < _matches.Count)
            {
                var m = _matches[_currentPrintIndex];
                e.Graphics.DrawString(m.Location, font, Brushes.Black, startX, startY);
                e.Graphics.DrawString(m.Attendance.ToString(), font, Brushes.Black, startX + 200, startY);
                e.Graphics.DrawString(m.HomeTeamCountry, font, Brushes.Black, startX + 350, startY);
                e.Graphics.DrawString(m.AwayTeamCountry, font, Brushes.Black, startX + 500, startY);

                startY += lineHeight;
                _currentPrintIndex++;

                if (startY > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            e.HasMorePages = false;
        }
    }
}
