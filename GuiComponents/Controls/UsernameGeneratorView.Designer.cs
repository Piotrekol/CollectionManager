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
            this.progressBar_usernames = new System.Windows.Forms.ProgressBar();
            this.button_Abort = new System.Windows.Forms.Button();
            this.button_Start = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel_main = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown_StartRank = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_EndRank = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_StartRank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_EndRank)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar_usernames
            // 
            this.progressBar_usernames.Location = new System.Drawing.Point(3, 116);
            this.progressBar_usernames.Name = "progressBar_usernames";
            this.progressBar_usernames.Size = new System.Drawing.Size(374, 23);
            this.progressBar_usernames.TabIndex = 29;
            // 
            // button_Abort
            // 
            this.button_Abort.Enabled = false;
            this.button_Abort.Location = new System.Drawing.Point(211, 87);
            this.button_Abort.Name = "button_Abort";
            this.button_Abort.Size = new System.Drawing.Size(166, 23);
            this.button_Abort.TabIndex = 28;
            this.button_Abort.Text = "Abort";
            this.button_Abort.UseVisualStyleBackColor = true;
            this.button_Abort.Click += new System.EventHandler(this.button_Abort_Click);
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(3, 87);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(166, 23);
            this.button_Start.TabIndex = 27;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(0, 145);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(380, 89);
            this.textBox1.TabIndex = 30;
            // 
            // panel_main
            // 
            this.panel_main.Controls.Add(this.label4);
            this.panel_main.Controls.Add(this.label3);
            this.panel_main.Controls.Add(this.numericUpDown_StartRank);
            this.panel_main.Controls.Add(this.label1);
            this.panel_main.Controls.Add(this.numericUpDown_EndRank);
            this.panel_main.Controls.Add(this.label2);
            this.panel_main.Location = new System.Drawing.Point(57, 3);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(263, 78);
            this.panel_main.TabIndex = 31;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Get usernames from player ranks";
            // 
            // numericUpDown_StartRank
            // 
            this.numericUpDown_StartRank.Location = new System.Drawing.Point(130, 28);
            this.numericUpDown_StartRank.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown_StartRank.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_StartRank.Name = "numericUpDown_StartRank";
            this.numericUpDown_StartRank.Size = new System.Drawing.Size(83, 20);
            this.numericUpDown_StartRank.TabIndex = 0;
            this.numericUpDown_StartRank.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Min";
            // 
            // numericUpDown_EndRank
            // 
            this.numericUpDown_EndRank.Location = new System.Drawing.Point(130, 54);
            this.numericUpDown_EndRank.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown_EndRank.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_EndRank.Name = "numericUpDown_EndRank";
            this.numericUpDown_EndRank.Size = new System.Drawing.Size(83, 20);
            this.numericUpDown_EndRank.TabIndex = 3;
            this.numericUpDown_EndRank.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Max";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Min";
            // 
            // UsernameGeneratorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_main);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.progressBar_usernames);
            this.Controls.Add(this.button_Abort);
            this.Controls.Add(this.button_Start);
            this.Name = "UsernameGeneratorView";
            this.Size = new System.Drawing.Size(380, 234);
            this.panel_main.ResumeLayout(false);
            this.panel_main.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_StartRank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_EndRank)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
