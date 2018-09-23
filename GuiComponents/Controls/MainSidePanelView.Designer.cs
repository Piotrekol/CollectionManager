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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_mapDownloads = new System.Windows.Forms.Button();
            this.button_beatmapListing = new System.Windows.Forms.Button();
            this.groupBox_onlineServices = new System.Windows.Forms.GroupBox();
            this.button_GetMissingMapData = new System.Windows.Forms.Button();
            this.button_downloadAllMissing = new System.Windows.Forms.Button();
            this.button_GenerateCollections = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_listMissingMaps = new System.Windows.Forms.Button();
            this.button_listAllCollections = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_saveAllCollections = new System.Windows.Forms.Button();
            this.button_collectionsSplit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_unloadCollections = new System.Windows.Forms.Button();
            this.button_loadDefaultCollection = new System.Windows.Forms.Button();
            this.button_loadCollection = new System.Windows.Forms.Button();
            this.button_refreshBeatmapList = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox_onlineServices.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_mapDownloads);
            this.panel1.Controls.Add(this.button_beatmapListing);
            this.panel1.Controls.Add(this.groupBox_onlineServices);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.button_refreshBeatmapList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(184, 512);
            this.panel1.TabIndex = 9;
            // 
            // button_mapDownloads
            // 
            this.button_mapDownloads.Location = new System.Drawing.Point(4, 311);
            this.button_mapDownloads.Name = "button_mapDownloads";
            this.button_mapDownloads.Size = new System.Drawing.Size(170, 23);
            this.button_mapDownloads.TabIndex = 13;
            this.button_mapDownloads.Text = "Show map downloads";
            this.button_mapDownloads.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_mapDownloads.UseVisualStyleBackColor = true;
            // 
            // button_beatmapListing
            // 
            this.button_beatmapListing.Location = new System.Drawing.Point(4, 282);
            this.button_beatmapListing.Name = "button_beatmapListing";
            this.button_beatmapListing.Size = new System.Drawing.Size(170, 23);
            this.button_beatmapListing.TabIndex = 12;
            this.button_beatmapListing.Text = "Show beatmap listing";
            this.button_beatmapListing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_beatmapListing.UseVisualStyleBackColor = true;
            // 
            // groupBox_onlineServices
            // 
            this.groupBox_onlineServices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_onlineServices.Controls.Add(this.button_GetMissingMapData);
            this.groupBox_onlineServices.Controls.Add(this.button_downloadAllMissing);
            this.groupBox_onlineServices.Controls.Add(this.button_GenerateCollections);
            this.groupBox_onlineServices.Location = new System.Drawing.Point(3, 340);
            this.groupBox_onlineServices.Name = "groupBox_onlineServices";
            this.groupBox_onlineServices.Size = new System.Drawing.Size(174, 103);
            this.groupBox_onlineServices.TabIndex = 12;
            this.groupBox_onlineServices.TabStop = false;
            this.groupBox_onlineServices.Text = "Online services";
            // 
            // button_GetMissingMapData
            // 
            this.button_GetMissingMapData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_GetMissingMapData.Location = new System.Drawing.Point(4, 74);
            this.button_GetMissingMapData.Name = "button_GetMissingMapData";
            this.button_GetMissingMapData.Size = new System.Drawing.Size(170, 23);
            this.button_GetMissingMapData.TabIndex = 15;
            this.button_GetMissingMapData.Text = "Get missing map data";
            this.button_GetMissingMapData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_GetMissingMapData.UseVisualStyleBackColor = true;
            this.button_GetMissingMapData.Visible = false;
            // 
            // button_downloadAllMissing
            // 
            this.button_downloadAllMissing.Location = new System.Drawing.Point(4, 18);
            this.button_downloadAllMissing.Name = "button_downloadAllMissing";
            this.button_downloadAllMissing.Size = new System.Drawing.Size(170, 23);
            this.button_downloadAllMissing.TabIndex = 14;
            this.button_downloadAllMissing.Text = "Download all missing maps";
            this.button_downloadAllMissing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_downloadAllMissing.UseVisualStyleBackColor = true;
            // 
            // button_GenerateCollections
            // 
            this.button_GenerateCollections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_GenerateCollections.Location = new System.Drawing.Point(4, 47);
            this.button_GenerateCollections.Name = "button_GenerateCollections";
            this.button_GenerateCollections.Size = new System.Drawing.Size(170, 23);
            this.button_GenerateCollections.TabIndex = 11;
            this.button_GenerateCollections.Text = "Generate collections";
            this.button_GenerateCollections.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_GenerateCollections.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.button_listMissingMaps);
            this.groupBox3.Controls.Add(this.button_listAllCollections);
            this.groupBox3.Location = new System.Drawing.Point(3, 198);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(174, 76);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Collections listing";
            // 
            // button_listMissingMaps
            // 
            this.button_listMissingMaps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_listMissingMaps.Location = new System.Drawing.Point(1, 48);
            this.button_listMissingMaps.Name = "button_listMissingMaps";
            this.button_listMissingMaps.Size = new System.Drawing.Size(170, 23);
            this.button_listMissingMaps.TabIndex = 8;
            this.button_listMissingMaps.Text = "List missing maps";
            this.button_listMissingMaps.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_listMissingMaps.UseVisualStyleBackColor = true;
            // 
            // button_listAllCollections
            // 
            this.button_listAllCollections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_listAllCollections.Location = new System.Drawing.Point(1, 19);
            this.button_listAllCollections.Name = "button_listAllCollections";
            this.button_listAllCollections.Size = new System.Drawing.Size(170, 23);
            this.button_listAllCollections.TabIndex = 7;
            this.button_listAllCollections.Text = "List all collections";
            this.button_listAllCollections.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_listAllCollections.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button_saveAllCollections);
            this.groupBox2.Controls.Add(this.button_collectionsSplit);
            this.groupBox2.Location = new System.Drawing.Point(3, 121);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(174, 76);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Collections saving";
            // 
            // button_saveAllCollections
            // 
            this.button_saveAllCollections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_saveAllCollections.Location = new System.Drawing.Point(1, 19);
            this.button_saveAllCollections.Name = "button_saveAllCollections";
            this.button_saveAllCollections.Size = new System.Drawing.Size(170, 23);
            this.button_saveAllCollections.TabIndex = 5;
            this.button_saveAllCollections.Text = "Save Collections";
            this.button_saveAllCollections.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_saveAllCollections.UseVisualStyleBackColor = true;
            // 
            // button_collectionsSplit
            // 
            this.button_collectionsSplit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_collectionsSplit.Location = new System.Drawing.Point(1, 48);
            this.button_collectionsSplit.Name = "button_collectionsSplit";
            this.button_collectionsSplit.Size = new System.Drawing.Size(170, 23);
            this.button_collectionsSplit.TabIndex = 6;
            this.button_collectionsSplit.Text = "Save collections in separate files";
            this.button_collectionsSplit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_collectionsSplit.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_unloadCollections);
            this.groupBox1.Controls.Add(this.button_loadDefaultCollection);
            this.groupBox1.Controls.Add(this.button_loadCollection);
            this.groupBox1.Location = new System.Drawing.Point(3, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(174, 108);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Collection loading";
            // 
            // button_unloadCollections
            // 
            this.button_unloadCollections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_unloadCollections.Location = new System.Drawing.Point(1, 77);
            this.button_unloadCollections.Name = "button_unloadCollections";
            this.button_unloadCollections.Size = new System.Drawing.Size(170, 23);
            this.button_unloadCollections.TabIndex = 8;
            this.button_unloadCollections.Text = "Clear collections";
            this.button_unloadCollections.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_unloadCollections.UseVisualStyleBackColor = true;
            // 
            // button_loadDefaultCollection
            // 
            this.button_loadDefaultCollection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_loadDefaultCollection.Location = new System.Drawing.Point(1, 48);
            this.button_loadDefaultCollection.Name = "button_loadDefaultCollection";
            this.button_loadDefaultCollection.Size = new System.Drawing.Size(170, 23);
            this.button_loadDefaultCollection.TabIndex = 7;
            this.button_loadDefaultCollection.Text = "Load osu! collection";
            this.button_loadDefaultCollection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_loadDefaultCollection.UseVisualStyleBackColor = true;
            // 
            // button_loadCollection
            // 
            this.button_loadCollection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_loadCollection.Location = new System.Drawing.Point(1, 19);
            this.button_loadCollection.Name = "button_loadCollection";
            this.button_loadCollection.Size = new System.Drawing.Size(170, 23);
            this.button_loadCollection.TabIndex = 3;
            this.button_loadCollection.Text = "Load collection( .db/.osdb)";
            this.button_loadCollection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_loadCollection.UseVisualStyleBackColor = true;
            // 
            // button_refreshBeatmapList
            // 
            this.button_refreshBeatmapList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_refreshBeatmapList.Location = new System.Drawing.Point(114, 475);
            this.button_refreshBeatmapList.Name = "button_refreshBeatmapList";
            this.button_refreshBeatmapList.Size = new System.Drawing.Size(60, 23);
            this.button_refreshBeatmapList.TabIndex = 10;
            this.button_refreshBeatmapList.Text = "Refresh";
            this.button_refreshBeatmapList.UseVisualStyleBackColor = true;
            this.button_refreshBeatmapList.Visible = false;
            // 
            // MainSidePanelView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "MainSidePanelView";
            this.Size = new System.Drawing.Size(184, 512);
            this.panel1.ResumeLayout(false);
            this.groupBox_onlineServices.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Button button_mapDownloads;
        public System.Windows.Forms.Button button_beatmapListing;
        public System.Windows.Forms.GroupBox groupBox_onlineServices;
        public System.Windows.Forms.Button button_GenerateCollections;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.Button button_listMissingMaps;
        public System.Windows.Forms.Button button_listAllCollections;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.Button button_saveAllCollections;
        public System.Windows.Forms.Button button_collectionsSplit;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Button button_loadDefaultCollection;
        public System.Windows.Forms.Button button_loadCollection;
        public System.Windows.Forms.Button button_unloadCollections;
        public System.Windows.Forms.Button button_refreshBeatmapList;
        public System.Windows.Forms.Button button_downloadAllMissing;
        public System.Windows.Forms.Button button_GetMissingMapData;
    }
}
