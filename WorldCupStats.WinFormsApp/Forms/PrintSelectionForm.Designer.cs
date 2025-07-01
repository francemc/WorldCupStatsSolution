namespace WorldCupStats.WinFormsApp.Forms
{
    partial class PrintSelectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var btnPrintPlayers = new Button();
            var btnPrintMatches = new Button();
            btnPrintPlayers.Text = "Print Player Rankings";
            btnPrintPlayers.Dock = DockStyle.Top;
            btnPrintMatches.Text = "Print Match Rankings";
            btnPrintMatches.Dock = DockStyle.Top; 
            btnPrintPlayers.Click += (s, e) => PrintPlayerRankings();
            btnPrintMatches.Click += (s, e) => PrintMatchRankings();

            Controls.Add(btnPrintMatches);
            Controls.Add(btnPrintPlayers);
            Size = new Size(300, 150);
            Text = "Select Print Option";
        }

        #endregion
    }
}