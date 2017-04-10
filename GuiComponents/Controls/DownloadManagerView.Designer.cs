namespace GuiComponents.Controls
{
    partial class DownloadManagerView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ListViewDownload = new BrightIdeasSoftware.FastObjectListView();
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.button_ToggleDownloads = new System.Windows.Forms.Button();
            this.label_status = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ListViewDownload)).BeginInit();
            this.SuspendLayout();
            // 
            // ListViewDownload
            // 
            this.ListViewDownload.AllColumns.Add(this.olvColumn3);
            this.ListViewDownload.AllColumns.Add(this.olvColumn1);
            this.ListViewDownload.AllColumns.Add(this.olvColumn2);
            this.ListViewDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListViewDownload.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn3,
            this.olvColumn1,
            this.olvColumn2});
            this.ListViewDownload.HideSelection = false;
            this.ListViewDownload.Location = new System.Drawing.Point(0, 32);
            this.ListViewDownload.Name = "ListViewDownload";
            this.ListViewDownload.ShowGroups = false;
            this.ListViewDownload.Size = new System.Drawing.Size(492, 328);
            this.ListViewDownload.TabIndex = 1;
            this.ListViewDownload.UnfocusedHighlightBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ListViewDownload.UseCompatibleStateImageBehavior = false;
            this.ListViewDownload.UseCustomSelectionColors = true;
            this.ListViewDownload.UseNotifyPropertyChanged = true;
            this.ListViewDownload.View = System.Windows.Forms.View.Details;
            this.ListViewDownload.VirtualMode = true;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "Id";
            this.olvColumn3.Text = "ID";
            this.olvColumn3.Width = 40;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Name";
            this.olvColumn1.Text = "Name";
            this.olvColumn1.Width = 332;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Progress";
            this.olvColumn2.Text = "Progress";
            this.olvColumn2.Width = 132;
            // 
            // button_ToggleDownloads
            // 
            this.button_ToggleDownloads.Location = new System.Drawing.Point(3, 3);
            this.button_ToggleDownloads.Name = "button_ToggleDownloads";
            this.button_ToggleDownloads.Size = new System.Drawing.Size(157, 23);
            this.button_ToggleDownloads.TabIndex = 3;
            this.button_ToggleDownloads.Text = "Stop downloads";
            this.button_ToggleDownloads.UseVisualStyleBackColor = true;
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.Location = new System.Drawing.Point(166, 8);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(34, 13);
            this.label_status.TabIndex = 4;
            this.label_status.Text = "         ";
            // 
            // DownloadManagerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.button_ToggleDownloads);
            this.Controls.Add(this.ListViewDownload);
            this.Name = "DownloadManagerView";
            this.Size = new System.Drawing.Size(492, 360);
            ((System.ComponentModel.ISupportInitialize)(this.ListViewDownload)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.FastObjectListView ListViewDownload;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private System.Windows.Forms.Button button_ToggleDownloads;
        private System.Windows.Forms.Label label_status;
    }
}
