﻿
namespace WorldCupStats.WinFormsApp.Forms
{
    partial class MainForm
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
            btnSettings = new Button();
            btnFavorites = new Button();
            btnRankings = new Button();
            SuspendLayout();
            // 
            // btnSettings
            // 
            btnSettings.Location = new Point(0, 0);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(94, 29);
            btnSettings.TabIndex = 0;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // btnFavorites
            // 
            btnFavorites.Location = new Point(285, 190);
            btnFavorites.Name = "btnFavorites";
            btnFavorites.Size = new Size(200, 30);
            btnFavorites.TabIndex = 1;
            btnFavorites.Text = "Select Favorites";
            btnFavorites.UseVisualStyleBackColor = true;
            btnFavorites.Click += btnFavorites_Click;
            // 
            // btnRankings
            // 
            btnRankings.Location = new Point(285, 230);
            btnRankings.Name = "btnRankings";
            btnRankings.Size = new Size(200, 30);
            btnRankings.TabIndex = 2;
            btnRankings.Text = "Rankings";
            btnRankings.UseVisualStyleBackColor = true;
            btnRankings.Click += btnRankings_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(802, 413);
            Controls.Add(btnRankings);
            Controls.Add(btnFavorites);
            Controls.Add(btnSettings);
            Name = "MainForm";
            Text = "MainForm";
            Load += MainForm_LoadAsync;
            ResumeLayout(false);
        }

        #endregion

        private Button btnSettings;
        private Button btnFavorites;
        private Button btnRankings;
    }
}