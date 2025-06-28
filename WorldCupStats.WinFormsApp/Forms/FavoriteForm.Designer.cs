
namespace WorldCupStats.WinFormsApp.Forms
{
    partial class FavoriteForm
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
            lblStatus = new Label();
            cmbTeams = new ComboBox();
            btnConfirm = new Button();
            pnlFavoritePlayers = new FlowLayoutPanel();
            pnlOtherPlayers = new FlowLayoutPanel();
            lblFavorites = new Label();
            lblOthers = new Label();
            SuspendLayout();
            // 
            // lblStatus
            // 
            lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            lblStatus.ForeColor = Color.DarkSlateGray;
            lblStatus.Location = new Point(20, 20);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(960, 25);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Choose your favorite Team";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cmbTeams
            // 
            cmbTeams.Font = new Font("Segoe UI", 10F);
            cmbTeams.FormattingEnabled = true;
            cmbTeams.Location = new Point(20, 55);
            cmbTeams.Name = "cmbTeams";
            cmbTeams.Size = new Size(350, 31);
            cmbTeams.TabIndex = 1;
            // 
            // btnConfirm
            // 
            btnConfirm.BackColor = Color.MediumAquamarine;
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnConfirm.ForeColor = Color.White;
            btnConfirm.Location = new Point(630, 55);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(150, 31);
            btnConfirm.TabIndex = 2;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = false;
            btnConfirm.Click += BtnConfirm_Click;
            // 
            // pnlFavoritePlayers
            // 
            pnlFavoritePlayers.AutoScroll = true;
            pnlFavoritePlayers.BackColor = Color.LightYellow;
            pnlFavoritePlayers.BorderStyle = BorderStyle.FixedSingle;
            pnlFavoritePlayers.FlowDirection = FlowDirection.TopDown;
            pnlFavoritePlayers.Location = new Point(20, 130);
            pnlFavoritePlayers.Name = "pnlFavoritePlayers";
            pnlFavoritePlayers.Padding = new Padding(10);
            pnlFavoritePlayers.Size = new Size(450, 420);
            pnlFavoritePlayers.TabIndex = 0;
            // 
            // pnlOtherPlayers
            // 
            pnlOtherPlayers.AutoScroll = true;
            pnlOtherPlayers.BackColor = Color.WhiteSmoke;
            pnlOtherPlayers.BorderStyle = BorderStyle.FixedSingle;
            pnlOtherPlayers.FlowDirection = FlowDirection.TopDown;
            pnlOtherPlayers.Location = new Point(510, 130);
            pnlOtherPlayers.Name = "pnlOtherPlayers";
            pnlOtherPlayers.Padding = new Padding(10);
            pnlOtherPlayers.Size = new Size(450, 420);
            pnlOtherPlayers.TabIndex = 0;
            pnlOtherPlayers.WrapContents = false;
            // 
            // lblFavorites
            // 
            lblFavorites.AutoSize = true;
            lblFavorites.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFavorites.Location = new Point(20, 100);
            lblFavorites.Name = "lblFavorites";
            lblFavorites.Size = new Size(201, 23);
            lblFavorites.TabIndex = 3;
            lblFavorites.Text = "Favorite Players (3 max)";
            // 
            // lblOthers
            // 
            lblOthers.AutoSize = true;
            lblOthers.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblOthers.Location = new Point(510, 100);
            lblOthers.Name = "lblOthers";
            lblOthers.Size = new Size(117, 23);
            lblOthers.TabIndex = 4;
            lblOthers.Text = "Other Players";
            // 
            // FavoriteForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(980, 580);
            Controls.Add(lblOthers);
            Controls.Add(lblFavorites);
            Controls.Add(pnlOtherPlayers);
            Controls.Add(pnlFavoritePlayers);
            Controls.Add(btnConfirm);
            Controls.Add(cmbTeams);
            Controls.Add(lblStatus);
            Name = "FavoriteForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Favorite Players Selection";
            Load += FavoriteForm_LoadAsync;
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private Label lblStatus;
        private ComboBox cmbTeams;
        private Button btnConfirm;
        private FlowLayoutPanel pnlFavoritePlayers;
        private FlowLayoutPanel pnlOtherPlayers;
        private Label lblFavorites;
        private Label lblOthers;
    }
}