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
            this.OpenBeatmapFolderMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SearchMapsetMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.SearchArtistMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.SearchTitleMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyUrlMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAsTextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.PullMapsetMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.ListViewBeatmaps = new BrightIdeasSoftware.FastDataListView();
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.column_stars = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.column_ar = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.column_cs = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.column_hp = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.column_od = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn10 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.OsuGrade = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.LastPlayed = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn12 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn13 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.TaikoGrade = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.CatchGrade = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ManiaGrade = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn9 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn11 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.MainBpm = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
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
            this.copyToolStripMenuItem,
            this.PullMapsetMenuStrip});
            this.BeatmapsContextMenuStrip.Name = "CollectionContextMenuStrip";
            this.BeatmapsContextMenuStrip.Size = new System.Drawing.Size(172, 114);
            // 
            // OpenDlMapMenuStrip
            // 
            this.OpenDlMapMenuStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenBeatmapPageMapMenuStrip,
            this.OpenBeatmapDownloadMapMenuStrip,
            this.OpenBeatmapFolderMenuStrip});
            this.OpenDlMapMenuStrip.Name = "OpenDlMapMenuStrip";
            this.OpenDlMapMenuStrip.Size = new System.Drawing.Size(171, 22);
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
            // OpenBeatmapFolderMenuStrip
            // 
            this.OpenBeatmapFolderMenuStrip.Name = "OpenBeatmapFolderMenuStrip";
            this.OpenBeatmapFolderMenuStrip.Size = new System.Drawing.Size(191, 22);
            this.OpenBeatmapFolderMenuStrip.Text = "Beatmap folder";
            this.OpenBeatmapFolderMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // DeleteMapMenuStrip
            // 
            this.DeleteMapMenuStrip.Enabled = false;
            this.DeleteMapMenuStrip.Name = "DeleteMapMenuStrip";
            this.DeleteMapMenuStrip.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.DeleteMapMenuStrip.Size = new System.Drawing.Size(171, 22);
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
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
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
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyUrlMenuStrip,
            this.copyAsTextMenuStrip});
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // copyUrlMenuStrip
            // 
            this.copyUrlMenuStrip.Name = "copyUrlMenuStrip";
            this.copyUrlMenuStrip.Size = new System.Drawing.Size(180, 22);
            this.copyUrlMenuStrip.Text = "url(s)";
            this.copyUrlMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // copyAsTextMenuStrip
            // 
            this.copyAsTextMenuStrip.Name = "copyAsTextMenuStrip";
            this.copyAsTextMenuStrip.Size = new System.Drawing.Size(180, 22);
            this.copyAsTextMenuStrip.Text = "As text";
            this.copyAsTextMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // PullMapsetMenuStrip
            // 
            this.PullMapsetMenuStrip.Name = "PullMapsetMenuStrip";
            this.PullMapsetMenuStrip.Size = new System.Drawing.Size(171, 22);
            this.PullMapsetMenuStrip.Text = "Pull whole mapset";
            this.PullMapsetMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // ListViewBeatmaps
            // 
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn2);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn4);
            this.ListViewBeatmaps.AllColumns.Add(this.column_stars);
            this.ListViewBeatmaps.AllColumns.Add(this.column_ar);
            this.ListViewBeatmaps.AllColumns.Add(this.column_cs);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn5);
            this.ListViewBeatmaps.AllColumns.Add(this.column_hp);
            this.ListViewBeatmaps.AllColumns.Add(this.column_od);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn3);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn10);
            this.ListViewBeatmaps.AllColumns.Add(this.OsuGrade);
            this.ListViewBeatmaps.AllColumns.Add(this.LastPlayed);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn12);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn13);
            this.ListViewBeatmaps.AllColumns.Add(this.TaikoGrade);
            this.ListViewBeatmaps.AllColumns.Add(this.CatchGrade);
            this.ListViewBeatmaps.AllColumns.Add(this.ManiaGrade);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn9);
            this.ListViewBeatmaps.AllColumns.Add(this.olvColumn11);
            this.ListViewBeatmaps.AllColumns.Add(this.MainBpm);
            this.ListViewBeatmaps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListViewBeatmaps.AutoGenerateColumns = false;
            this.ListViewBeatmaps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn2,
            this.olvColumn4,
            this.column_stars,
            this.column_ar,
            this.column_cs,
            this.olvColumn5,
            this.olvColumn3,
            this.olvColumn10,
            this.OsuGrade,
            this.LastPlayed,
            this.olvColumn13});
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
            this.ListViewBeatmaps.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ListViewBeatmaps_KeyUp);
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Name";
            this.olvColumn2.AspectToStringFormat = "";
            this.olvColumn2.IsEditable = false;
            this.olvColumn2.Text = "Name";
            this.olvColumn2.Width = 200;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "DiffName";
            this.olvColumn4.IsEditable = false;
            this.olvColumn4.Text = "Difficulty";
            this.olvColumn4.TextCopyFormat = "[{0}]";
            this.olvColumn4.Width = 100;
            // 
            // column_stars
            // 
            this.column_stars.AspectName = "StarsNomod";
            this.column_stars.IsEditable = false;
            this.column_stars.Text = "★";
            this.column_stars.TextCopyFormat = "{0}★";
            this.column_stars.Width = 30;
            // 
            // column_ar
            // 
            this.column_ar.AspectName = "ApproachRate";
            this.column_ar.IsEditable = false;
            this.column_ar.Text = "AR";
            this.column_ar.Width = 30;
            // 
            // column_cs
            // 
            this.column_cs.AspectName = "CircleSize";
            this.column_cs.IsEditable = false;
            this.column_cs.Text = "CS";
            this.column_cs.Width = 30;
            // 
            // olvColumn5
            // 
            this.olvColumn5.AspectName = "StateStr";
            this.olvColumn5.IsEditable = false;
            this.olvColumn5.Text = "State";
            // 
            // column_hp
            // 
            this.column_hp.AspectName = "HpDrainRate";
            this.column_hp.DisplayIndex = 8;
            this.column_hp.IsEditable = false;
            this.column_hp.IsVisible = false;
            this.column_hp.Text = "HP";
            this.column_hp.Width = 30;
            // 
            // column_od
            // 
            this.column_od.AspectName = "OverallDifficulty";
            this.column_od.DisplayIndex = 9;
            this.column_od.IsEditable = false;
            this.column_od.IsVisible = false;
            this.column_od.Text = "OD";
            this.column_od.Width = 30;
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
            // OsuGrade
            // 
            this.OsuGrade.AspectName = "OsuGrade";
            this.OsuGrade.IsEditable = false;
            this.OsuGrade.Text = "OsuGrade";
            // 
            // LastPlayed
            // 
            this.LastPlayed.AspectName = "LastPlayed";
            this.LastPlayed.IsEditable = false;
            this.LastPlayed.Text = "Last played";
            // 
            // olvColumn12
            // 
            this.olvColumn12.AspectName = "EditDate";
            this.olvColumn12.IsEditable = false;
            this.olvColumn12.IsVisible = false;
            this.olvColumn12.Text = "Add date";
            // 
            // olvColumn13
            // 
            this.olvColumn13.AspectName = "UserComment";
            this.olvColumn13.Text = "Comment";
            // 
            // TaikoGrade
            // 
            this.TaikoGrade.AspectName = "TaikoGrade";
            this.TaikoGrade.IsEditable = false;
            this.TaikoGrade.IsVisible = false;
            this.TaikoGrade.Text = "TaikoGrade";
            // 
            // CatchGrade
            // 
            this.CatchGrade.AspectName = "CatchGrade";
            this.CatchGrade.IsEditable = false;
            this.CatchGrade.IsVisible = false;
            this.CatchGrade.Text = "CatchGrade";
            // 
            // ManiaGrade
            // 
            this.ManiaGrade.AspectName = "ManiaGrade";
            this.ManiaGrade.IsEditable = false;
            this.ManiaGrade.IsVisible = false;
            this.ManiaGrade.Text = "ManiaGrade";
            // 
            // olvColumn9
            // 
            this.olvColumn9.AspectName = "MapSetId";
            this.olvColumn9.IsEditable = false;
            this.olvColumn9.IsVisible = false;
            this.olvColumn9.Text = "SetId";
            // 
            // olvColumn11
            // 
            this.olvColumn11.AspectName = "MapId";
            this.olvColumn11.IsEditable = false;
            this.olvColumn11.IsVisible = false;
            this.olvColumn11.Text = "MapId";
            // 
            // MainBpm
            // 
            this.MainBpm.AspectName = "MainBpm";
            this.MainBpm.DisplayIndex = 11;
            this.MainBpm.IsEditable = false;
            this.MainBpm.IsVisible = false;
            this.MainBpm.Text = "MainBpm";
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
        private BrightIdeasSoftware.OLVColumn column_stars;
        private BrightIdeasSoftware.OLVColumn olvColumn5;
        private BrightIdeasSoftware.OLVColumn column_ar;
        private BrightIdeasSoftware.OLVColumn column_cs;
        private BrightIdeasSoftware.OLVColumn column_hp;
        private BrightIdeasSoftware.OLVColumn column_od;
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
        private BrightIdeasSoftware.OLVColumn LastPlayed;
        private BrightIdeasSoftware.OLVColumn olvColumn12;
        private System.Windows.Forms.ToolStripMenuItem OpenBeatmapFolderMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyUrlMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyAsTextMenuStrip;
        private BrightIdeasSoftware.OLVColumn olvColumn13;
        private BrightIdeasSoftware.OLVColumn OsuGrade;
        private BrightIdeasSoftware.OLVColumn TaikoGrade;
        private BrightIdeasSoftware.OLVColumn CatchGrade;
        private BrightIdeasSoftware.OLVColumn ManiaGrade;
        private System.Windows.Forms.ToolStripMenuItem PullMapsetMenuStrip;
        private BrightIdeasSoftware.OLVColumn olvColumn9;
        private BrightIdeasSoftware.OLVColumn olvColumn11;
        private BrightIdeasSoftware.OLVColumn MainBpm;
    }
}
