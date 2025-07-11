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
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            beatmapListingView1 = new BeatmapListingView();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            combinedBeatmapPreviewView1 = new CombinedBeatmapPreviewView();
            scoresListingView1 = new CollectionManager.WinForms.Controls.ScoresListingView();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
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
            splitContainer1.Panel1.Controls.Add(beatmapListingView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new System.Drawing.Size(1284, 461);
            splitContainer1.SplitterDistance = 718;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 0;
            // 
            // beatmapListingView1
            // 
            beatmapListingView1.AllowForDeletion = false;
            beatmapListingView1.Dock = System.Windows.Forms.DockStyle.Fill;
            beatmapListingView1.Location = new System.Drawing.Point(0, 0);
            beatmapListingView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            beatmapListingView1.Name = "beatmapListingView1";
            beatmapListingView1.ResultText = null;
            beatmapListingView1.Size = new System.Drawing.Size(718, 461);
            beatmapListingView1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(combinedBeatmapPreviewView1);
            splitContainer2.Panel1MinSize = 5;
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(scoresListingView1);
            splitContainer2.Panel2MinSize = 5;
            splitContainer2.Size = new System.Drawing.Size(561, 461);
            splitContainer2.SplitterDistance = 312;
            splitContainer2.SplitterWidth = 5;
            splitContainer2.TabIndex = 1;
            // 
            // combinedBeatmapPreviewView1
            // 
            combinedBeatmapPreviewView1.BackColor = System.Drawing.SystemColors.Control;
            combinedBeatmapPreviewView1.Dock = System.Windows.Forms.DockStyle.Fill;
            combinedBeatmapPreviewView1.Location = new System.Drawing.Point(0, 0);
            combinedBeatmapPreviewView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            combinedBeatmapPreviewView1.Name = "combinedBeatmapPreviewView1";
            combinedBeatmapPreviewView1.Size = new System.Drawing.Size(561, 312);
            combinedBeatmapPreviewView1.TabIndex = 0;
            // 
            // scoresListingView1
            // 
            scoresListingView1.Dock = System.Windows.Forms.DockStyle.Fill;
            scoresListingView1.Location = new System.Drawing.Point(0, 0);
            scoresListingView1.ModParser = null;
            scoresListingView1.Name = "scoresListingView1";
            scoresListingView1.Scores = null;
            scoresListingView1.Size = new System.Drawing.Size(561, 144);
            scoresListingView1.TabIndex = 0;
            // 
            // BeatmapListingForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1284, 461);
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "BeatmapListingForm";
            Text = "Collection Manager - Beatmap listing";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private BeatmapListingView beatmapListingView1;
        private CombinedBeatmapPreviewView combinedBeatmapPreviewView1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private CollectionManager.WinForms.Controls.ScoresListingView scoresListingView1;
    }
}