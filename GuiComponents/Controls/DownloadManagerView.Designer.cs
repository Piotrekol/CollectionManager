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
            ListViewDownload = new BrightIdeasSoftware.FastObjectListView();
            olvColumn3 = new BrightIdeasSoftware.OLVColumn();
            olvColumn1 = new BrightIdeasSoftware.OLVColumn();
            olvColumn2 = new BrightIdeasSoftware.OLVColumn();
            button_ToggleDownloads = new System.Windows.Forms.Button();
            label_status = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)ListViewDownload).BeginInit();
            SuspendLayout();
            // 
            // ListViewDownload
            // 
            ListViewDownload.AllColumns.Add(olvColumn3);
            ListViewDownload.AllColumns.Add(olvColumn1);
            ListViewDownload.AllColumns.Add(olvColumn2);
            ListViewDownload.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ListViewDownload.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { olvColumn3, olvColumn1, olvColumn2 });
            ListViewDownload.Location = new System.Drawing.Point(0, 37);
            ListViewDownload.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ListViewDownload.Name = "ListViewDownload";
            ListViewDownload.ShowGroups = false;
            ListViewDownload.Size = new System.Drawing.Size(573, 378);
            ListViewDownload.TabIndex = 1;
            ListViewDownload.UnfocusedHighlightBackgroundColor = System.Drawing.Color.FromArgb(192, 255, 192);
            ListViewDownload.UseCompatibleStateImageBehavior = false;
            ListViewDownload.UseCustomSelectionColors = true;
            ListViewDownload.UseNotifyPropertyChanged = true;
            ListViewDownload.View = System.Windows.Forms.View.Details;
            ListViewDownload.VirtualMode = true;
            // 
            // olvColumn3
            // 
            olvColumn3.AspectName = "Id";
            olvColumn3.Text = "ID";
            olvColumn3.Width = 40;
            // 
            // olvColumn1
            // 
            olvColumn1.AspectName = "Name";
            olvColumn1.Text = "Name";
            olvColumn1.Width = 332;
            // 
            // olvColumn2
            // 
            olvColumn2.AspectName = "Progress";
            olvColumn2.Text = "Progress";
            olvColumn2.Width = 132;
            // 
            // button_ToggleDownloads
            // 
            button_ToggleDownloads.Location = new System.Drawing.Point(4, 3);
            button_ToggleDownloads.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button_ToggleDownloads.Name = "button_ToggleDownloads";
            button_ToggleDownloads.Size = new System.Drawing.Size(183, 27);
            button_ToggleDownloads.TabIndex = 3;
            button_ToggleDownloads.Text = "Stop downloads";
            button_ToggleDownloads.UseVisualStyleBackColor = true;
            // 
            // label_status
            // 
            label_status.AutoSize = true;
            label_status.Location = new System.Drawing.Point(194, 9);
            label_status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_status.Name = "label_status";
            label_status.Size = new System.Drawing.Size(34, 15);
            label_status.TabIndex = 4;
            label_status.Text = "         ";
            // 
            // DownloadManagerView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label_status);
            Controls.Add(button_ToggleDownloads);
            Controls.Add(ListViewDownload);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "DownloadManagerView";
            Size = new System.Drawing.Size(574, 415);
            ((System.ComponentModel.ISupportInitialize)ListViewDownload).EndInit();
            ResumeLayout(false);
            PerformLayout();

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
