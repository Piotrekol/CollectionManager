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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.collectionListingView1 = new GuiComponents.Controls.CollectionListingView();
            this.beatmapListingView1 = new GuiComponents.Controls.BeatmapListingView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.collectionListingView1);
            this.splitContainer1.Panel1MinSize = 230;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.beatmapListingView1);
            this.splitContainer1.Size = new System.Drawing.Size(1112, 406);
            this.splitContainer1.SplitterDistance = 230;
            this.splitContainer1.TabIndex = 0;
            // 
            // collectionListingView1
            // 
            this.collectionListingView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collectionListingView1.Location = new System.Drawing.Point(0, 0);
            this.collectionListingView1.Name = "collectionListingView1";
            this.collectionListingView1.SelectedCollection = null;
            this.collectionListingView1.Size = new System.Drawing.Size(230, 406);
            this.collectionListingView1.TabIndex = 0;
            // 
            // beatmapListingView1
            // 
            this.beatmapListingView1.AllowForDeletion = true;
            this.beatmapListingView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.beatmapListingView1.Location = new System.Drawing.Point(0, 0);
            this.beatmapListingView1.Name = "beatmapListingView1";
            this.beatmapListingView1.ResultText = null;
            this.beatmapListingView1.Size = new System.Drawing.Size(878, 406);
            this.beatmapListingView1.TabIndex = 0;
            // 
            // CombinedListingView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CombinedListingView";
            this.Size = new System.Drawing.Size(1112, 406);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private CollectionListingView collectionListingView1;
        private BeatmapListingView beatmapListingView1;
    }
}
