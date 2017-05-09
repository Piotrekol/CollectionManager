namespace GuiComponents.Controls
{
    partial class BeatmapListingView
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
            this.components = new System.ComponentModel.Container();
            this.label_resultsCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_beatmapSearch = new System.Windows.Forms.TextBox();
            this.BeatmapsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenDlMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenBeatmapPageMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenBeatmapDownloadMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.DownloadMapManagedMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.DownloadMapInBrowserMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SearchMapsetMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.SearchArtistMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.SearchTitleMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.ListViewBeatmaps = new BrightIdeasSoftware.FastDataListView();
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn10 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn6 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn7 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn8 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn9 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn11 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn12 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyUrlMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAsTextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenBeatmapFolderMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.BeatmapsContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListViewBeatmaps)).BeginInit();
            this.SuspendLayout();
            // 
            // label_resultsCount
            // 
            this.label_resultsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_resultsCount.AutoSize = true;
            this.label_resultsCount.Location = new System.Drawing.Point(524, 0);
            this.label_resultsCount.Name = "label_resultsCount";
            this.label_resultsCount.Size = new System.Drawing.Size(41, 13);
            this.label_resultsCount.TabIndex = 17;
            this.label_resultsCount.Text = "0 maps";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Search just like you do in osu!:";
            // 
            // textBox_beatmapSearch
            // 
            this.textBox_beatmapSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_beatmapSearch.Location = new System.Drawing.Point(3, 15);
            this.textBox_beatmapSearch.Name = "textBox_beatmapSearch";
            this.textBox_beatmapSearch.Size = new System.Drawing.Size(631, 20);
            this.textBox_beatmapSearch.TabIndex = 15;
            // 
            // BeatmapsContextMenuStrip
            // 
            this.BeatmapsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenDlMapMenuStrip,
            this.DeleteMapMenuStrip,
            this.searchToolStripMenuItem,
            this.copyToolStripMenuItem});
            this.BeatmapsContextMenuStrip.Name = "CollectionContextMenuStrip";
            this.BeatmapsContextMenuStrip.Size = new System.Drawing.Size(153, 114);
            // 
            // OpenDlMapMenuStrip
            // 
            this.OpenDlMapMenuStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenBeatmapPageMapMenuStrip,
            this.OpenBeatmapDownloadMapMenuStrip,
            this.OpenBeatmapFolderMenuStrip});
            this.OpenDlMapMenuStrip.Name = "OpenDlMapMenuStrip";
            this.OpenDlMapMenuStrip.Size = new System.Drawing.Size(152, 22);
            this.OpenDlMapMenuStrip.Text = "Open";
            // 
            // OpenBeatmapPageMapMenuStrip
            // 
            this.OpenBeatmapPageMapMenuStrip.Name = "OpenBeatmapPageMapMenuStrip";
            this.OpenBeatmapPageMapMenuStrip.Size = new System.Drawing.Size(191, 22);
            this.OpenBeatmapPageMapMenuStrip.Text = "Beatmap page(s)";
            this.OpenBeatmapPageMapMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // OpenBeatmapDownloadMapMenuStrip
            // 
            this.OpenBeatmapDownloadMapMenuStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DownloadMapManagedMenuStrip,
            this.DownloadMapInBrowserMenuStrip});
            this.OpenBeatmapDownloadMapMenuStrip.Name = "OpenBeatmapDownloadMapMenuStrip";
            this.OpenBeatmapDownloadMapMenuStrip.Size = new System.Drawing.Size(191, 22);
            this.OpenBeatmapDownloadMapMenuStrip.Text = "Download beatmap(s)";
            // 
            // DownloadMapManagedMenuStrip
            // 
            this.DownloadMapManagedMenuStrip.Name = "DownloadMapManagedMenuStrip";
            this.DownloadMapManagedMenuStrip.Size = new System.Drawing.Size(129, 22);
            this.DownloadMapManagedMenuStrip.Text = "Managed";
            this.DownloadMapManagedMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // DownloadMapInBrowserMenuStrip
            // 
            this.DownloadMapInBrowserMenuStrip.Name = "DownloadMapInBrowserMenuStrip";
            this.DownloadMapInBrowserMenuStrip.Size = new System.Drawing.Size(129, 22);
            this.DownloadMapInBrowserMenuStrip.Text = "In browser";
            this.DownloadMapInBrowserMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // DeleteMapMenuStrip
            // 
            this.DeleteMapMenuStrip.Enabled = false;
            this.DeleteMapMenuStrip.Name = "DeleteMapMenuStrip";
            this.DeleteMapMenuStrip.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.DeleteMapMenuStrip.Size = new System.Drawing.Size(152, 22);
            this.DeleteMapMenuStrip.Text = "Delete";
            this.DeleteMapMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SearchMapsetMapMenuStrip,
            this.SearchArtistMapMenuStrip,
            this.SearchTitleMapMenuStrip});
            this.searchToolStripMenuItem.Enabled = false;
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.searchToolStripMenuItem.Text = "Search";
            // 
            // SearchMapsetMapMenuStrip
            // 
            this.SearchMapsetMapMenuStrip.Name = "SearchMapsetMapMenuStrip";
            this.SearchMapsetMapMenuStrip.Size = new System.Drawing.Size(113, 22);
            this.SearchMapsetMapMenuStrip.Text = "mapset";
            // 
            // SearchArtistMapMenuStrip
            // 
            this.SearchArtistMapMenuStrip.Name = "SearchArtistMapMenuStrip";
            this.SearchArtistMapMenuStrip.Size = new System.Drawing.Size(113, 22);
            this.SearchArtistMapMenuStrip.Text = "artist";
            // 
            // SearchTitleMapMenuStrip
            // 
            this.SearchTitleMapMenuStrip.Name = "SearchTitleMapMenuStrip";
            this.SearchTitleMapMenuStrip.Size = new System.Drawing.Size(113, 22);
            this.SearchTitleMapMenuStrip.Text = "title";
            // 
            // ListViewBeatmaps
            // 
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn2);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn4);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn3);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn10);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn1);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn5);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn6);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn7);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn8);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn9);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn11);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn12);
            this.ListViewBeatmaps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListViewBeatmaps.AutoGenerateColumns = false;
            this.ListViewBeatmaps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn2,
            this.olvColumn4,
            this.olvColumn3,
            this.olvColumn10,
            this.olvColumn1,
            this.olvColumn5,
            this.olvColumn6,
            this.olvColumn7,
            this.olvColumn8,
            this.olvColumn9});
            this.ListViewBeatmaps.DataSource = null;
            this.ListViewBeatmaps.EmptyListMsg = "No collection selected";
            this.ListViewBeatmaps.HideSelection = false;
            this.ListViewBeatmaps.IsSimpleDragSource = true;
            this.ListViewBeatmaps.IsSimpleDropSink = true;
            this.ListViewBeatmaps.Location = new System.Drawing.Point(3, 41);
            this.ListViewBeatmaps.Name = "ListViewBeatmaps";
            this.ListViewBeatmaps.ShowGroups = false;
            this.ListViewBeatmaps.Size = new System.Drawing.Size(631, 461);
            this.ListViewBeatmaps.TabIndex = 14;
            this.ListViewBeatmaps.UnfocusedHighlightBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ListViewBeatmaps.UseCompatibleStateImageBehavior = false;
            this.ListViewBeatmaps.UseCustomSelectionColors = true;
            this.ListViewBeatmaps.View = System.Windows.Forms.View.Details;
            this.ListViewBeatmaps.VirtualMode = true;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Name";
            this.olvColumn2.AspectToStringFormat = "";
            this.olvColumn2.Text = "Name";
            this.olvColumn2.Width = 200;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "DiffName";
            this.olvColumn4.Text = "Difficulty";
            this.olvColumn4.TextCopyFormat = "[{0}]";
            this.olvColumn4.Width = 100;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "LocalBeatmapMissing";
            this.olvColumn3.CheckBoxes = true;
            this.olvColumn3.IsEditable = false;
            this.olvColumn3.MaximumWidth = 35;
            this.olvColumn3.MinimumWidth = 35;
            this.olvColumn3.Text = "N/A";
            this.olvColumn3.Width = 35;
            // 
            // olvColumn10
            // 
            this.olvColumn10.AspectName = "LocalVersionDiffers";
            this.olvColumn10.CheckBoxes = true;
            this.olvColumn10.IsEditable = false;
            this.olvColumn10.MaximumWidth = 35;
            this.olvColumn10.MinimumWidth = 35;
            this.olvColumn10.Text = "N/U";
            this.olvColumn10.Width = 35;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "StarsNomod";
            this.olvColumn1.Text = "★";
            this.olvColumn1.TextCopyFormat = "{0}★";
            this.olvColumn1.Width = 30;
            // 
            // olvColumn5
            // 
            this.olvColumn5.AspectName = "StateStr";
            this.olvColumn5.Text = "State";
            // 
            // olvColumn6
            // 
            this.olvColumn6.AspectName = "ApproachRate";
            this.olvColumn6.Text = "AR";
            this.olvColumn6.Width = 30;
            // 
            // olvColumn7
            // 
            this.olvColumn7.AspectName = "CircleSize";
            this.olvColumn7.Text = "CS";
            this.olvColumn7.Width = 30;
            // 
            // olvColumn8
            // 
            this.olvColumn8.AspectName = "HpDrainRate";
            this.olvColumn8.Text = "HP";
            this.olvColumn8.Width = 30;
            // 
            // olvColumn9
            // 
            this.olvColumn9.AspectName = "OverallDifficulty";
            this.olvColumn9.Text = "OD";
            this.olvColumn9.Width = 30;
            // 
            // olvColumn11
            // 
            this.olvColumn11.AspectName = "LastPlayed";
            this.olvColumn11.DisplayIndex = 10;
            this.olvColumn11.IsVisible = false;
            this.olvColumn11.Text = "Last played";
            // 
            // olvColumn12
            // 
            this.olvColumn12.AspectName = "EditDate";
            this.olvColumn12.IsVisible = false;
            this.olvColumn12.Text = "Add date";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyUrlMenuStrip,
            this.copyAsTextMenuStrip});
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // copyUrlMenuStrip
            // 
            this.copyUrlMenuStrip.Name = "copyUrlMenuStrip";
            this.copyUrlMenuStrip.Size = new System.Drawing.Size(152, 22);
            this.copyUrlMenuStrip.Text = "url(s)";
            // 
            // copyAsTextMenuStrip
            // 
            this.copyAsTextMenuStrip.Name = "copyAsTextMenuStrip";
            this.copyAsTextMenuStrip.Size = new System.Drawing.Size(152, 22);
            this.copyAsTextMenuStrip.Text = "As text";
            // 
            // OpenBeatmapFolderMenuStrip
            // 
            this.OpenBeatmapFolderMenuStrip.Name = "OpenBeatmapFolderMenuStrip";
            this.OpenBeatmapFolderMenuStrip.Size = new System.Drawing.Size(191, 22);
            this.OpenBeatmapFolderMenuStrip.Text = "Beatmap folder";
            this.OpenBeatmapFolderMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // BeatmapListingView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ListViewBeatmaps);
            this.Controls.Add(this.label_resultsCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_beatmapSearch);
            this.Name = "BeatmapListingView";
            this.Size = new System.Drawing.Size(634, 502);
            this.BeatmapsContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ListViewBeatmaps)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public BrightIdeasSoftware.FastDataListView ListViewBeatmaps;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn5;
        private BrightIdeasSoftware.OLVColumn olvColumn6;
        private BrightIdeasSoftware.OLVColumn olvColumn7;
        private BrightIdeasSoftware.OLVColumn olvColumn8;
        private BrightIdeasSoftware.OLVColumn olvColumn9;
        public System.Windows.Forms.Label label_resultsCount;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox textBox_beatmapSearch;
        private System.Windows.Forms.ContextMenuStrip BeatmapsContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem OpenDlMapMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem OpenBeatmapPageMapMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem OpenBeatmapDownloadMapMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem DownloadMapManagedMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem DownloadMapInBrowserMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem DeleteMapMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SearchMapsetMapMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem SearchArtistMapMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem SearchTitleMapMenuStrip;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private BrightIdeasSoftware.OLVColumn olvColumn10;
        private BrightIdeasSoftware.OLVColumn olvColumn11;
        private BrightIdeasSoftware.OLVColumn olvColumn12;
        private System.Windows.Forms.ToolStripMenuItem OpenBeatmapFolderMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyUrlMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyAsTextMenuStrip;
    }
}
