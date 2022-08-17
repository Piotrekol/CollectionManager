namespace GuiComponents.Controls
{
    partial class BeatmapThumbnailView
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.richTextBox_AdditionalStats = new System.Windows.Forms.RichTextBox();
            this.label_BasicStats = new System.Windows.Forms.Label();
            this.label_BeatmapName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(3, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(281, 142);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.richTextBox_AdditionalStats);
            this.panel1.Controls.Add(this.label_BasicStats);
            this.panel1.Controls.Add(this.label_BeatmapName);
            this.panel1.Location = new System.Drawing.Point(3, 141);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(281, 107);
            this.panel1.TabIndex = 14;
            // 
            // richTextBox_AdditionalStats
            // 
            this.richTextBox_AdditionalStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_AdditionalStats.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox_AdditionalStats.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox_AdditionalStats.ForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBox_AdditionalStats.Location = new System.Drawing.Point(3, 1);
            this.richTextBox_AdditionalStats.Name = "richTextBox_AdditionalStats";
            this.richTextBox_AdditionalStats.Size = new System.Drawing.Size(278, 30);
            this.richTextBox_AdditionalStats.TabIndex = 12;
            this.richTextBox_AdditionalStats.Text = "-";
            // 
            // label_BasicStats
            // 
            this.label_BasicStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_BasicStats.Location = new System.Drawing.Point(3, 58);
            this.label_BasicStats.Name = "label_BasicStats";
            this.label_BasicStats.Size = new System.Drawing.Size(100, 30);
            this.label_BasicStats.TabIndex = 11;
            this.label_BasicStats.Text = "-";
            // 
            // label_BeatmapName
            // 
            this.label_BeatmapName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_BeatmapName.AutoSize = true;
            this.label_BeatmapName.Location = new System.Drawing.Point(3, 38);
            this.label_BeatmapName.Name = "label_BeatmapName";
            this.label_BeatmapName.Size = new System.Drawing.Size(100, 13);
            this.label_BeatmapName.TabIndex = 9;
            this.label_BeatmapName.Text = "                               ";
            // 
            // BeatmapThumbnailView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Name = "BeatmapThumbnailView";
            this.Size = new System.Drawing.Size(284, 248);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_BeatmapName;
        private System.Windows.Forms.Label label_BasicStats;
        private System.Windows.Forms.RichTextBox richTextBox_AdditionalStats;
    }
}
