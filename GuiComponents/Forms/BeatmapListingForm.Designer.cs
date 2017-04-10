using GuiComponents.Controls;

namespace GuiComponents.Forms
{
    partial class BeatmapListingForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.beatmapListingView1 = new GuiComponents.Controls.BeatmapListingView();
            this.combinedBeatmapPreviewView1 = new GuiComponents.Controls.CombinedBeatmapPreviewView();
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
            this.splitContainer1.Panel1.Controls.Add(this.beatmapListingView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.combinedBeatmapPreviewView1);
            this.splitContainer1.Size = new System.Drawing.Size(938, 387);
            this.splitContainer1.SplitterDistance = 526;
            this.splitContainer1.TabIndex = 0;
            // 
            // beatmapListingView1
            // 
            this.beatmapListingView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.beatmapListingView1.Location = new System.Drawing.Point(0, 0);
            this.beatmapListingView1.Name = "beatmapListingView1";
            this.beatmapListingView1.ResultText = null;
            this.beatmapListingView1.Size = new System.Drawing.Size(526, 387);
            this.beatmapListingView1.TabIndex = 0;
            // 
            // combinedBeatmapPreviewView1
            // 
            this.combinedBeatmapPreviewView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.combinedBeatmapPreviewView1.Location = new System.Drawing.Point(0, 0);
            this.combinedBeatmapPreviewView1.Name = "combinedBeatmapPreviewView1";
            this.combinedBeatmapPreviewView1.Size = new System.Drawing.Size(408, 387);
            this.combinedBeatmapPreviewView1.TabIndex = 0;
            // 
            // BeatmapListingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 387);
            this.Controls.Add(this.splitContainer1);
            this.Name = "BeatmapListingForm";
            this.Text = "Collection Manager - Beatmap listing";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private BeatmapListingView beatmapListingView1;
        private CombinedBeatmapPreviewView combinedBeatmapPreviewView1;
    }
}