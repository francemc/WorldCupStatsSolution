using WorldCupStats.Data.Models;

namespace WorldCupStats.WinFormsApp.Forms
{
    partial class RankingsForm
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
            tabControl = new TabControl();
            tabPlayers = new TabPage();
            dgvPlayers = new DataGridView();
            tabMatches = new TabPage();
            dgvMatches = new DataGridView();
            btnPrint = new Button();
            SuspendLayout(); 
           
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPlayers);
            tabControl.Controls.Add(tabMatches);
            tabControl.Dock = DockStyle.Top;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(800, 278);
            tabControl.TabIndex = 0;
            // 
            // tabPlayers
            // 
            tabPlayers.Controls.Add(dgvPlayers);
            tabPlayers.Location = new Point(4, 29);
            tabPlayers.Name = "tabPlayers";
            tabPlayers.Size = new Size(792, 245);
            tabPlayers.TabIndex = 0;
            tabPlayers.Text = "Player Rankings";
            // 
            // dgvPlayers
            // 
            dgvPlayers.ColumnHeadersHeight = 29;
            dgvPlayers.Dock = DockStyle.Fill;
            dgvPlayers.Location = new Point(0, 0);
            dgvPlayers.Name = "dgvPlayers";
            dgvPlayers.RowHeadersWidth = 51;
            dgvPlayers.Size = new Size(792, 245);
            dgvPlayers.TabIndex = 0;
            // 
            // tabMatches
            // 
            tabMatches.Controls.Add(dgvMatches);
            tabMatches.Location = new Point(4, 29);
            tabMatches.Name = "tabMatches";
            tabMatches.Size = new Size(792, 245);
            tabMatches.TabIndex = 1;
            tabMatches.Text = "Match Attendance";
            // 
            // dgvMatches
            // 
            dgvMatches.ColumnHeadersHeight = 29;
            dgvMatches.Dock = DockStyle.Fill;
            dgvMatches.Location = new Point(0, 0);
            dgvMatches.Name = "dgvMatches";
            dgvMatches.RowHeadersWidth = 51;
            dgvMatches.Size = new Size(792, 245);
            dgvMatches.TabIndex = 0;
            // 
            // btnPrint
            // 
            btnPrint.Dock = DockStyle.Bottom;
            btnPrint.Location = new Point(0, 0);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(75, 40);
            btnPrint.TabIndex = 0;
            btnPrint.Text = "Print Rankings";
            btnPrint.Click += btnPrint_Click;
            // 
            // RankingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnPrint);
            Controls.Add(tabControl);
            Name = "RankingsForm";
            Text = "RankingsForm";
            this.Load += RankingsForm_Load;
            ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl;
        private TabPage tabPlayers;
        private TabPage tabMatches;
        private DataGridView dgvPlayers;
        private DataGridView dgvMatches;
        private Button btnPrint;
    }
}