namespace GuiComponents.Controls
{
    partial class CollectionListingView
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ListViewCollections = new BrightIdeasSoftware.FastObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Total = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.textBox_collectionNameSearch = new System.Windows.Forms.TextBox();
            this.CollectionContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameCollectionMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCollectionMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.mergeWithMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.intersectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.DuplicateMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.inverseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListViewCollections)).BeginInit();
            this.CollectionContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ListViewCollections);
            this.panel1.Controls.Add(this.textBox_collectionNameSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(464, 384);
            this.panel1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Search using collection names:";
            // 
            // ListViewCollections
            // 
            this.ListViewCollections.AllColumns.Add(this.olvColumn1);
            this.ListViewCollections.AllColumns.Add(this.Total);
            this.ListViewCollections.AllColumns.Add(this.olvColumn2);
            this.ListViewCollections.AllowDrop = true;
            this.ListViewCollections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListViewCollections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.Total,
            this.olvColumn2});
            this.ListViewCollections.EmptyListMsg = "No collections loaded";
            this.ListViewCollections.HideSelection = false;
            this.ListViewCollections.Location = new System.Drawing.Point(1, 39);
            this.ListViewCollections.Name = "ListViewCollections";
            this.ListViewCollections.ShowGroups = false;
            this.ListViewCollections.Size = new System.Drawing.Size(463, 345);
            this.ListViewCollections.TabIndex = 5;
            this.ListViewCollections.UnfocusedHighlightBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ListViewCollections.UseCompatibleStateImageBehavior = false;
            this.ListViewCollections.UseCustomSelectionColors = true;
            this.ListViewCollections.View = System.Windows.Forms.View.Details;
            this.ListViewCollections.VirtualMode = true;
            this.ListViewCollections.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ListViewCollections_KeyUp);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Name";
            this.olvColumn1.MaximumWidth = 400;
            this.olvColumn1.MinimumWidth = 20;
            this.olvColumn1.Text = "Name";
            this.olvColumn1.Width = 100;
            // 
            // Total
            // 
            this.Total.AspectName = "NumberOfBeatmaps";
            this.Total.Text = "Count";
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "NumberOfMissingBeatmaps";
            this.olvColumn2.Text = "Missing";
            // 
            // textBox_collectionNameSearch
            // 
            this.textBox_collectionNameSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_collectionNameSearch.Location = new System.Drawing.Point(1, 16);
            this.textBox_collectionNameSearch.Name = "textBox_collectionNameSearch";
            this.textBox_collectionNameSearch.Size = new System.Drawing.Size(463, 20);
            this.textBox_collectionNameSearch.TabIndex = 1;
            // 
            // CollectionContextMenuStrip
            // 
            this.CollectionContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateMenuStrip,
            this.renameCollectionMenuStrip,
            this.deleteCollectionMenuStrip,
            this.DuplicateMenuStrip,
            this.mergeWithMenuStrip,
            this.intersectMenuItem,
            this.inverseToolStripMenuItem});
            this.CollectionContextMenuStrip.Name = "CollectionContextMenuStrip";
            this.CollectionContextMenuStrip.Size = new System.Drawing.Size(181, 180);
            // 
            // renameCollectionMenuStrip
            // 
            this.renameCollectionMenuStrip.Name = "renameCollectionMenuStrip";
            this.renameCollectionMenuStrip.ShortcutKeyDisplayString = "F2";
            this.renameCollectionMenuStrip.Size = new System.Drawing.Size(180, 22);
            this.renameCollectionMenuStrip.Tag = "Rename";
            this.renameCollectionMenuStrip.Text = "Rename";
            this.renameCollectionMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // deleteCollectionMenuStrip
            // 
            this.deleteCollectionMenuStrip.Name = "deleteCollectionMenuStrip";
            this.deleteCollectionMenuStrip.ShortcutKeyDisplayString = "Del";
            this.deleteCollectionMenuStrip.Size = new System.Drawing.Size(180, 22);
            this.deleteCollectionMenuStrip.Tag = "Delete";
            this.deleteCollectionMenuStrip.Text = "Delete";
            this.deleteCollectionMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // mergeWithMenuStrip
            // 
            this.mergeWithMenuStrip.Name = "mergeWithMenuStrip";
            this.mergeWithMenuStrip.Size = new System.Drawing.Size(180, 22);
            this.mergeWithMenuStrip.Tag = "Merge";
            this.mergeWithMenuStrip.Text = "Merge selected";
            this.mergeWithMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // intersectMenuItem
            // 
            this.intersectMenuItem.Name = "intersectMenuItem";
            this.intersectMenuItem.Size = new System.Drawing.Size(180, 22);
            this.intersectMenuItem.Tag = "Intersect";
            this.intersectMenuItem.Text = "Intersection";
            this.intersectMenuItem.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // CreateMenuStrip
            // 
            this.CreateMenuStrip.Name = "CreateMenuStrip";
            this.CreateMenuStrip.Size = new System.Drawing.Size(180, 22);
            this.CreateMenuStrip.Tag = "Create";
            this.CreateMenuStrip.Text = "Create";
            this.CreateMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // DuplicateMenuStrip
            // 
            this.DuplicateMenuStrip.Name = "DuplicateMenuStrip";
            this.DuplicateMenuStrip.Size = new System.Drawing.Size(180, 22);
            this.DuplicateMenuStrip.Tag = "Duplicate";
            this.DuplicateMenuStrip.Text = "Duplicate";
            this.DuplicateMenuStrip.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // inverseToolStripMenuItem
            // 
            this.inverseToolStripMenuItem.Name = "inverseToolStripMenuItem";
            this.inverseToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.inverseToolStripMenuItem.Tag = "Inverse";
            this.inverseToolStripMenuItem.Text = "Inverse";
            this.inverseToolStripMenuItem.Click += new System.EventHandler(this.MenuStripClick);
            // 
            // CollectionListingView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "CollectionListingView";
            this.Size = new System.Drawing.Size(464, 384);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListViewCollections)).EndInit();
            this.CollectionContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private BrightIdeasSoftware.FastObjectListView ListViewCollections;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private System.Windows.Forms.TextBox textBox_collectionNameSearch;
        private System.Windows.Forms.ContextMenuStrip CollectionContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem renameCollectionMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteCollectionMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mergeWithMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem CreateMenuStrip;
        private BrightIdeasSoftware.OLVColumn Total;
        private System.Windows.Forms.ToolStripMenuItem DuplicateMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem intersectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inverseToolStripMenuItem;
    }
}
