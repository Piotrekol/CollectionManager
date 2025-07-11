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
            components = new System.ComponentModel.Container();
            label_resultsCount = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            textBox_beatmapSearch = new System.Windows.Forms.TextBox();
            BeatmapsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            OpenDlMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            OpenBeatmapPageMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            OpenBeatmapDownloadMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            DownloadMapManagedMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            DownloadMapInBrowserMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            OpenBeatmapFolderMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            DeleteMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            SearchMapsetMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            SearchArtistMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            SearchTitleMapMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyUrlMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            copyAsTextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            PullMapsetMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            exportBeatmapSetsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ListViewBeatmaps = new BrightIdeasSoftware.FastDataListView();
            olvColumn2 = new BrightIdeasSoftware.OLVColumn();
            olvColumn4 = new BrightIdeasSoftware.OLVColumn();
            column_stars = new BrightIdeasSoftware.OLVColumn();
            column_ar = new BrightIdeasSoftware.OLVColumn();
            column_cs = new BrightIdeasSoftware.OLVColumn();
            column_hp = new BrightIdeasSoftware.OLVColumn();
            column_od = new BrightIdeasSoftware.OLVColumn();
            column_bpm = new BrightIdeasSoftware.OLVColumn();
            column_state = new BrightIdeasSoftware.OLVColumn();
            olvColumn10 = new BrightIdeasSoftware.OLVColumn();
            OsuGrade = new BrightIdeasSoftware.OLVColumn();
            LastPlayed = new BrightIdeasSoftware.OLVColumn();
            EditDate = new BrightIdeasSoftware.OLVColumn();
            olvColumn13 = new BrightIdeasSoftware.OLVColumn();
            TaikoGrade = new BrightIdeasSoftware.OLVColumn();
            CatchGrade = new BrightIdeasSoftware.OLVColumn();
            ManiaGrade = new BrightIdeasSoftware.OLVColumn();
            olvColumn9 = new BrightIdeasSoftware.OLVColumn();
            olvColumn11 = new BrightIdeasSoftware.OLVColumn();
            ScoresCount = new BrightIdeasSoftware.OLVColumn();
            LastScoreDate = new BrightIdeasSoftware.OLVColumn();
            button_searchHelp = new System.Windows.Forms.Button();
            BeatmapsContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ListViewBeatmaps).BeginInit();
            SuspendLayout();
            // 
            // label_resultsCount
            // 
            label_resultsCount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label_resultsCount.Location = new System.Drawing.Point(885, -1);
            label_resultsCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_resultsCount.Name = "label_resultsCount";
            label_resultsCount.Size = new System.Drawing.Size(186, 15);
            label_resultsCount.TabIndex = 17;
            label_resultsCount.Text = "0 maps";
            label_resultsCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(4, -1);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(166, 15);
            label1.TabIndex = 16;
            label1.Text = "Search just like you do in osu!:";
            // 
            // textBox_beatmapSearch
            // 
            textBox_beatmapSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox_beatmapSearch.Location = new System.Drawing.Point(4, 17);
            textBox_beatmapSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox_beatmapSearch.Name = "textBox_beatmapSearch";
            textBox_beatmapSearch.Size = new System.Drawing.Size(1067, 23);
            textBox_beatmapSearch.TabIndex = 15;
            // 
            // BeatmapsContextMenuStrip
            // 
            BeatmapsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { OpenDlMapMenuStrip, DeleteMapMenuStrip, searchToolStripMenuItem, copyToolStripMenuItem, PullMapsetMenuStrip, exportBeatmapSetsMenuItem });
            BeatmapsContextMenuStrip.Name = "CollectionContextMenuStrip";
            BeatmapsContextMenuStrip.Size = new System.Drawing.Size(181, 136);
            // 
            // OpenDlMapMenuStrip
            // 
            OpenDlMapMenuStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { OpenBeatmapPageMapMenuStrip, OpenBeatmapDownloadMapMenuStrip, OpenBeatmapFolderMenuStrip });
            OpenDlMapMenuStrip.Name = "OpenDlMapMenuStrip";
            OpenDlMapMenuStrip.Size = new System.Drawing.Size(180, 22);
            OpenDlMapMenuStrip.Text = "Open";
            // 
            // OpenBeatmapPageMapMenuStrip
            // 
            OpenBeatmapPageMapMenuStrip.Name = "OpenBeatmapPageMapMenuStrip";
            OpenBeatmapPageMapMenuStrip.Size = new System.Drawing.Size(191, 22);
            OpenBeatmapPageMapMenuStrip.Text = "Beatmap page(s)";
            OpenBeatmapPageMapMenuStrip.Click += MenuStripClick;
            // 
            // OpenBeatmapDownloadMapMenuStrip
            // 
            OpenBeatmapDownloadMapMenuStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { DownloadMapManagedMenuStrip, DownloadMapInBrowserMenuStrip });
            OpenBeatmapDownloadMapMenuStrip.Name = "OpenBeatmapDownloadMapMenuStrip";
            OpenBeatmapDownloadMapMenuStrip.Size = new System.Drawing.Size(191, 22);
            OpenBeatmapDownloadMapMenuStrip.Text = "Download beatmap(s)";
            // 
            // DownloadMapManagedMenuStrip
            // 
            DownloadMapManagedMenuStrip.Name = "DownloadMapManagedMenuStrip";
            DownloadMapManagedMenuStrip.Size = new System.Drawing.Size(129, 22);
            DownloadMapManagedMenuStrip.Text = "Managed";
            DownloadMapManagedMenuStrip.Click += MenuStripClick;
            // 
            // DownloadMapInBrowserMenuStrip
            // 
            DownloadMapInBrowserMenuStrip.Name = "DownloadMapInBrowserMenuStrip";
            DownloadMapInBrowserMenuStrip.Size = new System.Drawing.Size(129, 22);
            DownloadMapInBrowserMenuStrip.Text = "In browser";
            DownloadMapInBrowserMenuStrip.Click += MenuStripClick;
            // 
            // OpenBeatmapFolderMenuStrip
            // 
            OpenBeatmapFolderMenuStrip.Name = "OpenBeatmapFolderMenuStrip";
            OpenBeatmapFolderMenuStrip.Size = new System.Drawing.Size(191, 22);
            OpenBeatmapFolderMenuStrip.Text = "Beatmap folder";
            OpenBeatmapFolderMenuStrip.Click += MenuStripClick;
            // 
            // DeleteMapMenuStrip
            // 
            DeleteMapMenuStrip.Enabled = false;
            DeleteMapMenuStrip.Name = "DeleteMapMenuStrip";
            DeleteMapMenuStrip.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            DeleteMapMenuStrip.Size = new System.Drawing.Size(180, 22);
            DeleteMapMenuStrip.Text = "Delete";
            DeleteMapMenuStrip.Click += MenuStripClick;
            // 
            // searchToolStripMenuItem
            // 
            searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SearchMapsetMapMenuStrip, SearchArtistMapMenuStrip, SearchTitleMapMenuStrip });
            searchToolStripMenuItem.Enabled = false;
            searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            searchToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            searchToolStripMenuItem.Text = "Search";
            // 
            // SearchMapsetMapMenuStrip
            // 
            SearchMapsetMapMenuStrip.Name = "SearchMapsetMapMenuStrip";
            SearchMapsetMapMenuStrip.Size = new System.Drawing.Size(113, 22);
            SearchMapsetMapMenuStrip.Text = "mapset";
            // 
            // SearchArtistMapMenuStrip
            // 
            SearchArtistMapMenuStrip.Name = "SearchArtistMapMenuStrip";
            SearchArtistMapMenuStrip.Size = new System.Drawing.Size(113, 22);
            SearchArtistMapMenuStrip.Text = "artist";
            // 
            // SearchTitleMapMenuStrip
            // 
            SearchTitleMapMenuStrip.Name = "SearchTitleMapMenuStrip";
            SearchTitleMapMenuStrip.Size = new System.Drawing.Size(113, 22);
            SearchTitleMapMenuStrip.Text = "title";
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { copyUrlMenuStrip, copyAsTextMenuStrip });
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            copyToolStripMenuItem.Text = "Copy";
            // 
            // copyUrlMenuStrip
            // 
            copyUrlMenuStrip.Name = "copyUrlMenuStrip";
            copyUrlMenuStrip.Size = new System.Drawing.Size(109, 22);
            copyUrlMenuStrip.Text = "url(s)";
            copyUrlMenuStrip.Click += MenuStripClick;
            // 
            // copyAsTextMenuStrip
            // 
            copyAsTextMenuStrip.Name = "copyAsTextMenuStrip";
            copyAsTextMenuStrip.Size = new System.Drawing.Size(109, 22);
            copyAsTextMenuStrip.Text = "As text";
            copyAsTextMenuStrip.Click += MenuStripClick;
            // 
            // PullMapsetMenuStrip
            // 
            PullMapsetMenuStrip.Name = "PullMapsetMenuStrip";
            PullMapsetMenuStrip.Size = new System.Drawing.Size(180, 22);
            PullMapsetMenuStrip.Text = "Pull whole mapset";
            PullMapsetMenuStrip.Click += MenuStripClick;
            // 
            // exportBeatmapSetsMenuItem
            // 
            exportBeatmapSetsMenuItem.Name = "exportBeatmapSetsMenuItem";
            exportBeatmapSetsMenuItem.Size = new System.Drawing.Size(180, 22);
            exportBeatmapSetsMenuItem.Text = "Export beatmap sets";
            exportBeatmapSetsMenuItem.Click += MenuStripClick;
            // 
            // ListViewBeatmaps
            // 
            ListViewBeatmaps.AllColumns.Add(olvColumn2);
            ListViewBeatmaps.AllColumns.Add(olvColumn4);
            ListViewBeatmaps.AllColumns.Add(column_stars);
            ListViewBeatmaps.AllColumns.Add(column_ar);
            ListViewBeatmaps.AllColumns.Add(column_cs);
            ListViewBeatmaps.AllColumns.Add(column_hp);
            ListViewBeatmaps.AllColumns.Add(column_od);
            ListViewBeatmaps.AllColumns.Add(column_bpm);
            ListViewBeatmaps.AllColumns.Add(column_state);
            ListViewBeatmaps.AllColumns.Add(olvColumn10);
            ListViewBeatmaps.AllColumns.Add(OsuGrade);
            ListViewBeatmaps.AllColumns.Add(LastPlayed);
            ListViewBeatmaps.AllColumns.Add(EditDate);
            ListViewBeatmaps.AllColumns.Add(olvColumn13);
            ListViewBeatmaps.AllColumns.Add(TaikoGrade);
            ListViewBeatmaps.AllColumns.Add(CatchGrade);
            ListViewBeatmaps.AllColumns.Add(ManiaGrade);
            ListViewBeatmaps.AllColumns.Add(olvColumn9);
            ListViewBeatmaps.AllColumns.Add(olvColumn11);
            ListViewBeatmaps.AllColumns.Add(ScoresCount);
            ListViewBeatmaps.AllColumns.Add(LastScoreDate);
            ListViewBeatmaps.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ListViewBeatmaps.AutoGenerateColumns = false;
            ListViewBeatmaps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { olvColumn2, olvColumn4, column_stars, column_ar, column_cs, column_bpm, column_state, OsuGrade, LastPlayed, EditDate, ScoresCount, LastScoreDate });
            ListViewBeatmaps.DataSource = null;
            ListViewBeatmaps.EmptyListMsg = "No collection selected";
            ListViewBeatmaps.IsSimpleDragSource = true;
            ListViewBeatmaps.IsSimpleDropSink = true;
            ListViewBeatmaps.Location = new System.Drawing.Point(4, 47);
            ListViewBeatmaps.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ListViewBeatmaps.Name = "ListViewBeatmaps";
            ListViewBeatmaps.ShowGroups = false;
            ListViewBeatmaps.Size = new System.Drawing.Size(1094, 498);
            ListViewBeatmaps.TabIndex = 14;
            ListViewBeatmaps.UnfocusedHighlightBackgroundColor = System.Drawing.Color.FromArgb(192, 255, 192);
            ListViewBeatmaps.UseCompatibleStateImageBehavior = false;
            ListViewBeatmaps.UseCustomSelectionColors = true;
            ListViewBeatmaps.View = System.Windows.Forms.View.Details;
            ListViewBeatmaps.VirtualMode = true;
            ListViewBeatmaps.KeyUp += ListViewBeatmaps_KeyUp;
            // 
            // olvColumn2
            // 
            olvColumn2.AspectName = "Name";
            olvColumn2.AspectToStringFormat = "";
            olvColumn2.IsEditable = false;
            olvColumn2.Text = "Name";
            olvColumn2.Width = 200;
            // 
            // olvColumn4
            // 
            olvColumn4.AspectName = "DiffName";
            olvColumn4.IsEditable = false;
            olvColumn4.Text = "Difficulty";
            olvColumn4.TextCopyFormat = "[{0}]";
            olvColumn4.Width = 100;
            // 
            // column_stars
            // 
            column_stars.AspectName = "StarsNomod";
            column_stars.IsEditable = false;
            column_stars.Text = "★";
            column_stars.TextCopyFormat = "{0}★";
            column_stars.Width = 30;
            // 
            // column_ar
            // 
            column_ar.AspectName = "ApproachRate";
            column_ar.IsEditable = false;
            column_ar.Text = "AR";
            column_ar.Width = 30;
            // 
            // column_cs
            // 
            column_cs.AspectName = "CircleSize";
            column_cs.IsEditable = false;
            column_cs.Text = "CS";
            column_cs.Width = 30;
            // 
            // column_hp
            // 
            column_hp.AspectName = "HpDrainRate";
            column_hp.DisplayIndex = 4;
            column_hp.IsEditable = false;
            column_hp.IsVisible = false;
            column_hp.Text = "HP";
            column_hp.Width = 30;
            // 
            // column_od
            // 
            column_od.AspectName = "OverallDifficulty";
            column_od.DisplayIndex = 4;
            column_od.IsEditable = false;
            column_od.IsVisible = false;
            column_od.Text = "OD";
            column_od.Width = 30;
            // 
            // column_bpm
            // 
            column_bpm.AspectName = "MainBpm";
            column_bpm.IsEditable = false;
            column_bpm.Text = "BPM";
            column_bpm.Width = 40;
            // 
            // column_state
            // 
            column_state.AspectName = "StateStr";
            column_state.IsEditable = false;
            column_state.Text = "State";
            // 
            // olvColumn10
            // 
            olvColumn10.AspectName = "LocalVersionDiffers";
            olvColumn10.CheckBoxes = true;
            olvColumn10.DisplayIndex = 10;
            olvColumn10.IsEditable = false;
            olvColumn10.IsVisible = false;
            olvColumn10.Text = "Different version";
            olvColumn10.Width = 90;
            // 
            // OsuGrade
            // 
            OsuGrade.AspectName = "OsuGrade";
            OsuGrade.IsEditable = false;
            OsuGrade.Text = "Rank";
            OsuGrade.Width = 50;
            // 
            // LastPlayed
            // 
            LastPlayed.AspectName = "LastPlayed";
            LastPlayed.IsEditable = false;
            LastPlayed.Text = "Last played";
            LastPlayed.Width = 75;
            // 
            // EditDate
            // 
            EditDate.AspectName = "EditDate";
            EditDate.IsEditable = false;
            EditDate.Text = "Last update";
            EditDate.Width = 75;
            // 
            // olvColumn13
            // 
            olvColumn13.AspectName = "UserComment";
            olvColumn13.DisplayIndex = 10;
            olvColumn13.IsVisible = false;
            olvColumn13.Text = "Comment";
            // 
            // TaikoGrade
            // 
            TaikoGrade.AspectName = "TaikoGrade";
            TaikoGrade.IsEditable = false;
            TaikoGrade.IsVisible = false;
            TaikoGrade.Text = "Taiko Rank";
            // 
            // CatchGrade
            // 
            CatchGrade.AspectName = "CatchGrade";
            CatchGrade.IsEditable = false;
            CatchGrade.IsVisible = false;
            CatchGrade.Text = "Catch Rank";
            // 
            // ManiaGrade
            // 
            ManiaGrade.AspectName = "ManiaGrade";
            ManiaGrade.IsEditable = false;
            ManiaGrade.IsVisible = false;
            ManiaGrade.Text = "Mania Rank";
            // 
            // olvColumn9
            // 
            olvColumn9.AspectName = "MapSetId";
            olvColumn9.DisplayIndex = 10;
            olvColumn9.IsEditable = false;
            olvColumn9.IsVisible = false;
            olvColumn9.Text = "MapSetId";
            // 
            // olvColumn11
            // 
            olvColumn11.AspectName = "MapId";
            olvColumn11.DisplayIndex = 10;
            olvColumn11.IsEditable = false;
            olvColumn11.IsVisible = false;
            olvColumn11.Text = "MapId";
            // 
            // ScoresCount
            // 
            ScoresCount.AspectName = "ScoresCount";
            ScoresCount.IsEditable = false;
            ScoresCount.Text = "Scores";
            // 
            // LastScoreDate
            // 
            LastScoreDate.AspectName = "LastScoreDate";
            LastScoreDate.IsEditable = false;
            LastScoreDate.Text = "Last score";
            // 
            // button_searchHelp
            // 
            button_searchHelp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button_searchHelp.Location = new System.Drawing.Point(1073, 17);
            button_searchHelp.Name = "button_searchHelp";
            button_searchHelp.Size = new System.Drawing.Size(22, 23);
            button_searchHelp.TabIndex = 18;
            button_searchHelp.Text = "?";
            button_searchHelp.UseVisualStyleBackColor = true;
            button_searchHelp.Click += button_searchHelp_Click;
            // 
            // BeatmapListingView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(button_searchHelp);
            Controls.Add(ListViewBeatmaps);
            Controls.Add(label_resultsCount);
            Controls.Add(label1);
            Controls.Add(textBox_beatmapSearch);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "BeatmapListingView";
            Size = new System.Drawing.Size(1098, 546);
            BeatmapsContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ListViewBeatmaps).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        public BrightIdeasSoftware.FastDataListView ListViewBeatmaps;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private BrightIdeasSoftware.OLVColumn column_stars;
        private BrightIdeasSoftware.OLVColumn column_state;
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
        private BrightIdeasSoftware.OLVColumn olvColumn10;
        private BrightIdeasSoftware.OLVColumn LastPlayed;
        private BrightIdeasSoftware.OLVColumn EditDate;
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
        private BrightIdeasSoftware.OLVColumn column_bpm;
        private System.Windows.Forms.ToolStripMenuItem exportBeatmapSetsMenuItem;
        private System.Windows.Forms.Button button_searchHelp;
        private BrightIdeasSoftware.OLVColumn ScoresCount;
        private BrightIdeasSoftware.OLVColumn LastScoreDate;
    }
}
