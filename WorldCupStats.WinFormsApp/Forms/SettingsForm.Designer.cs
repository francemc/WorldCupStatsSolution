namespace WorldCupStats.WinFormsApp.Forms
{
    partial class SettingsForm
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
            btnOk = new Button();
            btnCancel = new Button();
            cmbLanguage = new ComboBox();
            cmbChampionship = new ComboBox();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.Location = new Point(150, 194);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(94, 29);
            btnOk.TabIndex = 0;
            btnOk.Text = "Ok";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click_1;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(272, 194);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click_1;
            // 
            // cmbLanguage
            // 
            cmbLanguage.Location = new Point(190, 109);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(121, 28);
            cmbLanguage.TabIndex = 1;
            cmbLanguage.Text = "Language";
            // 
            // cmbChampionship
            // 
            cmbChampionship.Location = new Point(190, 143);
            cmbChampionship.Name = "cmbChampionship";
            cmbChampionship.Size = new Size(121, 28);
            cmbChampionship.TabIndex = 0;
            cmbChampionship.Text = "Championship";
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(517, 318);
            Controls.Add(cmbChampionship);
            Controls.Add(cmbLanguage);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            Name = "SettingsForm";
            Text = "SettingsForm";
            ResumeLayout(false);
        }

        #endregion

        private Button btnOk;
        private Button btnCancel;
        private ComboBox cmbLanguage;
        private ComboBox cmbChampionship;
    }
}