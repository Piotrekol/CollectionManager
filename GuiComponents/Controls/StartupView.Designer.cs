namespace GuiComponents.Controls
{
    partial class StartupView
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
            this.button_LoadOsuCollection = new System.Windows.Forms.Button();
            this.checkBox_DoNotShowOnStartup = new System.Windows.Forms.CheckBox();
            this.label_title = new System.Windows.Forms.Label();
            this.button_LoadCollectionFromFile = new System.Windows.Forms.Button();
            this.button_DoNothing = new System.Windows.Forms.Button();
            this.label_LoadDatabaseStatus = new System.Windows.Forms.Label();
            this.button_LoadDatabase = new System.Windows.Forms.Button();
            this.groupBox_Step1 = new System.Windows.Forms.GroupBox();
            this.button_LoadDatabaseSkip = new System.Windows.Forms.Button();
            this.groupBox_Step2 = new System.Windows.Forms.GroupBox();
            this.groupBox_Step1.SuspendLayout();
            this.groupBox_Step2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_LoadOsuCollection
            // 
            this.button_LoadOsuCollection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_LoadOsuCollection.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.button_LoadOsuCollection.Location = new System.Drawing.Point(6, 19);
            this.button_LoadOsuCollection.Name = "button_LoadOsuCollection";
            this.button_LoadOsuCollection.Size = new System.Drawing.Size(519, 23);
            this.button_LoadOsuCollection.TabIndex = 0;
            this.button_LoadOsuCollection.Text = "Load your osu! collection";
            this.button_LoadOsuCollection.UseVisualStyleBackColor = true;
            this.button_LoadOsuCollection.Click += new System.EventHandler(this.button_StartupAction_Click);
            // 
            // checkBox_DoNotShowOnStartup
            // 
            this.checkBox_DoNotShowOnStartup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_DoNotShowOnStartup.AutoSize = true;
            this.checkBox_DoNotShowOnStartup.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBox_DoNotShowOnStartup.Location = new System.Drawing.Point(4, 363);
            this.checkBox_DoNotShowOnStartup.Name = "checkBox_DoNotShowOnStartup";
            this.checkBox_DoNotShowOnStartup.Size = new System.Drawing.Size(191, 19);
            this.checkBox_DoNotShowOnStartup.TabIndex = 1;
            this.checkBox_DoNotShowOnStartup.Text = "Use selected options on startup";
            this.checkBox_DoNotShowOnStartup.UseVisualStyleBackColor = true;
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label_title.Location = new System.Drawing.Point(3, 12);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(196, 21);
            this.label_title.TabIndex = 2;
            this.label_title.Text = "osu! Collection Manager";
            // 
            // button_LoadCollectionFromFile
            // 
            this.button_LoadCollectionFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_LoadCollectionFromFile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.button_LoadCollectionFromFile.Location = new System.Drawing.Point(6, 48);
            this.button_LoadCollectionFromFile.Name = "button_LoadCollectionFromFile";
            this.button_LoadCollectionFromFile.Size = new System.Drawing.Size(519, 23);
            this.button_LoadCollectionFromFile.TabIndex = 3;
            this.button_LoadCollectionFromFile.Text = "Load osu! collection from file";
            this.button_LoadCollectionFromFile.UseVisualStyleBackColor = true;
            this.button_LoadCollectionFromFile.Click += new System.EventHandler(this.button_StartupAction_Click);
            // 
            // button_DoNothing
            // 
            this.button_DoNothing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_DoNothing.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.button_DoNothing.Location = new System.Drawing.Point(6, 77);
            this.button_DoNothing.Name = "button_DoNothing";
            this.button_DoNothing.Size = new System.Drawing.Size(519, 23);
            this.button_DoNothing.TabIndex = 4;
            this.button_DoNothing.Text = "Continue without loading collections";
            this.button_DoNothing.UseVisualStyleBackColor = true;
            this.button_DoNothing.Click += new System.EventHandler(this.button_StartupAction_Click);
            // 
            // label_LoadDatabaseStatus
            // 
            this.label_LoadDatabaseStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_LoadDatabaseStatus.Location = new System.Drawing.Point(6, 18);
            this.label_LoadDatabaseStatus.Name = "label_LoadDatabaseStatus";
            this.label_LoadDatabaseStatus.Size = new System.Drawing.Size(519, 62);
            this.label_LoadDatabaseStatus.TabIndex = 6;
            this.label_LoadDatabaseStatus.Text = "...";
            this.label_LoadDatabaseStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_LoadDatabase
            // 
            this.button_LoadDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_LoadDatabase.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.button_LoadDatabase.Location = new System.Drawing.Point(6, 83);
            this.button_LoadDatabase.Name = "button_LoadDatabase";
            this.button_LoadDatabase.Size = new System.Drawing.Size(519, 23);
            this.button_LoadDatabase.TabIndex = 7;
            this.button_LoadDatabase.Text = "Load beatmaps from different location";
            this.button_LoadDatabase.UseVisualStyleBackColor = true;
            this.button_LoadDatabase.Click += new System.EventHandler(this.button_DatabaseAction_Click);
            // 
            // groupBox_Step1
            // 
            this.groupBox_Step1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Step1.Controls.Add(this.button_LoadDatabaseSkip);
            this.groupBox_Step1.Controls.Add(this.button_LoadDatabase);
            this.groupBox_Step1.Controls.Add(this.label_LoadDatabaseStatus);
            this.groupBox_Step1.Location = new System.Drawing.Point(7, 36);
            this.groupBox_Step1.Name = "groupBox_Step1";
            this.groupBox_Step1.Size = new System.Drawing.Size(531, 147);
            this.groupBox_Step1.TabIndex = 8;
            this.groupBox_Step1.TabStop = false;
            this.groupBox_Step1.Text = "Load osu! database";
            // 
            // button_LoadDatabaseSkip
            // 
            this.button_LoadDatabaseSkip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_LoadDatabaseSkip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.button_LoadDatabaseSkip.Location = new System.Drawing.Point(6, 113);
            this.button_LoadDatabaseSkip.Name = "button_LoadDatabaseSkip";
            this.button_LoadDatabaseSkip.Size = new System.Drawing.Size(519, 23);
            this.button_LoadDatabaseSkip.TabIndex = 9;
            this.button_LoadDatabaseSkip.Text = "Skip";
            this.button_LoadDatabaseSkip.UseVisualStyleBackColor = true;
            this.button_LoadDatabaseSkip.Click += new System.EventHandler(this.button_DatabaseAction_Click);
            // 
            // groupBox_Step2
            // 
            this.groupBox_Step2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Step2.Controls.Add(this.button_LoadOsuCollection);
            this.groupBox_Step2.Controls.Add(this.button_LoadCollectionFromFile);
            this.groupBox_Step2.Controls.Add(this.button_DoNothing);
            this.groupBox_Step2.Location = new System.Drawing.Point(7, 189);
            this.groupBox_Step2.Name = "groupBox_Step2";
            this.groupBox_Step2.Size = new System.Drawing.Size(531, 112);
            this.groupBox_Step2.TabIndex = 9;
            this.groupBox_Step2.TabStop = false;
            this.groupBox_Step2.Text = "Starting point";
            // 
            // StartupView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox_Step2);
            this.Controls.Add(this.groupBox_Step1);
            this.Controls.Add(this.label_title);
            this.Controls.Add(this.checkBox_DoNotShowOnStartup);
            this.Name = "StartupView";
            this.Size = new System.Drawing.Size(541, 385);
            this.groupBox_Step1.ResumeLayout(false);
            this.groupBox_Step2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_LoadOsuCollection;
        private System.Windows.Forms.CheckBox checkBox_DoNotShowOnStartup;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Button button_LoadCollectionFromFile;
        private System.Windows.Forms.Button button_DoNothing;
        private System.Windows.Forms.Label label_LoadDatabaseStatus;
        private System.Windows.Forms.Button button_LoadDatabase;
        private System.Windows.Forms.GroupBox groupBox_Step1;
        private System.Windows.Forms.GroupBox groupBox_Step2;
        private System.Windows.Forms.Button button_LoadDatabaseSkip;
    }
}
