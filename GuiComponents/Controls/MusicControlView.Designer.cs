namespace GuiComponents.Controls
{
    partial class MusicControlView
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
            this.trackBar_Volume = new System.Windows.Forms.TrackBar();
            this.checkBox_musicPlayer = new System.Windows.Forms.CheckBox();
            this.panel_audioPlayback = new System.Windows.Forms.Panel();
            this.checkBox_DT = new System.Windows.Forms.CheckBox();
            this.trackBar_position = new System.Windows.Forms.TrackBar();
            this.button_StopPreview = new System.Windows.Forms.Button();
            this.checkBox_autoPlay = new System.Windows.Forms.CheckBox();
            this.button_StartPreview = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Volume)).BeginInit();
            this.panel_audioPlayback.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_position)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar_Volume
            // 
            this.trackBar_Volume.AutoSize = false;
            this.trackBar_Volume.Location = new System.Drawing.Point(183, 3);
            this.trackBar_Volume.Maximum = 100;
            this.trackBar_Volume.Name = "trackBar_Volume";
            this.trackBar_Volume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_Volume.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBar_Volume.Size = new System.Drawing.Size(20, 55);
            this.trackBar_Volume.TabIndex = 20;
            this.trackBar_Volume.TickFrequency = 15;
            this.trackBar_Volume.Value = 30;
            // 
            // checkBox_musicPlayer
            // 
            this.checkBox_musicPlayer.AutoSize = true;
            this.checkBox_musicPlayer.Location = new System.Drawing.Point(52, 25);
            this.checkBox_musicPlayer.Name = "checkBox_musicPlayer";
            this.checkBox_musicPlayer.Size = new System.Drawing.Size(64, 17);
            this.checkBox_musicPlayer.TabIndex = 21;
            this.checkBox_musicPlayer.Text = "♫ mode";
            this.checkBox_musicPlayer.UseVisualStyleBackColor = true;
            // 
            // panel_audioPlayback
            // 
            this.panel_audioPlayback.Controls.Add(this.checkBox_DT);
            this.panel_audioPlayback.Controls.Add(this.trackBar_position);
            this.panel_audioPlayback.Controls.Add(this.checkBox_musicPlayer);
            this.panel_audioPlayback.Controls.Add(this.button_StopPreview);
            this.panel_audioPlayback.Controls.Add(this.checkBox_autoPlay);
            this.panel_audioPlayback.Controls.Add(this.button_StartPreview);
            this.panel_audioPlayback.Location = new System.Drawing.Point(0, 0);
            this.panel_audioPlayback.Name = "panel_audioPlayback";
            this.panel_audioPlayback.Size = new System.Drawing.Size(169, 60);
            this.panel_audioPlayback.TabIndex = 19;
            // 
            // checkBox_DT
            // 
            this.checkBox_DT.AutoSize = true;
            this.checkBox_DT.Location = new System.Drawing.Point(122, 25);
            this.checkBox_DT.Name = "checkBox_DT";
            this.checkBox_DT.Size = new System.Drawing.Size(41, 17);
            this.checkBox_DT.TabIndex = 23;
            this.checkBox_DT.Text = "DT";
            this.checkBox_DT.UseVisualStyleBackColor = true;
            // 
            // trackBar_position
            // 
            this.trackBar_position.AutoSize = false;
            this.trackBar_position.Location = new System.Drawing.Point(4, 4);
            this.trackBar_position.Maximum = 100;
            this.trackBar_position.Name = "trackBar_position";
            this.trackBar_position.Size = new System.Drawing.Size(162, 20);
            this.trackBar_position.TabIndex = 11;
            this.trackBar_position.TickFrequency = 5;
            // 
            // button_StopPreview
            // 
            this.button_StopPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_StopPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_StopPreview.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_StopPreview.Location = new System.Drawing.Point(25, 34);
            this.button_StopPreview.Name = "button_StopPreview";
            this.button_StopPreview.Size = new System.Drawing.Size(23, 23);
            this.button_StopPreview.TabIndex = 9;
            this.button_StopPreview.Text = "⏸";
            this.button_StopPreview.UseVisualStyleBackColor = true;
            // 
            // checkBox_autoPlay
            // 
            this.checkBox_autoPlay.AutoSize = true;
            this.checkBox_autoPlay.Location = new System.Drawing.Point(52, 41);
            this.checkBox_autoPlay.Name = "checkBox_autoPlay";
            this.checkBox_autoPlay.Size = new System.Drawing.Size(67, 17);
            this.checkBox_autoPlay.TabIndex = 22;
            this.checkBox_autoPlay.Text = "Autoplay";
            this.checkBox_autoPlay.UseVisualStyleBackColor = true;
            // 
            // button_StartPreview
            // 
            this.button_StartPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_StartPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_StartPreview.Location = new System.Drawing.Point(0, 34);
            this.button_StartPreview.Name = "button_StartPreview";
            this.button_StartPreview.Size = new System.Drawing.Size(23, 23);
            this.button_StartPreview.TabIndex = 10;
            this.button_StartPreview.Text = "▶";
            this.button_StartPreview.UseVisualStyleBackColor = true;
            // 
            // MusicControlView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.trackBar_Volume);
            this.Controls.Add(this.panel_audioPlayback);
            this.Name = "MusicControlView";
            this.Size = new System.Drawing.Size(214, 60);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Volume)).EndInit();
            this.panel_audioPlayback.ResumeLayout(false);
            this.panel_audioPlayback.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_position)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar_Volume;
        private System.Windows.Forms.CheckBox checkBox_musicPlayer;
        private System.Windows.Forms.Panel panel_audioPlayback;
        private System.Windows.Forms.TrackBar trackBar_position;
        private System.Windows.Forms.Button button_StopPreview;
        private System.Windows.Forms.Button button_StartPreview;
        private System.Windows.Forms.CheckBox checkBox_DT;
        private System.Windows.Forms.CheckBox checkBox_autoPlay;
    }
}
