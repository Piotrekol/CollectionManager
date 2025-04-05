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
            downloadManagerView1 = new GuiComponents.Controls.DownloadManagerView();
            SuspendLayout();
            // 
            // downloadManagerView1
            // 
            downloadManagerView1.Dock = System.Windows.Forms.DockStyle.Fill;
            downloadManagerView1.DownloadButtonIsEnabled = true;
            downloadManagerView1.Location = new System.Drawing.Point(0, 0);
            downloadManagerView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            downloadManagerView1.Name = "downloadManagerView1";
            downloadManagerView1.Size = new System.Drawing.Size(584, 493);
            downloadManagerView1.TabIndex = 0;
            // 
            // DownloadManagerFormView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(584, 493);
            Controls.Add(downloadManagerView1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "DownloadManagerFormView";
            Text = "Collection Manager - Download list";
            ResumeLayout(false);

        }

        #endregion

        private Controls.DownloadManagerView downloadManagerView1;
    }
}