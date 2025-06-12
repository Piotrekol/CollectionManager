namespace GuiComponents.Controls
{
    partial class CombinedListingView
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
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            collectionListingView1 = new CollectionListingView();
            beatmapListingView1 = new BeatmapListingView();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(collectionListingView1);
            splitContainer1.Panel1MinSize = 230;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(beatmapListingView1);
            splitContainer1.Size = new System.Drawing.Size(1297, 468);
            splitContainer1.SplitterDistance = 264;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 0;
            // 
            // collectionListingView1
            // 
            collectionListingView1.Dock = System.Windows.Forms.DockStyle.Fill;
            collectionListingView1.HighlightedCollections = null;
            collectionListingView1.Location = new System.Drawing.Point(0, 0);
            collectionListingView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            collectionListingView1.Name = "collectionListingView1";
            collectionListingView1.SelectedCollection = null;
            collectionListingView1.Size = new System.Drawing.Size(264, 468);
            collectionListingView1.TabIndex = 0;
            // 
            // beatmapListingView1
            // 
            beatmapListingView1.AllowForDeletion = true;
            beatmapListingView1.Dock = System.Windows.Forms.DockStyle.Fill;
            beatmapListingView1.Location = new System.Drawing.Point(0, 0);
            beatmapListingView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            beatmapListingView1.Name = "beatmapListingView1";
            beatmapListingView1.ResultText = null;
            beatmapListingView1.Size = new System.Drawing.Size(1028, 468);
            beatmapListingView1.TabIndex = 0;
            // 
            // CombinedListingView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CombinedListingView";
            Size = new System.Drawing.Size(1297, 468);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private CollectionListingView collectionListingView1;
        private BeatmapListingView beatmapListingView1;
    }
}
