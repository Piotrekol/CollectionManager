namespace GuiComponents.Controls
{
    partial class InfoTextView
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
            this.label_UpdateText = new System.Windows.Forms.Label();
            this.label_beatmapsMissing = new System.Windows.Forms.Label();
            this.label_LoadedCollections = new System.Windows.Forms.Label();
            this.label_LoadedBeatmaps = new System.Windows.Forms.Label();
            this.label_BeatmapsInCollections = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_UpdateText
            // 
            this.label_UpdateText.AutoSize = true;
            this.label_UpdateText.Location = new System.Drawing.Point(1, 0);
            this.label_UpdateText.Name = "label_UpdateText";
            this.label_UpdateText.Size = new System.Drawing.Size(75, 13);
            this.label_UpdateText.TabIndex = 13;
            this.label_UpdateText.Text = "<UpdateText>";
            // 
            // label_beatmapsMissing
            // 
            this.label_beatmapsMissing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_beatmapsMissing.AutoSize = true;
            this.label_beatmapsMissing.Location = new System.Drawing.Point(884, 0);
            this.label_beatmapsMissing.Name = "label_beatmapsMissing";
            this.label_beatmapsMissing.Size = new System.Drawing.Size(101, 13);
            this.label_beatmapsMissing.TabIndex = 12;
            this.label_beatmapsMissing.Text = "<BeatmapsMissing>";
            // 
            // label_LoadedCollections
            // 
            this.label_LoadedCollections.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_LoadedCollections.AutoSize = true;
            this.label_LoadedCollections.Location = new System.Drawing.Point(590, 0);
            this.label_LoadedCollections.Name = "label_LoadedCollections";
            this.label_LoadedCollections.Size = new System.Drawing.Size(106, 13);
            this.label_LoadedCollections.TabIndex = 11;
            this.label_LoadedCollections.Text = "<CollectionsLoaded>";
            // 
            // label_LoadedBeatmaps
            // 
            this.label_LoadedBeatmaps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_LoadedBeatmaps.AutoSize = true;
            this.label_LoadedBeatmaps.Location = new System.Drawing.Point(433, 0);
            this.label_LoadedBeatmaps.Name = "label_LoadedBeatmaps";
            this.label_LoadedBeatmaps.Size = new System.Drawing.Size(101, 13);
            this.label_LoadedBeatmaps.TabIndex = 10;
            this.label_LoadedBeatmaps.Text = "<beatmapsLoaded>";
            // 
            // label_BeatmapsInCollections
            // 
            this.label_BeatmapsInCollections.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_BeatmapsInCollections.AutoSize = true;
            this.label_BeatmapsInCollections.Location = new System.Drawing.Point(732, 0);
            this.label_BeatmapsInCollections.Name = "label_BeatmapsInCollections";
            this.label_BeatmapsInCollections.Size = new System.Drawing.Size(126, 13);
            this.label_BeatmapsInCollections.TabIndex = 14;
            this.label_BeatmapsInCollections.Text = "<BeatmapsInCollections>";
            // 
            // InfoTextView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_BeatmapsInCollections);
            this.Controls.Add(this.label_beatmapsMissing);
            this.Controls.Add(this.label_LoadedCollections);
            this.Controls.Add(this.label_LoadedBeatmaps);
            this.Controls.Add(this.label_UpdateText);
            this.Name = "InfoTextView";
            this.Size = new System.Drawing.Size(1010, 17);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_UpdateText;
        private System.Windows.Forms.Label label_beatmapsMissing;
        private System.Windows.Forms.Label label_LoadedCollections;
        private System.Windows.Forms.Label label_LoadedBeatmaps;
        private System.Windows.Forms.Label label_BeatmapsInCollections;
    }
}
