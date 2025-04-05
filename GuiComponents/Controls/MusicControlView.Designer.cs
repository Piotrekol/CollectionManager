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
            trackBar_Volume = new System.Windows.Forms.TrackBar();
            checkBox_musicPlayer = new System.Windows.Forms.CheckBox();
            panel_audioPlayback = new System.Windows.Forms.Panel();
            checkBox_DT = new System.Windows.Forms.CheckBox();
            trackBar_position = new System.Windows.Forms.TrackBar();
            button_StopPreview = new System.Windows.Forms.Button();
            checkBox_autoPlay = new System.Windows.Forms.CheckBox();
            button_StartPreview = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)trackBar_Volume).BeginInit();
            panel_audioPlayback.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar_position).BeginInit();
            SuspendLayout();
            // 
            // trackBar_Volume
            // 
            trackBar_Volume.AutoSize = false;
            trackBar_Volume.Location = new System.Drawing.Point(214, 3);
            trackBar_Volume.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            trackBar_Volume.Maximum = 100;
            trackBar_Volume.Name = "trackBar_Volume";
            trackBar_Volume.Orientation = System.Windows.Forms.Orientation.Vertical;
            trackBar_Volume.RightToLeft = System.Windows.Forms.RightToLeft.No;
            trackBar_Volume.Size = new System.Drawing.Size(23, 63);
            trackBar_Volume.TabIndex = 20;
            trackBar_Volume.TickFrequency = 15;
            trackBar_Volume.Value = 30;
            // 
            // checkBox_musicPlayer
            // 
            checkBox_musicPlayer.AutoSize = true;
            checkBox_musicPlayer.Location = new System.Drawing.Point(61, 29);
            checkBox_musicPlayer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBox_musicPlayer.Name = "checkBox_musicPlayer";
            checkBox_musicPlayer.Size = new System.Drawing.Size(70, 19);
            checkBox_musicPlayer.TabIndex = 21;
            checkBox_musicPlayer.Text = "♫ mode";
            checkBox_musicPlayer.UseVisualStyleBackColor = true;
            // 
            // panel_audioPlayback
            // 
            panel_audioPlayback.Controls.Add(checkBox_DT);
            panel_audioPlayback.Controls.Add(trackBar_position);
            panel_audioPlayback.Controls.Add(checkBox_musicPlayer);
            panel_audioPlayback.Controls.Add(button_StopPreview);
            panel_audioPlayback.Controls.Add(checkBox_autoPlay);
            panel_audioPlayback.Controls.Add(button_StartPreview);
            panel_audioPlayback.Location = new System.Drawing.Point(0, 0);
            panel_audioPlayback.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel_audioPlayback.Name = "panel_audioPlayback";
            panel_audioPlayback.Size = new System.Drawing.Size(197, 69);
            panel_audioPlayback.TabIndex = 19;
            // 
            // checkBox_DT
            // 
            checkBox_DT.AutoSize = true;
            checkBox_DT.Location = new System.Drawing.Point(142, 29);
            checkBox_DT.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBox_DT.Name = "checkBox_DT";
            checkBox_DT.Size = new System.Drawing.Size(40, 19);
            checkBox_DT.TabIndex = 23;
            checkBox_DT.Text = "DT";
            checkBox_DT.UseVisualStyleBackColor = true;
            // 
            // trackBar_position
            // 
            trackBar_position.AutoSize = false;
            trackBar_position.Location = new System.Drawing.Point(5, 5);
            trackBar_position.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            trackBar_position.Maximum = 100;
            trackBar_position.Name = "trackBar_position";
            trackBar_position.Size = new System.Drawing.Size(189, 23);
            trackBar_position.TabIndex = 11;
            trackBar_position.TickFrequency = 5;
            // 
            // button_StopPreview
            // 
            button_StopPreview.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            button_StopPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button_StopPreview.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            button_StopPreview.Location = new System.Drawing.Point(29, 39);
            button_StopPreview.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button_StopPreview.Name = "button_StopPreview";
            button_StopPreview.Size = new System.Drawing.Size(27, 27);
            button_StopPreview.TabIndex = 9;
            button_StopPreview.Text = "⏸";
            button_StopPreview.UseVisualStyleBackColor = true;
            // 
            // checkBox_autoPlay
            // 
            checkBox_autoPlay.AutoSize = true;
            checkBox_autoPlay.Location = new System.Drawing.Point(61, 47);
            checkBox_autoPlay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBox_autoPlay.Name = "checkBox_autoPlay";
            checkBox_autoPlay.Size = new System.Drawing.Size(74, 19);
            checkBox_autoPlay.TabIndex = 22;
            checkBox_autoPlay.Text = "Autoplay";
            checkBox_autoPlay.UseVisualStyleBackColor = true;
            // 
            // button_StartPreview
            // 
            button_StartPreview.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            button_StartPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            button_StartPreview.Location = new System.Drawing.Point(0, 39);
            button_StartPreview.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button_StartPreview.Name = "button_StartPreview";
            button_StartPreview.Size = new System.Drawing.Size(27, 27);
            button_StartPreview.TabIndex = 10;
            button_StartPreview.Text = "▶";
            button_StartPreview.UseVisualStyleBackColor = true;
            // 
            // MusicControlView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(trackBar_Volume);
            Controls.Add(panel_audioPlayback);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MusicControlView";
            Size = new System.Drawing.Size(251, 69);
            ((System.ComponentModel.ISupportInitialize)trackBar_Volume).EndInit();
            panel_audioPlayback.ResumeLayout(false);
            panel_audioPlayback.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar_position).EndInit();
            ResumeLayout(false);

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
