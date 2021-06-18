namespace GuiComponents.Controls
{
    partial class MainSidePanelView
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Opennn = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_loadCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_loadDefaultCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_saveAllCollections = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_saveOsuCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_collectionsSplit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_unloadCollections = new System.Windows.Forms.ToolStripMenuItem();
            this.listingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_listAllCollections = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_listMissingMaps = new System.Windows.Forms.ToolStripMenuItem();
            this.onlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_mapDownloads = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_downloadAllMissing = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_GenerateCollections = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_GetMissingMapData = new System.Windows.Forms.ToolStripMenuItem();
            this.osustatsCollectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_osustatsLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_osustatsCollections = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_newCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.noCollectionsLoadedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_beatmapListing = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_refreshBeatmapList = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_resetSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.listingToolStripMenuItem,
            this.onlineToolStripMenuItem,
            this.osustatsCollectionsToolStripMenuItem,
            this.Menu_beatmapListing,
            this.Menu_refreshBeatmapList,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(724, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_Opennn,
            this.saveToolStripMenuItem,
            this.Menu_unloadCollections});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // MenuItem_Opennn
            // 
            this.MenuItem_Opennn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_loadCollection,
            this.Menu_loadDefaultCollection});
            this.MenuItem_Opennn.Name = "MenuItem_Opennn";
            this.MenuItem_Opennn.Size = new System.Drawing.Size(103, 22);
            this.MenuItem_Opennn.Text = "Open";
            // 
            // Menu_loadCollection
            // 
            this.Menu_loadCollection.Name = "Menu_loadCollection";
            this.Menu_loadCollection.Size = new System.Drawing.Size(187, 22);
            this.Menu_loadCollection.Text = "Collection(.db/.osdb)";
            // 
            // Menu_loadDefaultCollection
            // 
            this.Menu_loadDefaultCollection.Name = "Menu_loadDefaultCollection";
            this.Menu_loadDefaultCollection.Size = new System.Drawing.Size(187, 22);
            this.Menu_loadDefaultCollection.Text = "osu! collection";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_saveAllCollections,
            this.Menu_saveOsuCollection,
            this.Menu_collectionsSplit});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // Menu_saveAllCollections
            // 
            this.Menu_saveAllCollections.Name = "Menu_saveAllCollections";
            this.Menu_saveAllCollections.Size = new System.Drawing.Size(217, 22);
            this.Menu_saveAllCollections.Text = "Collection(.db/.osdb)";
            // 
            // Menu_saveOsuCollection
            // 
            this.Menu_saveOsuCollection.Name = "Menu_saveOsuCollection";
            this.Menu_saveOsuCollection.Size = new System.Drawing.Size(217, 22);
            this.Menu_saveOsuCollection.Text = "osu! collection";
            // 
            // Menu_collectionsSplit
            // 
            this.Menu_collectionsSplit.Name = "Menu_collectionsSplit";
            this.Menu_collectionsSplit.Size = new System.Drawing.Size(217, 22);
            this.Menu_collectionsSplit.Text = "Collections in separate files";
            // 
            // Menu_unloadCollections
            // 
            this.Menu_unloadCollections.Name = "Menu_unloadCollections";
            this.Menu_unloadCollections.Size = new System.Drawing.Size(103, 22);
            this.Menu_unloadCollections.Text = "Clear";
            // 
            // listingToolStripMenuItem
            // 
            this.listingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_listAllCollections,
            this.Menu_listMissingMaps});
            this.listingToolStripMenuItem.Name = "listingToolStripMenuItem";
            this.listingToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.listingToolStripMenuItem.Text = "Listing";
            // 
            // Menu_listAllCollections
            // 
            this.Menu_listAllCollections.Name = "Menu_listAllCollections";
            this.Menu_listAllCollections.Size = new System.Drawing.Size(168, 22);
            this.Menu_listAllCollections.Text = "List all collections";
            // 
            // Menu_listMissingMaps
            // 
            this.Menu_listMissingMaps.Name = "Menu_listMissingMaps";
            this.Menu_listMissingMaps.Size = new System.Drawing.Size(168, 22);
            this.Menu_listMissingMaps.Text = "List missing maps";
            // 
            // onlineToolStripMenuItem
            // 
            this.onlineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_mapDownloads,
            this.Menu_downloadAllMissing,
            this.Menu_GenerateCollections,
            this.Menu_GetMissingMapData});
            this.onlineToolStripMenuItem.Name = "onlineToolStripMenuItem";
            this.onlineToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.onlineToolStripMenuItem.Text = "Online";
            // 
            // Menu_mapDownloads
            // 
            this.Menu_mapDownloads.Name = "Menu_mapDownloads";
            this.Menu_mapDownloads.Size = new System.Drawing.Size(219, 22);
            this.Menu_mapDownloads.Text = "Show map downloads";
            // 
            // Menu_downloadAllMissing
            // 
            this.Menu_downloadAllMissing.Name = "Menu_downloadAllMissing";
            this.Menu_downloadAllMissing.Size = new System.Drawing.Size(219, 22);
            this.Menu_downloadAllMissing.Text = "Download all missing maps";
            // 
            // Menu_GenerateCollections
            // 
            this.Menu_GenerateCollections.Name = "Menu_GenerateCollections";
            this.Menu_GenerateCollections.Size = new System.Drawing.Size(219, 22);
            this.Menu_GenerateCollections.Text = "Generate collections";
            // 
            // Menu_GetMissingMapData
            // 
            this.Menu_GetMissingMapData.Name = "Menu_GetMissingMapData";
            this.Menu_GetMissingMapData.Size = new System.Drawing.Size(219, 22);
            this.Menu_GetMissingMapData.Text = "Get missing map data";
            this.Menu_GetMissingMapData.Visible = false;
            // 
            // osustatsCollectionsToolStripMenuItem
            // 
            this.osustatsCollectionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_osustatsLogin,
            this.Menu_osustatsCollections,
            this.Menu_newCollection});
            this.osustatsCollectionsToolStripMenuItem.Name = "osustatsCollectionsToolStripMenuItem";
            this.osustatsCollectionsToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.osustatsCollectionsToolStripMenuItem.Text = "Osustats collections";
            // 
            // Menu_osustatsLogin
            // 
            this.Menu_osustatsLogin.Name = "Menu_osustatsLogin";
            this.Menu_osustatsLogin.Size = new System.Drawing.Size(192, 22);
            this.Menu_osustatsLogin.Text = "Login...";
            // 
            // Menu_osustatsCollections
            // 
            this.Menu_osustatsCollections.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.Menu_osustatsCollections.Name = "Menu_osustatsCollections";
            this.Menu_osustatsCollections.Size = new System.Drawing.Size(192, 22);
            this.Menu_osustatsCollections.Text = "Your collections";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(127, 22);
            this.toolStripMenuItem2.Text = "Login first";
            // 
            // Menu_newCollection
            // 
            this.Menu_newCollection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noCollectionsLoadedToolStripMenuItem});
            this.Menu_newCollection.Name = "Menu_newCollection";
            this.Menu_newCollection.Size = new System.Drawing.Size(192, 22);
            this.Menu_newCollection.Text = "Upload new collection";
            // 
            // noCollectionsLoadedToolStripMenuItem
            // 
            this.noCollectionsLoadedToolStripMenuItem.Name = "noCollectionsLoadedToolStripMenuItem";
            this.noCollectionsLoadedToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.noCollectionsLoadedToolStripMenuItem.Text = "No new collections loaded";
            // 
            // Menu_beatmapListing
            // 
            this.Menu_beatmapListing.Name = "Menu_beatmapListing";
            this.Menu_beatmapListing.Size = new System.Drawing.Size(133, 20);
            this.Menu_beatmapListing.Text = "Show beatmap listing";
            // 
            // Menu_refreshBeatmapList
            // 
            this.Menu_refreshBeatmapList.Name = "Menu_refreshBeatmapList";
            this.Menu_refreshBeatmapList.Size = new System.Drawing.Size(58, 20);
            this.Menu_refreshBeatmapList.Text = "Refresh";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_resetSettings});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // Menu_resetSettings
            // 
            this.Menu_resetSettings.Name = "Menu_resetSettings";
            this.Menu_resetSettings.Size = new System.Drawing.Size(102, 22);
            this.Menu_resetSettings.Text = "Reset";
            // 
            // MainSidePanelView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainSidePanelView";
            this.Size = new System.Drawing.Size(724, 23);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Opennn;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu_listAllCollections;
        private System.Windows.Forms.ToolStripMenuItem Menu_listMissingMaps;
        private System.Windows.Forms.ToolStripMenuItem onlineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu_mapDownloads;
        private System.Windows.Forms.ToolStripMenuItem Menu_downloadAllMissing;
        private System.Windows.Forms.ToolStripMenuItem Menu_GenerateCollections;
        private System.Windows.Forms.ToolStripMenuItem Menu_GetMissingMapData;
        private System.Windows.Forms.ToolStripMenuItem osustatsCollectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu_osustatsLogin;
        private System.Windows.Forms.ToolStripMenuItem Menu_osustatsCollections;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem Menu_loadCollection;
        private System.Windows.Forms.ToolStripMenuItem Menu_loadDefaultCollection;
        private System.Windows.Forms.ToolStripMenuItem Menu_saveAllCollections;
        private System.Windows.Forms.ToolStripMenuItem Menu_collectionsSplit;
        private System.Windows.Forms.ToolStripMenuItem Menu_unloadCollections;
        private System.Windows.Forms.ToolStripMenuItem Menu_beatmapListing;
        private System.Windows.Forms.ToolStripMenuItem Menu_refreshBeatmapList;
        private System.Windows.Forms.ToolStripMenuItem Menu_newCollection;
        private System.Windows.Forms.ToolStripMenuItem noCollectionsLoadedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu_saveOsuCollection;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu_resetSettings;
    }
}
