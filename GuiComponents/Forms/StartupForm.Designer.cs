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
            this.startupView1 = new GuiComponents.Controls.StartupView();
            this.SuspendLayout();
            // 
            // startupView1
            // 
            this.startupView1.CollectionButtonsEnabled = true;
            this.startupView1.CollectionStatusText = "";
            this.startupView1.DatabaseButtonsEnabled = true;
            this.startupView1.LoadDatabaseStatusText = "...";
            this.startupView1.LoadDefaultCollectionButtonEnabled = true;
            this.startupView1.Location = new System.Drawing.Point(0, 0);
            this.startupView1.Name = "startupView1";
            this.startupView1.Size = new System.Drawing.Size(519, 379);
            this.startupView1.TabIndex = 0;
            this.startupView1.UseSelectedOptionsOnStartup = false;
            // 
            // StartupForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(519, 379);
            this.Controls.Add(this.startupView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(535, 418);
            this.MinimumSize = new System.Drawing.Size(535, 418);
            this.Name = "StartupForm";
            this.Text = "osu! Collection Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.StartupView startupView1;
    }
}