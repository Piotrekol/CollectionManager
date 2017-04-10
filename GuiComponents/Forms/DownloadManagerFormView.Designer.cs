namespace GuiComponents.Forms
{
    partial class DownloadManagerFormView
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
            this.downloadManagerView1 = new GuiComponents.Controls.DownloadManagerView();
            this.SuspendLayout();
            // 
            // downloadManagerView1
            // 
            this.downloadManagerView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.downloadManagerView1.DownloadButtonIsEnabled = true;
            this.downloadManagerView1.Location = new System.Drawing.Point(0, 0);
            this.downloadManagerView1.Name = "downloadManagerView1";
            this.downloadManagerView1.Size = new System.Drawing.Size(514, 461);
            this.downloadManagerView1.TabIndex = 0;
            // 
            // DownloadManagerFormView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 461);
            this.Controls.Add(this.downloadManagerView1);
            this.Name = "DownloadManagerFormView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Collection Manager - Download list";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.DownloadManagerView downloadManagerView1;
    }
}