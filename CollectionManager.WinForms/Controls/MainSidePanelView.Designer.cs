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
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_Opennn = new System.Windows.Forms.ToolStripMenuItem();
            Menu_loadCollection = new System.Windows.Forms.ToolStripMenuItem();
            Menu_loadDefaultCollection = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            Menu_saveAllCollections = new System.Windows.Forms.ToolStripMenuItem();
            Menu_saveOsuCollection = new System.Windows.Forms.ToolStripMenuItem();
            Menu_collectionsSplit = new System.Windows.Forms.ToolStripMenuItem();
            listingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            Menu_listAllCollections = new System.Windows.Forms.ToolStripMenuItem();
            Menu_listMissingMaps = new System.Windows.Forms.ToolStripMenuItem();
            Menu_unloadCollections = new System.Windows.Forms.ToolStripMenuItem();
            onlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            Menu_mapDownloads = new System.Windows.Forms.ToolStripMenuItem();
            Menu_downloadAllMissing = new System.Windows.Forms.ToolStripMenuItem();
            Menu_GenerateCollections = new System.Windows.Forms.ToolStripMenuItem();
            Menu_GetMissingMapData = new System.Windows.Forms.ToolStripMenuItem();
            osustatsCollectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            Menu_osustatsLogin = new System.Windows.Forms.ToolStripMenuItem();
            Menu_osustatsCollections = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            Menu_newCollection = new System.Windows.Forms.ToolStripMenuItem();
            noCollectionsLoadedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            Menu_beatmapListing = new System.Windows.Forms.ToolStripMenuItem();
            Menu_refreshBeatmapList = new System.Windows.Forms.ToolStripMenuItem();
            settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            Menu_resetSettings = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, onlineToolStripMenuItem, osustatsCollectionsToolStripMenuItem, Menu_beatmapListing, Menu_refreshBeatmapList, settingsToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(845, 24);
            menuStrip1.TabIndex = 10;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MenuItem_Opennn, saveToolStripMenuItem, listingToolStripMenuItem1, Menu_unloadCollections });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // MenuItem_Opennn
            // 
            MenuItem_Opennn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Menu_loadCollection, Menu_loadDefaultCollection });
            MenuItem_Opennn.Name = "MenuItem_Opennn";
            MenuItem_Opennn.Size = new System.Drawing.Size(109, 22);
            MenuItem_Opennn.Text = "Open";
            // 
            // Menu_loadCollection
            // 
            Menu_loadCollection.Name = "Menu_loadCollection";
            Menu_loadCollection.Size = new System.Drawing.Size(187, 22);
            Menu_loadCollection.Text = "Collection(.db/.osdb)";
            // 
            // Menu_loadDefaultCollection
            // 
            Menu_loadDefaultCollection.Name = "Menu_loadDefaultCollection";
            Menu_loadDefaultCollection.Size = new System.Drawing.Size(187, 22);
            Menu_loadDefaultCollection.Text = "osu! collection";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Menu_saveAllCollections, Menu_saveOsuCollection, Menu_collectionsSplit });
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            saveToolStripMenuItem.Text = "Save";
            // 
            // Menu_saveAllCollections
            // 
            Menu_saveAllCollections.Name = "Menu_saveAllCollections";
            Menu_saveAllCollections.Size = new System.Drawing.Size(217, 22);
            Menu_saveAllCollections.Text = "Collection(.db/.osdb)";
            // 
            // Menu_saveOsuCollection
            // 
            Menu_saveOsuCollection.Name = "Menu_saveOsuCollection";
            Menu_saveOsuCollection.Size = new System.Drawing.Size(217, 22);
            Menu_saveOsuCollection.Text = "osu! collection";
            // 
            // Menu_collectionsSplit
            // 
            Menu_collectionsSplit.Name = "Menu_collectionsSplit";
            Menu_collectionsSplit.Size = new System.Drawing.Size(217, 22);
            Menu_collectionsSplit.Text = "Collections in separate files";
            // 
            // listingToolStripMenuItem1
            // 
            listingToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Menu_listAllCollections, Menu_listMissingMaps });
            listingToolStripMenuItem1.Name = "listingToolStripMenuItem1";
            listingToolStripMenuItem1.Size = new System.Drawing.Size(109, 22);
            listingToolStripMenuItem1.Text = "Listing";
            // 
            // Menu_listAllCollections
            // 
            Menu_listAllCollections.Name = "Menu_listAllCollections";
            Menu_listAllCollections.Size = new System.Drawing.Size(168, 22);
            Menu_listAllCollections.Text = "List all collections";
            // 
            // Menu_listMissingMaps
            // 
            Menu_listMissingMaps.Name = "Menu_listMissingMaps";
            Menu_listMissingMaps.Size = new System.Drawing.Size(168, 22);
            Menu_listMissingMaps.Text = "List missing maps";
            // 
            // Menu_unloadCollections
            // 
            Menu_unloadCollections.Name = "Menu_unloadCollections";
            Menu_unloadCollections.Size = new System.Drawing.Size(109, 22);
            Menu_unloadCollections.Text = "Clear";
            // 
            // onlineToolStripMenuItem
            // 
            onlineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Menu_mapDownloads, Menu_downloadAllMissing, Menu_GenerateCollections, Menu_GetMissingMapData });
            onlineToolStripMenuItem.Name = "onlineToolStripMenuItem";
            onlineToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            onlineToolStripMenuItem.Text = "Online";
            // 
            // Menu_mapDownloads
            // 
            Menu_mapDownloads.Name = "Menu_mapDownloads";
            Menu_mapDownloads.Size = new System.Drawing.Size(219, 22);
            Menu_mapDownloads.Text = "Show map downloads";
            // 
            // Menu_downloadAllMissing
            // 
            Menu_downloadAllMissing.Name = "Menu_downloadAllMissing";
            Menu_downloadAllMissing.Size = new System.Drawing.Size(219, 22);
            Menu_downloadAllMissing.Text = "Download all missing maps";
            // 
            // Menu_GenerateCollections
            // 
            Menu_GenerateCollections.Name = "Menu_GenerateCollections";
            Menu_GenerateCollections.Size = new System.Drawing.Size(219, 22);
            Menu_GenerateCollections.Text = "Generate collections";
            // 
            // Menu_GetMissingMapData
            // 
            Menu_GetMissingMapData.Name = "Menu_GetMissingMapData";
            Menu_GetMissingMapData.Size = new System.Drawing.Size(219, 22);
            Menu_GetMissingMapData.Text = "Get missing map data";
            Menu_GetMissingMapData.Visible = false;
            // 
            // osustatsCollectionsToolStripMenuItem
            // 
            osustatsCollectionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Menu_osustatsLogin, Menu_osustatsCollections, Menu_newCollection });
            osustatsCollectionsToolStripMenuItem.Name = "osustatsCollectionsToolStripMenuItem";
            osustatsCollectionsToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            osustatsCollectionsToolStripMenuItem.Text = "Osustats collections";
            // 
            // Menu_osustatsLogin
            // 
            Menu_osustatsLogin.Name = "Menu_osustatsLogin";
            Menu_osustatsLogin.Size = new System.Drawing.Size(192, 22);
            Menu_osustatsLogin.Text = "Login...";
            // 
            // Menu_osustatsCollections
            // 
            Menu_osustatsCollections.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem2 });
            Menu_osustatsCollections.Name = "Menu_osustatsCollections";
            Menu_osustatsCollections.Size = new System.Drawing.Size(192, 22);
            Menu_osustatsCollections.Text = "Your collections";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(127, 22);
            toolStripMenuItem2.Text = "Login first";
            // 
            // Menu_newCollection
            // 
            Menu_newCollection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { noCollectionsLoadedToolStripMenuItem });
            Menu_newCollection.Name = "Menu_newCollection";
            Menu_newCollection.Size = new System.Drawing.Size(192, 22);
            Menu_newCollection.Text = "Upload new collection";
            // 
            // noCollectionsLoadedToolStripMenuItem
            // 
            noCollectionsLoadedToolStripMenuItem.Name = "noCollectionsLoadedToolStripMenuItem";
            noCollectionsLoadedToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            noCollectionsLoadedToolStripMenuItem.Text = "No new collections loaded";
            // 
            // Menu_beatmapListing
            // 
            Menu_beatmapListing.Name = "Menu_beatmapListing";
            Menu_beatmapListing.Size = new System.Drawing.Size(101, 20);
            Menu_beatmapListing.Text = "Beatmap listing";
            // 
            // Menu_refreshBeatmapList
            // 
            Menu_refreshBeatmapList.Name = "Menu_refreshBeatmapList";
            Menu_refreshBeatmapList.Size = new System.Drawing.Size(58, 20);
            Menu_refreshBeatmapList.Text = "Refresh";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Menu_resetSettings });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // Menu_resetSettings
            // 
            Menu_resetSettings.Name = "Menu_resetSettings";
            Menu_resetSettings.Size = new System.Drawing.Size(102, 22);
            Menu_resetSettings.Text = "Reset";
            // 
            // MainSidePanelView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(menuStrip1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MainSidePanelView";
            Size = new System.Drawing.Size(845, 27);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Opennn;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem listingToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem Menu_listAllCollections;
        private System.Windows.Forms.ToolStripMenuItem Menu_listMissingMaps;
    }
}
