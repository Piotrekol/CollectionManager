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
            button_LoadOsuCollection = new System.Windows.Forms.Button();
            checkBox_DoNotShowOnStartup = new System.Windows.Forms.CheckBox();
            label_title = new System.Windows.Forms.Label();
            button_LoadCollectionFromFile = new System.Windows.Forms.Button();
            button_DoNothing = new System.Windows.Forms.Button();
            label_LoadDatabaseStatus = new System.Windows.Forms.Label();
            button_LoadDatabase = new System.Windows.Forms.Button();
            groupBox_Step1 = new System.Windows.Forms.GroupBox();
            flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            flowLayoutPanel_loadOsuInstanceDbButtons = new System.Windows.Forms.FlowLayoutPanel();
            button_LoadLazer = new System.Windows.Forms.Button();
            button_LoadStable = new System.Windows.Forms.Button();
            button_LoadDatabaseSkip = new System.Windows.Forms.Button();
            groupBox_Step2 = new System.Windows.Forms.GroupBox();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            label_OpenedCollection = new System.Windows.Forms.Label();
            infoTextView1 = new InfoTextView();
            groupBox_Step1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel_loadOsuInstanceDbButtons.SuspendLayout();
            groupBox_Step2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // button_LoadOsuCollection
            // 
            button_LoadOsuCollection.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            button_LoadOsuCollection.Font = new System.Drawing.Font("Segoe UI", 9F);
            button_LoadOsuCollection.Location = new System.Drawing.Point(3, 37);
            button_LoadOsuCollection.Name = "button_LoadOsuCollection";
            button_LoadOsuCollection.Size = new System.Drawing.Size(495, 23);
            button_LoadOsuCollection.TabIndex = 4;
            button_LoadOsuCollection.Text = "Load your osu! collection";
            button_LoadOsuCollection.UseVisualStyleBackColor = true;
            button_LoadOsuCollection.Click += button_StartupAction_Click;
            // 
            // checkBox_DoNotShowOnStartup
            // 
            checkBox_DoNotShowOnStartup.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            checkBox_DoNotShowOnStartup.AutoSize = true;
            checkBox_DoNotShowOnStartup.Font = new System.Drawing.Font("Segoe UI", 9F);
            checkBox_DoNotShowOnStartup.Location = new System.Drawing.Point(4, 390);
            checkBox_DoNotShowOnStartup.Name = "checkBox_DoNotShowOnStartup";
            checkBox_DoNotShowOnStartup.Size = new System.Drawing.Size(191, 19);
            checkBox_DoNotShowOnStartup.TabIndex = 7;
            checkBox_DoNotShowOnStartup.Text = "Use selected options on startup";
            checkBox_DoNotShowOnStartup.UseVisualStyleBackColor = true;
            // 
            // label_title
            // 
            label_title.AutoSize = true;
            label_title.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            label_title.Location = new System.Drawing.Point(3, 12);
            label_title.Name = "label_title";
            label_title.Size = new System.Drawing.Size(196, 21);
            label_title.TabIndex = 2;
            label_title.Text = "osu! Collection Manager";
            // 
            // button_LoadCollectionFromFile
            // 
            button_LoadCollectionFromFile.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            button_LoadCollectionFromFile.Font = new System.Drawing.Font("Segoe UI", 9F);
            button_LoadCollectionFromFile.Location = new System.Drawing.Point(3, 66);
            button_LoadCollectionFromFile.Name = "button_LoadCollectionFromFile";
            button_LoadCollectionFromFile.Size = new System.Drawing.Size(495, 23);
            button_LoadCollectionFromFile.TabIndex = 5;
            button_LoadCollectionFromFile.Text = "Load osu! collection from file";
            button_LoadCollectionFromFile.UseVisualStyleBackColor = true;
            button_LoadCollectionFromFile.Click += button_StartupAction_Click;
            // 
            // button_DoNothing
            // 
            button_DoNothing.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            button_DoNothing.Font = new System.Drawing.Font("Segoe UI", 9F);
            button_DoNothing.Location = new System.Drawing.Point(3, 95);
            button_DoNothing.Name = "button_DoNothing";
            button_DoNothing.Size = new System.Drawing.Size(495, 23);
            button_DoNothing.TabIndex = 6;
            button_DoNothing.Text = "Continue without loading any more collections";
            button_DoNothing.UseVisualStyleBackColor = true;
            button_DoNothing.Click += button_StartupAction_Click;
            // 
            // label_LoadDatabaseStatus
            // 
            label_LoadDatabaseStatus.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label_LoadDatabaseStatus.Location = new System.Drawing.Point(3, 0);
            label_LoadDatabaseStatus.Name = "label_LoadDatabaseStatus";
            label_LoadDatabaseStatus.Size = new System.Drawing.Size(511, 49);
            label_LoadDatabaseStatus.TabIndex = 6;
            label_LoadDatabaseStatus.Text = "AAA\r\nBBB\r\nCCC";
            label_LoadDatabaseStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_LoadDatabase
            // 
            button_LoadDatabase.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            button_LoadDatabase.Font = new System.Drawing.Font("Segoe UI", 9F);
            button_LoadDatabase.Location = new System.Drawing.Point(3, 81);
            button_LoadDatabase.Name = "button_LoadDatabase";
            button_LoadDatabase.Size = new System.Drawing.Size(495, 23);
            button_LoadDatabase.TabIndex = 2;
            button_LoadDatabase.Text = "Load beatmaps from custom location";
            button_LoadDatabase.UseVisualStyleBackColor = true;
            button_LoadDatabase.Click += button_DatabaseAction_Click;
            // 
            // groupBox_Step1
            // 
            groupBox_Step1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox_Step1.Controls.Add(flowLayoutPanel2);
            groupBox_Step1.Location = new System.Drawing.Point(7, 36);
            groupBox_Step1.Name = "groupBox_Step1";
            groupBox_Step1.Size = new System.Drawing.Size(525, 162);
            groupBox_Step1.TabIndex = 8;
            groupBox_Step1.TabStop = false;
            groupBox_Step1.Text = "Load osu! database";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(label_LoadDatabaseStatus);
            flowLayoutPanel2.Controls.Add(flowLayoutPanel_loadOsuInstanceDbButtons);
            flowLayoutPanel2.Controls.Add(button_LoadDatabase);
            flowLayoutPanel2.Controls.Add(button_LoadDatabaseSkip);
            flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel2.Location = new System.Drawing.Point(3, 19);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new System.Drawing.Size(519, 140);
            flowLayoutPanel2.TabIndex = 10;
            // 
            // flowLayoutPanel_loadOsuInstanceDbButtons
            // 
            flowLayoutPanel_loadOsuInstanceDbButtons.Controls.Add(button_LoadLazer);
            flowLayoutPanel_loadOsuInstanceDbButtons.Controls.Add(button_LoadStable);
            flowLayoutPanel_loadOsuInstanceDbButtons.Location = new System.Drawing.Point(3, 52);
            flowLayoutPanel_loadOsuInstanceDbButtons.Name = "flowLayoutPanel_loadOsuInstanceDbButtons";
            flowLayoutPanel_loadOsuInstanceDbButtons.Size = new System.Drawing.Size(495, 23);
            flowLayoutPanel_loadOsuInstanceDbButtons.TabIndex = 8;
            // 
            // button_LoadLazer
            // 
            button_LoadLazer.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            button_LoadLazer.Font = new System.Drawing.Font("Segoe UI", 9F);
            button_LoadLazer.Location = new System.Drawing.Point(0, 0);
            button_LoadLazer.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            button_LoadLazer.Name = "button_LoadLazer";
            button_LoadLazer.Size = new System.Drawing.Size(245, 23);
            button_LoadLazer.TabIndex = 0;
            button_LoadLazer.Text = "Load osu! lazer beatmaps";
            button_LoadLazer.UseVisualStyleBackColor = true;
            button_LoadLazer.Click += button_DatabaseAction_Click;
            // 
            // button_LoadStable
            // 
            button_LoadStable.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            button_LoadStable.Font = new System.Drawing.Font("Segoe UI", 9F);
            button_LoadStable.Location = new System.Drawing.Point(250, 0);
            button_LoadStable.Margin = new System.Windows.Forms.Padding(0);
            button_LoadStable.Name = "button_LoadStable";
            button_LoadStable.Size = new System.Drawing.Size(245, 23);
            button_LoadStable.TabIndex = 1;
            button_LoadStable.Text = "Load osu! stable beatmaps";
            button_LoadStable.UseVisualStyleBackColor = true;
            button_LoadStable.Click += button_DatabaseAction_Click;
            // 
            // button_LoadDatabaseSkip
            // 
            button_LoadDatabaseSkip.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            button_LoadDatabaseSkip.Font = new System.Drawing.Font("Segoe UI", 9F);
            button_LoadDatabaseSkip.Location = new System.Drawing.Point(3, 110);
            button_LoadDatabaseSkip.Name = "button_LoadDatabaseSkip";
            button_LoadDatabaseSkip.Size = new System.Drawing.Size(495, 23);
            button_LoadDatabaseSkip.TabIndex = 3;
            button_LoadDatabaseSkip.Text = "Skip";
            button_LoadDatabaseSkip.UseVisualStyleBackColor = true;
            button_LoadDatabaseSkip.Click += button_DatabaseAction_Click;
            // 
            // groupBox_Step2
            // 
            groupBox_Step2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox_Step2.Controls.Add(flowLayoutPanel1);
            groupBox_Step2.Location = new System.Drawing.Point(7, 200);
            groupBox_Step2.Name = "groupBox_Step2";
            groupBox_Step2.Size = new System.Drawing.Size(525, 143);
            groupBox_Step2.TabIndex = 9;
            groupBox_Step2.TabStop = false;
            groupBox_Step2.Text = "Starting point";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(label_OpenedCollection);
            flowLayoutPanel1.Controls.Add(button_LoadOsuCollection);
            flowLayoutPanel1.Controls.Add(button_LoadCollectionFromFile);
            flowLayoutPanel1.Controls.Add(button_DoNothing);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.Location = new System.Drawing.Point(3, 19);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(519, 121);
            flowLayoutPanel1.TabIndex = 10;
            // 
            // label_OpenedCollection
            // 
            label_OpenedCollection.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label_OpenedCollection.Location = new System.Drawing.Point(3, 0);
            label_OpenedCollection.Name = "label_OpenedCollection";
            label_OpenedCollection.Size = new System.Drawing.Size(511, 34);
            label_OpenedCollection.TabIndex = 7;
            label_OpenedCollection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoTextView1
            // 
            infoTextView1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            infoTextView1.CMStatusVisiable = false;
            infoTextView1.Location = new System.Drawing.Point(4, 366);
            infoTextView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            infoTextView1.Name = "infoTextView1";
            infoTextView1.Size = new System.Drawing.Size(525, 18);
            infoTextView1.TabIndex = 10;
            // 
            // StartupView
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            Controls.Add(infoTextView1);
            Controls.Add(groupBox_Step2);
            Controls.Add(groupBox_Step1);
            Controls.Add(label_title);
            Controls.Add(checkBox_DoNotShowOnStartup);
            Name = "StartupView";
            Size = new System.Drawing.Size(535, 412);
            groupBox_Step1.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel_loadOsuInstanceDbButtons.ResumeLayout(false);
            groupBox_Step2.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

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
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label_OpenedCollection;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private InfoTextView infoTextView1;
		private System.Windows.Forms.Button button_LoadLazer;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_loadOsuInstanceDbButtons;
        private System.Windows.Forms.Button button_LoadStable;
    }
}
