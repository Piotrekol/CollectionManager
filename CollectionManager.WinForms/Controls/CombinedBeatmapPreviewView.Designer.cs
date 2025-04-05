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
            beatmapThumbnailView1 = new BeatmapThumbnailView();
            musicControlView1 = new MusicControlView();
            SuspendLayout();
            // 
            // beatmapThumbnailView1
            // 
            beatmapThumbnailView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            beatmapThumbnailView1.Location = new System.Drawing.Point(2, 2);
            beatmapThumbnailView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            beatmapThumbnailView1.Name = "beatmapThumbnailView1";
            beatmapThumbnailView1.Size = new System.Drawing.Size(462, 388);
            beatmapThumbnailView1.TabIndex = 0;
            // 
            // musicControlView1
            // 
            musicControlView1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            musicControlView1.IsAutoPlayEnabled = false;
            musicControlView1.IsMusicPlayerMode = false;
            musicControlView1.IsUserSeeking = false;
            musicControlView1.Location = new System.Drawing.Point(217, 339);
            musicControlView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            musicControlView1.Name = "musicControlView1";
            musicControlView1.Position = 0;
            musicControlView1.Size = new System.Drawing.Size(250, 67);
            musicControlView1.TabIndex = 1;
            musicControlView1.Volume = 0.3F;
            // 
            // CombinedBeatmapPreviewView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(musicControlView1);
            Controls.Add(beatmapThumbnailView1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CombinedBeatmapPreviewView";
            Size = new System.Drawing.Size(467, 406);
            ResumeLayout(false);

        }

        #endregion

        private BeatmapThumbnailView beatmapThumbnailView1;
        private MusicControlView musicControlView1;
    }
}
