namespace GuiComponents.Controls
{
    partial class CombinedBeatmapPreviewView
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
            this.musicControlView1 = new GuiComponents.Controls.MusicControlView();
            this.beatmapThumbnailView1 = new GuiComponents.Controls.BeatmapThumbnailView();
            this.SuspendLayout();
            // 
            // musicControlView1
            // 
            this.musicControlView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.musicControlView1.IsAutoPlayEnabled = false;
            this.musicControlView1.IsMusicPlayerMode = false;
            this.musicControlView1.IsUserSeeking = false;
            this.musicControlView1.Location = new System.Drawing.Point(186, 294);
            this.musicControlView1.Name = "musicControlView1";
            this.musicControlView1.Position = 0;
            this.musicControlView1.Size = new System.Drawing.Size(214, 58);
            this.musicControlView1.TabIndex = 1;
            this.musicControlView1.Volume = 0.3F;
            // 
            // beatmapThumbnailView1
            // 
            this.beatmapThumbnailView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.beatmapThumbnailView1.Location = new System.Drawing.Point(2, 2);
            this.beatmapThumbnailView1.Name = "beatmapThumbnailView1";
            this.beatmapThumbnailView1.Size = new System.Drawing.Size(396, 336);
            this.beatmapThumbnailView1.TabIndex = 0;
            // 
            // CombinedBeatmapPreviewView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.musicControlView1);
            this.Controls.Add(this.beatmapThumbnailView1);
            this.Name = "CombinedBeatmapPreviewView";
            this.Size = new System.Drawing.Size(400, 352);
            this.ResumeLayout(false);

        }

        #endregion

        private BeatmapThumbnailView beatmapThumbnailView1;
        private MusicControlView musicControlView1;
    }
}
