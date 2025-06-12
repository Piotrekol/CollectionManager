namespace GuiComponents.Controls
{
    partial class UsernameGeneratorView
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
            progressBar_usernames = new System.Windows.Forms.ProgressBar();
            button_Abort = new System.Windows.Forms.Button();
            button_Start = new System.Windows.Forms.Button();
            textBox1 = new System.Windows.Forms.TextBox();
            panel_main = new System.Windows.Forms.Panel();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            numericUpDown_StartRank = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            numericUpDown_EndRank = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            panel_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_StartRank).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_EndRank).BeginInit();
            SuspendLayout();
            // 
            // progressBar_usernames
            // 
            progressBar_usernames.Location = new System.Drawing.Point(4, 134);
            progressBar_usernames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            progressBar_usernames.Name = "progressBar_usernames";
            progressBar_usernames.Size = new System.Drawing.Size(436, 27);
            progressBar_usernames.TabIndex = 29;
            // 
            // button_Abort
            // 
            button_Abort.Enabled = false;
            button_Abort.Location = new System.Drawing.Point(246, 100);
            button_Abort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button_Abort.Name = "button_Abort";
            button_Abort.Size = new System.Drawing.Size(194, 27);
            button_Abort.TabIndex = 28;
            button_Abort.Text = "Abort";
            button_Abort.UseVisualStyleBackColor = true;
            button_Abort.Click += button_Abort_Click;
            // 
            // button_Start
            // 
            button_Start.Location = new System.Drawing.Point(4, 100);
            button_Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button_Start.Name = "button_Start";
            button_Start.Size = new System.Drawing.Size(194, 27);
            button_Start.TabIndex = 27;
            button_Start.Text = "Start";
            button_Start.UseVisualStyleBackColor = true;
            button_Start.Click += button_Start_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox1.Location = new System.Drawing.Point(0, 167);
            textBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(443, 102);
            textBox1.TabIndex = 30;
            // 
            // panel_main
            // 
            panel_main.Controls.Add(label4);
            panel_main.Controls.Add(label3);
            panel_main.Controls.Add(numericUpDown_StartRank);
            panel_main.Controls.Add(label1);
            panel_main.Controls.Add(numericUpDown_EndRank);
            panel_main.Controls.Add(label2);
            panel_main.Location = new System.Drawing.Point(66, 3);
            panel_main.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel_main.Name = "panel_main";
            panel_main.Size = new System.Drawing.Size(307, 90);
            panel_main.TabIndex = 31;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(46, 40);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(28, 15);
            label4.TabIndex = 6;
            label4.Text = "Min";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(66, 3);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(180, 15);
            label3.TabIndex = 5;
            label3.Text = "Get usernames from player ranks";
            // 
            // numericUpDown_StartRank
            // 
            numericUpDown_StartRank.Location = new System.Drawing.Point(152, 32);
            numericUpDown_StartRank.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numericUpDown_StartRank.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numericUpDown_StartRank.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown_StartRank.Name = "numericUpDown_StartRank";
            numericUpDown_StartRank.Size = new System.Drawing.Size(97, 23);
            numericUpDown_StartRank.TabIndex = 0;
            numericUpDown_StartRank.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(80, 35);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(28, 15);
            label1.TabIndex = 2;
            label1.Text = "Min";
            // 
            // numericUpDown_EndRank
            // 
            numericUpDown_EndRank.Location = new System.Drawing.Point(152, 62);
            numericUpDown_EndRank.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numericUpDown_EndRank.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown_EndRank.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown_EndRank.Name = "numericUpDown_EndRank";
            numericUpDown_EndRank.Size = new System.Drawing.Size(97, 23);
            numericUpDown_EndRank.TabIndex = 3;
            numericUpDown_EndRank.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(80, 65);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(29, 15);
            label2.TabIndex = 4;
            label2.Text = "Max";
            // 
            // UsernameGeneratorView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel_main);
            Controls.Add(textBox1);
            Controls.Add(progressBar_usernames);
            Controls.Add(button_Abort);
            Controls.Add(button_Start);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "UsernameGeneratorView";
            Size = new System.Drawing.Size(443, 270);
            panel_main.ResumeLayout(false);
            panel_main.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_StartRank).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_EndRank).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar_usernames;
        public System.Windows.Forms.Button button_Abort;
        public System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel_main;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown_StartRank;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_EndRank;
        private System.Windows.Forms.Label label2;
    }
}
