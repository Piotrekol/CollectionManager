namespace GuiComponents.Forms
{
    partial class StartupForm
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
            startupView1 = new GuiComponents.Controls.StartupView();
            SuspendLayout();
            // 
            // startupView1
            // 
            startupView1.CollectionButtonsEnabled = true;
            startupView1.CollectionStatusText = "";
            startupView1.DatabaseButtonsEnabled = true;
            startupView1.Dock = System.Windows.Forms.DockStyle.Fill;
            startupView1.LoadDatabaseStatusText = "...";
            startupView1.LoadLazerDatabaseButtonEnabled = true;
            startupView1.LoadOsuCollectionButtonEnabled = true;
            startupView1.Location = new System.Drawing.Point(0, 0);
            startupView1.Name = "startupView1";
            startupView1.Size = new System.Drawing.Size(519, 461);
            startupView1.TabIndex = 0;
            startupView1.UseSelectedOptionsOnStartup = false;
            startupView1.UseSelectedOptionsOnStartupEnabled = true;
            // 
            // StartupForm
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(519, 461);
            Controls.Add(startupView1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(535, 500);
            MinimumSize = new System.Drawing.Size(535, 500);
            Name = "StartupForm";
            Text = "osu! Collection Manager";
            ResumeLayout(false);

        }

        #endregion

        private Controls.StartupView startupView1;
    }
}