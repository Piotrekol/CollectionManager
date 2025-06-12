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
            components = new System.ComponentModel.Container();
            panel1 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            ListViewCollections = new BrightIdeasSoftware.FastObjectListView();
            olvColumn1 = new BrightIdeasSoftware.OLVColumn();
            Total = new BrightIdeasSoftware.OLVColumn();
            olvColumn2 = new BrightIdeasSoftware.OLVColumn();
            column_id = new BrightIdeasSoftware.OLVColumn();
            textBox_collectionNameSearch = new System.Windows.Forms.TextBox();
            CreateMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            renameCollectionMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            deleteCollectionMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DuplicateMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            mergeWithMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            intersectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            differenceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            inverseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CollectionContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            exportBeatmapSetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ListViewCollections).BeginInit();
            CollectionContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Controls.Add(ListViewCollections);
            panel1.Controls.Add(textBox_collectionNameSearch);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(541, 443);
            panel1.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(-2, 0);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(170, 15);
            label1.TabIndex = 2;
            label1.Text = "Search using collection names:";
            // 
            // ListViewCollections
            // 
            ListViewCollections.AllColumns.Add(olvColumn1);
            ListViewCollections.AllColumns.Add(Total);
            ListViewCollections.AllColumns.Add(olvColumn2);
            ListViewCollections.AllColumns.Add(column_id);
            ListViewCollections.AllowDrop = true;
            ListViewCollections.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ListViewCollections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { olvColumn1, Total, olvColumn2 });
            ListViewCollections.EmptyListMsg = "No collections loaded";
            ListViewCollections.Location = new System.Drawing.Point(1, 45);
            ListViewCollections.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ListViewCollections.Name = "ListViewCollections";
            ListViewCollections.ShowGroups = false;
            ListViewCollections.Size = new System.Drawing.Size(540, 397);
            ListViewCollections.Sorting = System.Windows.Forms.SortOrder.Ascending;
            ListViewCollections.TabIndex = 5;
            ListViewCollections.UnfocusedHighlightBackgroundColor = System.Drawing.Color.FromArgb(192, 255, 192);
            ListViewCollections.UseCompatibleStateImageBehavior = false;
            ListViewCollections.UseCustomSelectionColors = true;
            ListViewCollections.View = System.Windows.Forms.View.Details;
            ListViewCollections.VirtualMode = true;
            ListViewCollections.KeyDown += ListViewCollections_KeyDown;
            ListViewCollections.KeyUp += ListViewCollections_KeyUp;
            // 
            // olvColumn1
            // 
            olvColumn1.AspectName = "Name";
            olvColumn1.MaximumWidth = 400;
            olvColumn1.MinimumWidth = 20;
            olvColumn1.Text = "Name";
            olvColumn1.Width = 100;
            // 
            // Total
            // 
            Total.AspectName = "NumberOfBeatmaps";
            Total.Text = "Count";
            // 
            // olvColumn2
            // 
            olvColumn2.AspectName = "NumberOfMissingBeatmaps";
            olvColumn2.Text = "Missing";
            // 
            // column_id
            // 
            column_id.AspectName = "Id";
            column_id.DisplayIndex = 2;
            column_id.IsEditable = false;
            column_id.IsVisible = false;
            column_id.Text = "Id";
            column_id.Width = 40;
            // 
            // textBox_collectionNameSearch
            // 
            textBox_collectionNameSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox_collectionNameSearch.Location = new System.Drawing.Point(1, 18);
            textBox_collectionNameSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox_collectionNameSearch.Name = "textBox_collectionNameSearch";
            textBox_collectionNameSearch.Size = new System.Drawing.Size(540, 23);
            textBox_collectionNameSearch.TabIndex = 1;
            // 
            // CreateMenuStrip
            // 
            CreateMenuStrip.Name = "CreateMenuStrip";
            CreateMenuStrip.Size = new System.Drawing.Size(180, 22);
            CreateMenuStrip.Tag = "Create";
            CreateMenuStrip.Text = "Create";
            CreateMenuStrip.ToolTipText = "Create new collection";
            CreateMenuStrip.Click += MenuStripClick;
            // 
            // renameCollectionMenuStrip
            // 
            renameCollectionMenuStrip.Name = "renameCollectionMenuStrip";
            renameCollectionMenuStrip.ShortcutKeyDisplayString = "F2";
            renameCollectionMenuStrip.Size = new System.Drawing.Size(180, 22);
            renameCollectionMenuStrip.Tag = "Rename";
            renameCollectionMenuStrip.Text = "Rename";
            renameCollectionMenuStrip.ToolTipText = "Rename currently selected collection";
            renameCollectionMenuStrip.Click += MenuStripClick;
            // 
            // deleteCollectionMenuStrip
            // 
            deleteCollectionMenuStrip.Name = "deleteCollectionMenuStrip";
            deleteCollectionMenuStrip.ShortcutKeyDisplayString = "Del";
            deleteCollectionMenuStrip.Size = new System.Drawing.Size(180, 22);
            deleteCollectionMenuStrip.Tag = "Delete";
            deleteCollectionMenuStrip.Text = "Delete";
            deleteCollectionMenuStrip.ToolTipText = "Delete currently selected collections";
            deleteCollectionMenuStrip.Click += MenuStripClick;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
            copyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            copyToolStripMenuItem.Tag = "Copy";
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += MenuStripClick;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+V";
            pasteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            pasteToolStripMenuItem.Tag = "Paste";
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += MenuStripClick;
            // 
            // DuplicateMenuStrip
            // 
            DuplicateMenuStrip.Name = "DuplicateMenuStrip";
            DuplicateMenuStrip.Size = new System.Drawing.Size(180, 22);
            DuplicateMenuStrip.Tag = "Duplicate";
            DuplicateMenuStrip.Text = "Duplicate";
            DuplicateMenuStrip.ToolTipText = "Create a copy of currently selected collection";
            DuplicateMenuStrip.Click += MenuStripClick;
            // 
            // mergeWithMenuStrip
            // 
            mergeWithMenuStrip.Name = "mergeWithMenuStrip";
            mergeWithMenuStrip.Size = new System.Drawing.Size(180, 22);
            mergeWithMenuStrip.Tag = "Merge";
            mergeWithMenuStrip.Text = "Merge selected";
            mergeWithMenuStrip.ToolTipText = "Merge beatmaps from selected collections into new collection";
            mergeWithMenuStrip.Click += MenuStripClick;
            // 
            // intersectMenuItem
            // 
            intersectMenuItem.Name = "intersectMenuItem";
            intersectMenuItem.Size = new System.Drawing.Size(180, 22);
            intersectMenuItem.Tag = "Intersect";
            intersectMenuItem.Text = "Intersection";
            intersectMenuItem.ToolTipText = "Create a collection that contains beatmaps that exist in all selected collections";
            intersectMenuItem.Click += MenuStripClick;
            // 
            // differenceMenuItem
            // 
            differenceMenuItem.Name = "differenceMenuItem";
            differenceMenuItem.Size = new System.Drawing.Size(180, 22);
            differenceMenuItem.Tag = "Difference";
            differenceMenuItem.Text = "Difference";
            differenceMenuItem.ToolTipText = "Create a collection that contains beatmaps that exist in only one of the selected collections";
            differenceMenuItem.Click += MenuStripClick;
            // 
            // inverseToolStripMenuItem
            // 
            inverseToolStripMenuItem.Name = "inverseToolStripMenuItem";
            inverseToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            inverseToolStripMenuItem.Tag = "Inverse";
            inverseToolStripMenuItem.Text = "Inverse";
            inverseToolStripMenuItem.ToolTipText = "Create new collection that contains every beatmap not present in all selected collections";
            inverseToolStripMenuItem.Click += MenuStripClick;
            // 
            // CollectionContextMenuStrip
            // 
            CollectionContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { CreateMenuStrip, renameCollectionMenuStrip, deleteCollectionMenuStrip, copyToolStripMenuItem, pasteToolStripMenuItem, DuplicateMenuStrip, mergeWithMenuStrip, intersectMenuItem, differenceMenuItem, inverseToolStripMenuItem, exportBeatmapSetsToolStripMenuItem });
            CollectionContextMenuStrip.Name = "CollectionContextMenuStrip";
            CollectionContextMenuStrip.Size = new System.Drawing.Size(181, 246);
            // 
            // exportBeatmapSetsToolStripMenuItem
            // 
            exportBeatmapSetsToolStripMenuItem.Name = "exportBeatmapSetsToolStripMenuItem";
            exportBeatmapSetsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            exportBeatmapSetsToolStripMenuItem.Tag = "Export";
            exportBeatmapSetsToolStripMenuItem.Text = "Export beatmap sets";
            exportBeatmapSetsToolStripMenuItem.Click += MenuStripClick;
            // 
            // CollectionListingView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CollectionListingView";
            Size = new System.Drawing.Size(541, 443);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ListViewCollections).EndInit();
            CollectionContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private BrightIdeasSoftware.FastObjectListView ListViewCollections;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private System.Windows.Forms.TextBox textBox_collectionNameSearch;
        private BrightIdeasSoftware.OLVColumn Total;
        private System.Windows.Forms.ToolStripMenuItem CreateMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem renameCollectionMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteCollectionMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DuplicateMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mergeWithMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem intersectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem differenceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inverseToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip CollectionContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exportBeatmapSetsToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn column_id;
    }
}
