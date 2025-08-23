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
            column_name = new BrightIdeasSoftware.OLVColumn();
            olvColumn4 = new BrightIdeasSoftware.OLVColumn();
            column_stars = new BrightIdeasSoftware.OLVColumn();
            column_ar = new BrightIdeasSoftware.OLVColumn();
            column_cs = new BrightIdeasSoftware.OLVColumn();
            column_hp = new BrightIdeasSoftware.OLVColumn();
            column_od = new BrightIdeasSoftware.OLVColumn();
            column_bpm = new BrightIdeasSoftware.OLVColumn();
            column_state = new BrightIdeasSoftware.OLVColumn();
            column_LocalVersionDiffers = new BrightIdeasSoftware.OLVColumn();
            OsuGrade = new BrightIdeasSoftware.OLVColumn();
            column_LastPlayed = new BrightIdeasSoftware.OLVColumn();
            column_EditDate = new BrightIdeasSoftware.OLVColumn();
            column_Comment = new BrightIdeasSoftware.OLVColumn();
            TaikoGrade = new BrightIdeasSoftware.OLVColumn();
            CatchGrade = new BrightIdeasSoftware.OLVColumn();
            ManiaGrade = new BrightIdeasSoftware.OLVColumn();
            column_SetId = new BrightIdeasSoftware.OLVColumn();
            column_MapId = new BrightIdeasSoftware.OLVColumn();
            ScoresCount = new BrightIdeasSoftware.OLVColumn();
            column_LastScoreDate = new BrightIdeasSoftware.OLVColumn();
            column_Directory = new BrightIdeasSoftware.OLVColumn();
            button_searchHelp = new System.Windows.Forms.Button();
            comboBox_grouping = new System.Windows.Forms.ComboBox();
            BeatmapsContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ListViewBeatmaps).BeginInit();
            SuspendLayout();
            // 
            // label_resultsCount
            // 
            label_resultsCount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label_resultsCount.Location = new System.Drawing.Point(954, 5);
            label_resultsCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_resultsCount.Name = "label_resultsCount";
            label_resultsCount.Size = new System.Drawing.Size(140, 15);
            label_resultsCount.TabIndex = 17;
            label_resultsCount.Text = "0 maps";
            label_resultsCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBox_beatmapSearch
            // 
            textBox_beatmapSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox_beatmapSearch.Location = new System.Drawing.Point(1, 1);
            textBox_beatmapSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox_beatmapSearch.Name = "textBox_beatmapSearch";
            textBox_beatmapSearch.PlaceholderText = "Search just like you do in osu!";
            textBox_beatmapSearch.Size = new System.Drawing.Size(758, 23);
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
            ListViewBeatmaps.AllColumns.Add(column_name);
            ListViewBeatmaps.AllColumns.Add(olvColumn4);
            ListViewBeatmaps.AllColumns.Add(column_stars);
            ListViewBeatmaps.AllColumns.Add(column_ar);
            ListViewBeatmaps.AllColumns.Add(column_cs);
            ListViewBeatmaps.AllColumns.Add(column_hp);
            ListViewBeatmaps.AllColumns.Add(column_od);
            ListViewBeatmaps.AllColumns.Add(column_bpm);
            ListViewBeatmaps.AllColumns.Add(column_state);
            ListViewBeatmaps.AllColumns.Add(column_LocalVersionDiffers);
            ListViewBeatmaps.AllColumns.Add(OsuGrade);
            ListViewBeatmaps.AllColumns.Add(column_LastPlayed);
            ListViewBeatmaps.AllColumns.Add(column_EditDate);
            ListViewBeatmaps.AllColumns.Add(column_Comment);
            ListViewBeatmaps.AllColumns.Add(TaikoGrade);
            ListViewBeatmaps.AllColumns.Add(CatchGrade);
            ListViewBeatmaps.AllColumns.Add(ManiaGrade);
            ListViewBeatmaps.AllColumns.Add(column_SetId);
            ListViewBeatmaps.AllColumns.Add(column_MapId);
            ListViewBeatmaps.AllColumns.Add(ScoresCount);
            ListViewBeatmaps.AllColumns.Add(column_LastScoreDate);
            ListViewBeatmaps.AllColumns.Add(column_Directory);
            ListViewBeatmaps.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ListViewBeatmaps.AutoGenerateColumns = false;
            ListViewBeatmaps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { column_name, olvColumn4, column_stars, column_ar, column_cs, column_bpm, column_state, OsuGrade, column_LastPlayed, column_EditDate, ScoresCount, column_LastScoreDate, column_Directory });
            ListViewBeatmaps.DataSource = null;
            ListViewBeatmaps.EmptyListMsg = "No collection selected";
            ListViewBeatmaps.IsSimpleDragSource = true;
            ListViewBeatmaps.IsSimpleDropSink = true;
            ListViewBeatmaps.Location = new System.Drawing.Point(1, 30);
            ListViewBeatmaps.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ListViewBeatmaps.Name = "ListViewBeatmaps";
            ListViewBeatmaps.ShowGroups = false;
            ListViewBeatmaps.Size = new System.Drawing.Size(1096, 536);
            ListViewBeatmaps.TabIndex = 14;
            ListViewBeatmaps.UnfocusedHighlightBackgroundColor = System.Drawing.Color.FromArgb(192, 255, 192);
            ListViewBeatmaps.UseCompatibleStateImageBehavior = false;
            ListViewBeatmaps.UseCustomSelectionColors = true;
            ListViewBeatmaps.View = System.Windows.Forms.View.Details;
            ListViewBeatmaps.VirtualMode = true;
            ListViewBeatmaps.KeyUp += ListViewBeatmaps_KeyUp;
            // 
            // column_name
            // 
            column_name.AspectName = "Name";
            column_name.AspectToStringFormat = "";
            column_name.IsEditable = false;
            column_name.Text = "Name";
            column_name.Width = 200;
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
            // column_LocalVersionDiffers
            // 
            column_LocalVersionDiffers.AspectName = "LocalVersionDiffers";
            column_LocalVersionDiffers.CheckBoxes = true;
            column_LocalVersionDiffers.DisplayIndex = 10;
            column_LocalVersionDiffers.IsEditable = false;
            column_LocalVersionDiffers.IsVisible = false;
            column_LocalVersionDiffers.Text = "Different version";
            column_LocalVersionDiffers.Width = 90;
            // 
            // OsuGrade
            // 
            OsuGrade.AspectName = "OsuGrade";
            OsuGrade.IsEditable = false;
            OsuGrade.Text = "Rank";
            OsuGrade.Width = 50;
            // 
            // column_LastPlayed
            // 
            column_LastPlayed.AspectName = "LastPlayed";
            column_LastPlayed.IsEditable = false;
            column_LastPlayed.Text = "Last played";
            column_LastPlayed.Width = 75;
            // 
            // column_EditDate
            // 
            column_EditDate.AspectName = "EditDate";
            column_EditDate.IsEditable = false;
            column_EditDate.Text = "Last update";
            column_EditDate.Width = 75;
            // 
            // column_Comment
            // 
            column_Comment.AspectName = "UserComment";
            column_Comment.DisplayIndex = 10;
            column_Comment.IsVisible = false;
            column_Comment.Text = "Comment";
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
            // column_SetId
            // 
            column_SetId.AspectName = "MapSetId";
            column_SetId.DisplayIndex = 10;
            column_SetId.IsEditable = false;
            column_SetId.IsVisible = false;
            column_SetId.Text = "MapSetId";
            // 
            // column_MapId
            // 
            column_MapId.AspectName = "MapId";
            column_MapId.DisplayIndex = 10;
            column_MapId.IsEditable = false;
            column_MapId.IsVisible = false;
            column_MapId.Text = "MapId";
            // 
            // ScoresCount
            // 
            ScoresCount.AspectName = "ScoresCount";
            ScoresCount.IsEditable = false;
            ScoresCount.Text = "Scores";
            // 
            // column_LastScoreDate
            // 
            column_LastScoreDate.AspectName = "LastScoreDate";
            column_LastScoreDate.IsEditable = false;
            column_LastScoreDate.Text = "Last score";
            // 
            // column_Directory
            // 
            column_Directory.AspectName = "Dir";
            column_Directory.IsEditable = false;
            column_Directory.IsVisible = false;
            column_Directory.Text = "Directory";
            // 
            // button_searchHelp
            // 
            button_searchHelp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button_searchHelp.Location = new System.Drawing.Point(925, 0);
            button_searchHelp.Name = "button_searchHelp";
            button_searchHelp.Size = new System.Drawing.Size(22, 25);
            button_searchHelp.TabIndex = 18;
            button_searchHelp.Text = "?";
            button_searchHelp.UseVisualStyleBackColor = true;
            button_searchHelp.Click += button_searchHelp_Click;
            // 
            // comboBox_grouping
            // 
            comboBox_grouping.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            comboBox_grouping.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox_grouping.FormattingEnabled = true;
            comboBox_grouping.Location = new System.Drawing.Point(766, 1);
            comboBox_grouping.Name = "comboBox_grouping";
            comboBox_grouping.Size = new System.Drawing.Size(153, 23);
            comboBox_grouping.TabIndex = 19;
            comboBox_grouping.SelectedIndexChanged += comboBox_grouping_SelectedIndexChanged;
            // 
            // BeatmapListingView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(comboBox_grouping);
            Controls.Add(button_searchHelp);
            Controls.Add(ListViewBeatmaps);
            Controls.Add(label_resultsCount);
            Controls.Add(textBox_beatmapSearch);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "BeatmapListingView";
            Size = new System.Drawing.Size(1098, 567);
            BeatmapsContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ListViewBeatmaps).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        public BrightIdeasSoftware.FastDataListView ListViewBeatmaps;
        private BrightIdeasSoftware.OLVColumn column_name;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private BrightIdeasSoftware.OLVColumn column_stars;
        private BrightIdeasSoftware.OLVColumn column_state;
        private BrightIdeasSoftware.OLVColumn column_ar;
        private BrightIdeasSoftware.OLVColumn column_cs;
        private BrightIdeasSoftware.OLVColumn column_hp;
        private BrightIdeasSoftware.OLVColumn column_od;
        public System.Windows.Forms.Label label_resultsCount;
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
        private BrightIdeasSoftware.OLVColumn column_LocalVersionDiffers;
        private BrightIdeasSoftware.OLVColumn column_LastPlayed;
        private BrightIdeasSoftware.OLVColumn column_EditDate;
        private System.Windows.Forms.ToolStripMenuItem OpenBeatmapFolderMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyUrlMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyAsTextMenuStrip;
        private BrightIdeasSoftware.OLVColumn column_Comment;
        private BrightIdeasSoftware.OLVColumn OsuGrade;
        private BrightIdeasSoftware.OLVColumn TaikoGrade;
        private BrightIdeasSoftware.OLVColumn CatchGrade;
        private BrightIdeasSoftware.OLVColumn ManiaGrade;
        private System.Windows.Forms.ToolStripMenuItem PullMapsetMenuStrip;
        private BrightIdeasSoftware.OLVColumn column_SetId;
        private BrightIdeasSoftware.OLVColumn column_MapId;
        private BrightIdeasSoftware.OLVColumn column_bpm;
        private System.Windows.Forms.ToolStripMenuItem exportBeatmapSetsMenuItem;
        private System.Windows.Forms.Button button_searchHelp;
        private BrightIdeasSoftware.OLVColumn ScoresCount;
        private BrightIdeasSoftware.OLVColumn column_LastScoreDate;
        private BrightIdeasSoftware.OLVColumn column_Directory;
        private System.Windows.Forms.ComboBox comboBox_grouping;
    }
}
