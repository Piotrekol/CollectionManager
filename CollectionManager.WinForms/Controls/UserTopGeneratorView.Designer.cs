namespace GuiComponents.Controls
{
    partial class UserTopGeneratorView
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
            groupBox_configuration = new System.Windows.Forms.GroupBox();
            label11 = new System.Windows.Forms.Label();
            comboBox_gamemode = new System.Windows.Forms.ComboBox();
            button_GenerateUsernames = new System.Windows.Forms.Button();
            label6 = new System.Windows.Forms.Label();
            textBox_apiKey = new System.Windows.Forms.TextBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            label12 = new System.Windows.Forms.Label();
            textBox_allowedMods = new System.Windows.Forms.TextBox();
            label10 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            numericUpDown_maximumPP = new System.Windows.Forms.NumericUpDown();
            rb_scores_Adown = new System.Windows.Forms.RadioButton();
            rb_scores_Sup = new System.Windows.Forms.RadioButton();
            rb_scores_all = new System.Windows.Forms.RadioButton();
            numericUpDown_accMax = new System.Windows.Forms.NumericUpDown();
            numericUpDown_accMin = new System.Windows.Forms.NumericUpDown();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            numericUpDown_minimumPP = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            label_collectionNameExample = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            textBox_collectionNameFormat = new System.Windows.Forms.TextBox();
            textBox_usernames = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label_processingStatus = new System.Windows.Forms.Label();
            progressBar_usernames = new System.Windows.Forms.ProgressBar();
            button_Abort = new System.Windows.Forms.Button();
            button_Start = new System.Windows.Forms.Button();
            collectionListingView1 = new CollectionListingView();
            panel1 = new System.Windows.Forms.Panel();
            groupBox_configuration.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_maximumPP).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_accMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_accMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_minimumPP).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox_configuration
            // 
            groupBox_configuration.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox_configuration.Controls.Add(label11);
            groupBox_configuration.Controls.Add(comboBox_gamemode);
            groupBox_configuration.Controls.Add(button_GenerateUsernames);
            groupBox_configuration.Controls.Add(label6);
            groupBox_configuration.Controls.Add(textBox_apiKey);
            groupBox_configuration.Controls.Add(groupBox3);
            groupBox_configuration.Controls.Add(label2);
            groupBox_configuration.Controls.Add(label_collectionNameExample);
            groupBox_configuration.Controls.Add(label7);
            groupBox_configuration.Controls.Add(textBox_collectionNameFormat);
            groupBox_configuration.Controls.Add(textBox_usernames);
            groupBox_configuration.Controls.Add(label1);
            groupBox_configuration.Location = new System.Drawing.Point(4, 3);
            groupBox_configuration.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox_configuration.Name = "groupBox_configuration";
            groupBox_configuration.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox_configuration.Size = new System.Drawing.Size(609, 429);
            groupBox_configuration.TabIndex = 0;
            groupBox_configuration.TabStop = false;
            groupBox_configuration.Text = "Configuration";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(349, 377);
            label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(72, 15);
            label11.TabIndex = 30;
            label11.Text = "Gamemode:";
            // 
            // comboBox_gamemode
            // 
            comboBox_gamemode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox_gamemode.FormattingEnabled = true;
            comboBox_gamemode.Items.AddRange(new object[] { "osu", "taiko", "ctb", "mania" });
            comboBox_gamemode.Location = new System.Drawing.Point(352, 397);
            comboBox_gamemode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBox_gamemode.Name = "comboBox_gamemode";
            comboBox_gamemode.Size = new System.Drawing.Size(140, 23);
            comboBox_gamemode.TabIndex = 29;
            // 
            // button_GenerateUsernames
            // 
            button_GenerateUsernames.Location = new System.Drawing.Point(461, 110);
            button_GenerateUsernames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button_GenerateUsernames.Name = "button_GenerateUsernames";
            button_GenerateUsernames.Size = new System.Drawing.Size(140, 27);
            button_GenerateUsernames.TabIndex = 28;
            button_GenerateUsernames.Text = "Generate usernames";
            button_GenerateUsernames.UseVisualStyleBackColor = true;
            button_GenerateUsernames.Click += button_GenerateUsernames_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(19, 378);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(96, 15);
            label6.TabIndex = 25;
            label6.Text = "Your osu!api key:";
            // 
            // textBox_apiKey
            // 
            textBox_apiKey.Location = new System.Drawing.Point(22, 397);
            textBox_apiKey.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox_apiKey.Name = "textBox_apiKey";
            textBox_apiKey.PasswordChar = '*';
            textBox_apiKey.Size = new System.Drawing.Size(269, 23);
            textBox_apiKey.TabIndex = 22;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label12);
            groupBox3.Controls.Add(textBox_allowedMods);
            groupBox3.Controls.Add(label10);
            groupBox3.Controls.Add(label9);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(numericUpDown_maximumPP);
            groupBox3.Controls.Add(rb_scores_Adown);
            groupBox3.Controls.Add(rb_scores_Sup);
            groupBox3.Controls.Add(rb_scores_all);
            groupBox3.Controls.Add(numericUpDown_accMax);
            groupBox3.Controls.Add(numericUpDown_accMin);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(numericUpDown_minimumPP);
            groupBox3.Location = new System.Drawing.Point(22, 188);
            groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Size = new System.Drawing.Size(579, 149);
            groupBox3.TabIndex = 21;
            groupBox3.TabStop = false;
            groupBox3.Text = "Conditions";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(7, 113);
            label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(210, 15);
            label12.TabIndex = 31;
            label12.Text = "comma separated list of mods to save:";
            // 
            // textBox_allowedMods
            // 
            textBox_allowedMods.Location = new System.Drawing.Point(227, 110);
            textBox_allowedMods.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox_allowedMods.Name = "textBox_allowedMods";
            textBox_allowedMods.Size = new System.Drawing.Size(265, 23);
            textBox_allowedMods.TabIndex = 31;
            textBox_allowedMods.Text = "All";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(176, 80);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(32, 15);
            label10.TabIndex = 25;
            label10.Text = "max:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(50, 80);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(31, 15);
            label9.TabIndex = 24;
            label9.Text = "min:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(176, 52);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(32, 15);
            label8.TabIndex = 23;
            label8.Text = "max:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(50, 52);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(31, 15);
            label3.TabIndex = 22;
            label3.Text = "min:";
            // 
            // numericUpDown_maximumPP
            // 
            numericUpDown_maximumPP.DecimalPlaces = 2;
            numericUpDown_maximumPP.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numericUpDown_maximumPP.Location = new System.Drawing.Point(217, 50);
            numericUpDown_maximumPP.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numericUpDown_maximumPP.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            numericUpDown_maximumPP.Name = "numericUpDown_maximumPP";
            numericUpDown_maximumPP.Size = new System.Drawing.Size(82, 23);
            numericUpDown_maximumPP.TabIndex = 21;
            numericUpDown_maximumPP.Value = new decimal(new int[] { 2000, 0, 0, 0 });
            // 
            // rb_scores_Adown
            // 
            rb_scores_Adown.AutoSize = true;
            rb_scores_Adown.Location = new System.Drawing.Point(227, 23);
            rb_scores_Adown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rb_scores_Adown.Name = "rb_scores_Adown";
            rb_scores_Adown.Size = new System.Drawing.Size(118, 19);
            rb_scores_Adown.TabIndex = 17;
            rb_scores_Adown.Tag = "2";
            rb_scores_Adown.Text = "Only A and worse";
            rb_scores_Adown.UseVisualStyleBackColor = true;
            rb_scores_Adown.CheckedChanged += RadioButton_AllowScores_CheckedChanged;
            // 
            // rb_scores_Sup
            // 
            rb_scores_Sup.AutoSize = true;
            rb_scores_Sup.Location = new System.Drawing.Point(99, 22);
            rb_scores_Sup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rb_scores_Sup.Name = "rb_scores_Sup";
            rb_scores_Sup.Size = new System.Drawing.Size(116, 19);
            rb_scores_Sup.TabIndex = 16;
            rb_scores_Sup.Tag = "1";
            rb_scores_Sup.Text = "Only S and better";
            rb_scores_Sup.UseVisualStyleBackColor = true;
            rb_scores_Sup.CheckedChanged += RadioButton_AllowScores_CheckedChanged;
            // 
            // rb_scores_all
            // 
            rb_scores_all.AutoSize = true;
            rb_scores_all.Checked = true;
            rb_scores_all.Location = new System.Drawing.Point(10, 22);
            rb_scores_all.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rb_scores_all.Name = "rb_scores_all";
            rb_scores_all.Size = new System.Drawing.Size(75, 19);
            rb_scores_all.TabIndex = 15;
            rb_scores_all.TabStop = true;
            rb_scores_all.Tag = "0";
            rb_scores_all.Text = "All scores";
            rb_scores_all.UseVisualStyleBackColor = true;
            rb_scores_all.CheckedChanged += RadioButton_AllowScores_CheckedChanged;
            // 
            // numericUpDown_accMax
            // 
            numericUpDown_accMax.DecimalPlaces = 2;
            numericUpDown_accMax.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numericUpDown_accMax.Location = new System.Drawing.Point(217, 80);
            numericUpDown_accMax.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numericUpDown_accMax.Name = "numericUpDown_accMax";
            numericUpDown_accMax.Size = new System.Drawing.Size(82, 23);
            numericUpDown_accMax.TabIndex = 13;
            numericUpDown_accMax.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // numericUpDown_accMin
            // 
            numericUpDown_accMin.DecimalPlaces = 2;
            numericUpDown_accMin.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numericUpDown_accMin.Location = new System.Drawing.Point(88, 77);
            numericUpDown_accMin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numericUpDown_accMin.Name = "numericUpDown_accMin";
            numericUpDown_accMin.Size = new System.Drawing.Size(82, 23);
            numericUpDown_accMin.TabIndex = 12;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(7, 80);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(30, 15);
            label5.TabIndex = 11;
            label5.Text = "Acc:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(7, 52);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(24, 15);
            label4.TabIndex = 10;
            label4.Text = "PP:";
            // 
            // numericUpDown_minimumPP
            // 
            numericUpDown_minimumPP.DecimalPlaces = 2;
            numericUpDown_minimumPP.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numericUpDown_minimumPP.Location = new System.Drawing.Point(88, 50);
            numericUpDown_minimumPP.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numericUpDown_minimumPP.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            numericUpDown_minimumPP.Name = "numericUpDown_minimumPP";
            numericUpDown_minimumPP.Size = new System.Drawing.Size(82, 23);
            numericUpDown_minimumPP.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(19, 140);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(177, 15);
            label2.TabIndex = 20;
            label2.Text = "{0} - Username | {1} - Mods used";
            // 
            // label_collectionNameExample
            // 
            label_collectionNameExample.AutoSize = true;
            label_collectionNameExample.Location = new System.Drawing.Point(308, 162);
            label_collectionNameExample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_collectionNameExample.Name = "label_collectionNameExample";
            label_collectionNameExample.Size = new System.Drawing.Size(67, 15);
            label_collectionNameExample.TabIndex = 19;
            label_collectionNameExample.Text = "<example>";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(19, 121);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(147, 15);
            label7.TabIndex = 18;
            label7.Text = "Collection naming format:";
            // 
            // textBox_collectionNameFormat
            // 
            textBox_collectionNameFormat.Location = new System.Drawing.Point(22, 158);
            textBox_collectionNameFormat.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox_collectionNameFormat.Name = "textBox_collectionNameFormat";
            textBox_collectionNameFormat.Size = new System.Drawing.Size(279, 23);
            textBox_collectionNameFormat.TabIndex = 17;
            textBox_collectionNameFormat.Text = "#Tops.{0}";
            textBox_collectionNameFormat.TextChanged += textBox_collectionNameFormat_TextChanged;
            // 
            // textBox_usernames
            // 
            textBox_usernames.Location = new System.Drawing.Point(22, 38);
            textBox_usernames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox_usernames.Multiline = true;
            textBox_usernames.Name = "textBox_usernames";
            textBox_usernames.Size = new System.Drawing.Size(578, 66);
            textBox_usernames.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(19, 20);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(313, 15);
            label1.TabIndex = 9;
            label1.Text = "Enter comma separated list of usernames to get data from";
            // 
            // label_processingStatus
            // 
            label_processingStatus.AutoSize = true;
            label_processingStatus.Location = new System.Drawing.Point(22, 67);
            label_processingStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_processingStatus.Name = "label_processingStatus";
            label_processingStatus.Size = new System.Drawing.Size(57, 15);
            label_processingStatus.TabIndex = 29;
            label_processingStatus.Text = "Waiting...";
            // 
            // progressBar_usernames
            // 
            progressBar_usernames.Location = new System.Drawing.Point(26, 37);
            progressBar_usernames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            progressBar_usernames.Name = "progressBar_usernames";
            progressBar_usernames.Size = new System.Drawing.Size(579, 27);
            progressBar_usernames.TabIndex = 26;
            // 
            // button_Abort
            // 
            button_Abort.Enabled = false;
            button_Abort.Location = new System.Drawing.Point(324, 3);
            button_Abort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button_Abort.Name = "button_Abort";
            button_Abort.Size = new System.Drawing.Size(280, 27);
            button_Abort.TabIndex = 24;
            button_Abort.Text = "Abort";
            button_Abort.UseVisualStyleBackColor = true;
            button_Abort.Click += button_Abort_Click;
            // 
            // button_Start
            // 
            button_Start.Location = new System.Drawing.Point(26, 3);
            button_Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button_Start.Name = "button_Start";
            button_Start.Size = new System.Drawing.Size(280, 27);
            button_Start.TabIndex = 23;
            button_Start.Text = "Start";
            button_Start.UseVisualStyleBackColor = true;
            button_Start.Click += button_Start_Click;
            // 
            // collectionListingView1
            // 
            collectionListingView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            collectionListingView1.HighlightedCollections = null;
            collectionListingView1.Location = new System.Drawing.Point(0, 548);
            collectionListingView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            collectionListingView1.Name = "collectionListingView1";
            collectionListingView1.SelectedCollection = null;
            collectionListingView1.Size = new System.Drawing.Size(604, 378);
            collectionListingView1.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(button_Start);
            panel1.Controls.Add(label_processingStatus);
            panel1.Controls.Add(button_Abort);
            panel1.Controls.Add(progressBar_usernames);
            panel1.Location = new System.Drawing.Point(0, 440);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(605, 102);
            panel1.TabIndex = 2;
            // 
            // UserTopGeneratorView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel1);
            Controls.Add(collectionListingView1);
            Controls.Add(groupBox_configuration);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "UserTopGeneratorView";
            Size = new System.Drawing.Size(613, 927);
            groupBox_configuration.ResumeLayout(false);
            groupBox_configuration.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_maximumPP).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_accMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_accMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_minimumPP).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_configuration;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.NumericUpDown numericUpDown_maximumPP;
        public System.Windows.Forms.RadioButton rb_scores_Adown;
        public System.Windows.Forms.RadioButton rb_scores_Sup;
        public System.Windows.Forms.RadioButton rb_scores_all;
        public System.Windows.Forms.NumericUpDown numericUpDown_accMax;
        public System.Windows.Forms.NumericUpDown numericUpDown_accMin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.NumericUpDown numericUpDown_minimumPP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_collectionNameExample;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox textBox_collectionNameFormat;
        public System.Windows.Forms.TextBox textBox_usernames;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private CollectionListingView collectionListingView1;
        private System.Windows.Forms.ProgressBar progressBar_usernames;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.Button button_Abort;
        public System.Windows.Forms.Button button_Start;
        public System.Windows.Forms.TextBox textBox_apiKey;
        public System.Windows.Forms.Button button_GenerateUsernames;
        private System.Windows.Forms.Label label_processingStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBox_gamemode;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.TextBox textBox_allowedMods;
    }
}
