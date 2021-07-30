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
            this.groupBox_configuration = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBox_gamemode = new System.Windows.Forms.ComboBox();
            this.button_GenerateUsernames = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_apiKey = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown_maximumPP = new System.Windows.Forms.NumericUpDown();
            this.rb_scores_Adown = new System.Windows.Forms.RadioButton();
            this.rb_scores_Sup = new System.Windows.Forms.RadioButton();
            this.rb_scores_all = new System.Windows.Forms.RadioButton();
            this.numericUpDown_accMax = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_accMin = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown_minimumPP = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label_collectionNameExample = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_collectionNameFormat = new System.Windows.Forms.TextBox();
            this.textBox_usernames = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label_processingStatus = new System.Windows.Forms.Label();
            this.progressBar_usernames = new System.Windows.Forms.ProgressBar();
            this.button_Abort = new System.Windows.Forms.Button();
            this.button_Start = new System.Windows.Forms.Button();
            this.collectionListingView1 = new GuiComponents.Controls.CollectionListingView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox_allowedMods = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox_configuration.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maximumPP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_accMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_accMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_minimumPP)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_configuration
            // 
            this.groupBox_configuration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_configuration.Controls.Add(this.label11);
            this.groupBox_configuration.Controls.Add(this.comboBox_gamemode);
            this.groupBox_configuration.Controls.Add(this.button_GenerateUsernames);
            this.groupBox_configuration.Controls.Add(this.label6);
            this.groupBox_configuration.Controls.Add(this.textBox_apiKey);
            this.groupBox_configuration.Controls.Add(this.groupBox3);
            this.groupBox_configuration.Controls.Add(this.label2);
            this.groupBox_configuration.Controls.Add(this.label_collectionNameExample);
            this.groupBox_configuration.Controls.Add(this.label7);
            this.groupBox_configuration.Controls.Add(this.textBox_collectionNameFormat);
            this.groupBox_configuration.Controls.Add(this.textBox_usernames);
            this.groupBox_configuration.Controls.Add(this.label1);
            this.groupBox_configuration.Location = new System.Drawing.Point(3, 3);
            this.groupBox_configuration.Name = "groupBox_configuration";
            this.groupBox_configuration.Size = new System.Drawing.Size(535, 372);
            this.groupBox_configuration.TabIndex = 0;
            this.groupBox_configuration.TabStop = false;
            this.groupBox_configuration.Text = "Configuration";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(299, 327);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "Gamemode:";
            // 
            // comboBox_gamemode
            // 
            this.comboBox_gamemode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_gamemode.FormattingEnabled = true;
            this.comboBox_gamemode.Items.AddRange(new object[] {
            "osu",
            "taiko",
            "ctb",
            "mania"});
            this.comboBox_gamemode.Location = new System.Drawing.Point(302, 344);
            this.comboBox_gamemode.Name = "comboBox_gamemode";
            this.comboBox_gamemode.Size = new System.Drawing.Size(121, 21);
            this.comboBox_gamemode.TabIndex = 29;
            // 
            // button_GenerateUsernames
            // 
            this.button_GenerateUsernames.Location = new System.Drawing.Point(395, 95);
            this.button_GenerateUsernames.Name = "button_GenerateUsernames";
            this.button_GenerateUsernames.Size = new System.Drawing.Size(120, 23);
            this.button_GenerateUsernames.TabIndex = 28;
            this.button_GenerateUsernames.Text = "Generate usernames";
            this.button_GenerateUsernames.UseVisualStyleBackColor = true;
            this.button_GenerateUsernames.Click += new System.EventHandler(this.button_GenerateUsernames_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 328);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Your osu!api key:";
            // 
            // textBox_apiKey
            // 
            this.textBox_apiKey.Location = new System.Drawing.Point(19, 344);
            this.textBox_apiKey.Name = "textBox_apiKey";
            this.textBox_apiKey.PasswordChar = '*';
            this.textBox_apiKey.Size = new System.Drawing.Size(231, 20);
            this.textBox_apiKey.TabIndex = 22;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.textBox_allowedMods);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.numericUpDown_maximumPP);
            this.groupBox3.Controls.Add(this.rb_scores_Adown);
            this.groupBox3.Controls.Add(this.rb_scores_Sup);
            this.groupBox3.Controls.Add(this.rb_scores_all);
            this.groupBox3.Controls.Add(this.numericUpDown_accMax);
            this.groupBox3.Controls.Add(this.numericUpDown_accMin);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.numericUpDown_minimumPP);
            this.groupBox3.Location = new System.Drawing.Point(19, 163);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(496, 129);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Conditions";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(151, 69);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "max:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(43, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "min:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(151, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "max:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "min:";
            // 
            // numericUpDown_maximumPP
            // 
            this.numericUpDown_maximumPP.DecimalPlaces = 2;
            this.numericUpDown_maximumPP.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown_maximumPP.Location = new System.Drawing.Point(186, 43);
            this.numericUpDown_maximumPP.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numericUpDown_maximumPP.Name = "numericUpDown_maximumPP";
            this.numericUpDown_maximumPP.Size = new System.Drawing.Size(70, 20);
            this.numericUpDown_maximumPP.TabIndex = 21;
            this.numericUpDown_maximumPP.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // rb_scores_Adown
            // 
            this.rb_scores_Adown.AutoSize = true;
            this.rb_scores_Adown.Location = new System.Drawing.Point(195, 20);
            this.rb_scores_Adown.Name = "rb_scores_Adown";
            this.rb_scores_Adown.Size = new System.Drawing.Size(108, 17);
            this.rb_scores_Adown.TabIndex = 17;
            this.rb_scores_Adown.Tag = "2";
            this.rb_scores_Adown.Text = "Only A and worse";
            this.rb_scores_Adown.UseVisualStyleBackColor = true;
            this.rb_scores_Adown.CheckedChanged += new System.EventHandler(this.RadioButton_AllowScores_CheckedChanged);
            // 
            // rb_scores_Sup
            // 
            this.rb_scores_Sup.AutoSize = true;
            this.rb_scores_Sup.Location = new System.Drawing.Point(85, 19);
            this.rb_scores_Sup.Name = "rb_scores_Sup";
            this.rb_scores_Sup.Size = new System.Drawing.Size(107, 17);
            this.rb_scores_Sup.TabIndex = 16;
            this.rb_scores_Sup.Tag = "1";
            this.rb_scores_Sup.Text = "Only S and better";
            this.rb_scores_Sup.UseVisualStyleBackColor = true;
            this.rb_scores_Sup.CheckedChanged += new System.EventHandler(this.RadioButton_AllowScores_CheckedChanged);
            // 
            // rb_scores_all
            // 
            this.rb_scores_all.AutoSize = true;
            this.rb_scores_all.Checked = true;
            this.rb_scores_all.Location = new System.Drawing.Point(9, 19);
            this.rb_scores_all.Name = "rb_scores_all";
            this.rb_scores_all.Size = new System.Drawing.Size(70, 17);
            this.rb_scores_all.TabIndex = 15;
            this.rb_scores_all.TabStop = true;
            this.rb_scores_all.Tag = "0";
            this.rb_scores_all.Text = "All scores";
            this.rb_scores_all.UseVisualStyleBackColor = true;
            this.rb_scores_all.CheckedChanged += new System.EventHandler(this.RadioButton_AllowScores_CheckedChanged);
            // 
            // numericUpDown_accMax
            // 
            this.numericUpDown_accMax.DecimalPlaces = 2;
            this.numericUpDown_accMax.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown_accMax.Location = new System.Drawing.Point(186, 69);
            this.numericUpDown_accMax.Name = "numericUpDown_accMax";
            this.numericUpDown_accMax.Size = new System.Drawing.Size(70, 20);
            this.numericUpDown_accMax.TabIndex = 13;
            this.numericUpDown_accMax.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numericUpDown_accMin
            // 
            this.numericUpDown_accMin.DecimalPlaces = 2;
            this.numericUpDown_accMin.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown_accMin.Location = new System.Drawing.Point(75, 67);
            this.numericUpDown_accMin.Name = "numericUpDown_accMin";
            this.numericUpDown_accMin.Size = new System.Drawing.Size(70, 20);
            this.numericUpDown_accMin.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Acc:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "PP:";
            // 
            // numericUpDown_minimumPP
            // 
            this.numericUpDown_minimumPP.DecimalPlaces = 2;
            this.numericUpDown_minimumPP.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown_minimumPP.Location = new System.Drawing.Point(75, 43);
            this.numericUpDown_minimumPP.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown_minimumPP.Name = "numericUpDown_minimumPP";
            this.numericUpDown_minimumPP.Size = new System.Drawing.Size(70, 20);
            this.numericUpDown_minimumPP.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "{0} - Username | {1} - Mods used";
            // 
            // label_collectionNameExample
            // 
            this.label_collectionNameExample.AutoSize = true;
            this.label_collectionNameExample.Location = new System.Drawing.Point(264, 140);
            this.label_collectionNameExample.Name = "label_collectionNameExample";
            this.label_collectionNameExample.Size = new System.Drawing.Size(58, 13);
            this.label_collectionNameExample.TabIndex = 19;
            this.label_collectionNameExample.Text = "<example>";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 105);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Collection naming format:";
            // 
            // textBox_collectionNameFormat
            // 
            this.textBox_collectionNameFormat.Location = new System.Drawing.Point(19, 137);
            this.textBox_collectionNameFormat.Name = "textBox_collectionNameFormat";
            this.textBox_collectionNameFormat.Size = new System.Drawing.Size(240, 20);
            this.textBox_collectionNameFormat.TabIndex = 17;
            this.textBox_collectionNameFormat.Text = "#Tops.{0}";
            this.textBox_collectionNameFormat.TextChanged += new System.EventHandler(this.textBox_collectionNameFormat_TextChanged);
            // 
            // textBox_usernames
            // 
            this.textBox_usernames.Location = new System.Drawing.Point(19, 33);
            this.textBox_usernames.Multiline = true;
            this.textBox_usernames.Name = "textBox_usernames";
            this.textBox_usernames.Size = new System.Drawing.Size(496, 58);
            this.textBox_usernames.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(277, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Enter comma separated list of usernames to get data from";
            // 
            // label_processingStatus
            // 
            this.label_processingStatus.AutoSize = true;
            this.label_processingStatus.Location = new System.Drawing.Point(19, 58);
            this.label_processingStatus.Name = "label_processingStatus";
            this.label_processingStatus.Size = new System.Drawing.Size(52, 13);
            this.label_processingStatus.TabIndex = 29;
            this.label_processingStatus.Text = "Waiting...";
            // 
            // progressBar_usernames
            // 
            this.progressBar_usernames.Location = new System.Drawing.Point(22, 32);
            this.progressBar_usernames.Name = "progressBar_usernames";
            this.progressBar_usernames.Size = new System.Drawing.Size(496, 23);
            this.progressBar_usernames.TabIndex = 26;
            // 
            // button_Abort
            // 
            this.button_Abort.Enabled = false;
            this.button_Abort.Location = new System.Drawing.Point(278, 3);
            this.button_Abort.Name = "button_Abort";
            this.button_Abort.Size = new System.Drawing.Size(240, 23);
            this.button_Abort.TabIndex = 24;
            this.button_Abort.Text = "Abort";
            this.button_Abort.UseVisualStyleBackColor = true;
            this.button_Abort.Click += new System.EventHandler(this.button_Abort_Click);
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(22, 3);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(240, 23);
            this.button_Start.TabIndex = 23;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // collectionListingView1
            // 
            this.collectionListingView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collectionListingView1.Location = new System.Drawing.Point(0, 475);
            this.collectionListingView1.Name = "collectionListingView1";
            this.collectionListingView1.SelectedCollection = null;
            this.collectionListingView1.Size = new System.Drawing.Size(538, 328);
            this.collectionListingView1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_Start);
            this.panel1.Controls.Add(this.label_processingStatus);
            this.panel1.Controls.Add(this.button_Abort);
            this.panel1.Controls.Add(this.progressBar_usernames);
            this.panel1.Location = new System.Drawing.Point(0, 381);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(538, 88);
            this.panel1.TabIndex = 2;
            // 
            // textBox_allowedMods
            // 
            this.textBox_allowedMods.Location = new System.Drawing.Point(195, 95);
            this.textBox_allowedMods.Name = "textBox_allowedMods";
            this.textBox_allowedMods.Size = new System.Drawing.Size(228, 20);
            this.textBox_allowedMods.TabIndex = 31;
            this.textBox_allowedMods.Text = "All";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 98);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(187, 13);
            this.label12.TabIndex = 31;
            this.label12.Text = "comma separated list of mods to save:";
            // 
            // UserTopGeneratorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.collectionListingView1);
            this.Controls.Add(this.groupBox_configuration);
            this.Name = "UserTopGeneratorView";
            this.Size = new System.Drawing.Size(538, 803);
            this.groupBox_configuration.ResumeLayout(false);
            this.groupBox_configuration.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maximumPP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_accMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_accMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_minimumPP)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

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
