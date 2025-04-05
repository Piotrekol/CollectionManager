using GuiComponents.Controls;

namespace GuiComponents.Forms
{
    partial class MainFormView
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
            mainSidePanelView1 = new MainSidePanelView();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            combinedListingView1 = new CombinedListingView();
            tabControlEx1 = new TabControlEx();
            tabPage1 = new System.Windows.Forms.TabPage();
            combinedBeatmapPreviewView1 = new CombinedBeatmapPreviewView();
            tabPage2 = new System.Windows.Forms.TabPage();
            collectionTextView1 = new CollectionTextView();
            infoTextView1 = new InfoTextView();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControlEx1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // mainSidePanelView1
            // 
            mainSidePanelView1.Dock = System.Windows.Forms.DockStyle.Top;
            mainSidePanelView1.Location = new System.Drawing.Point(0, 0);
            mainSidePanelView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            mainSidePanelView1.Name = "mainSidePanelView1";
            mainSidePanelView1.Size = new System.Drawing.Size(1582, 27);
            mainSidePanelView1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            splitContainer1.Location = new System.Drawing.Point(4, 48);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(combinedListingView1);
            splitContainer1.Panel1MinSize = 50;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabControlEx1);
            splitContainer1.Size = new System.Drawing.Size(1576, 586);
            splitContainer1.SplitterDistance = 1030;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 3;
            // 
            // combinedListingView1
            // 
            combinedListingView1.Dock = System.Windows.Forms.DockStyle.Fill;
            combinedListingView1.Location = new System.Drawing.Point(0, 0);
            combinedListingView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            combinedListingView1.Name = "combinedListingView1";
            combinedListingView1.Size = new System.Drawing.Size(1030, 586);
            combinedListingView1.TabIndex = 0;
            // 
            // tabControlEx1
            // 
            tabControlEx1.Controls.Add(tabPage1);
            tabControlEx1.Controls.Add(tabPage2);
            tabControlEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlEx1.Location = new System.Drawing.Point(0, 0);
            tabControlEx1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControlEx1.Name = "tabControlEx1";
            tabControlEx1.SelectedIndex = 0;
            tabControlEx1.Size = new System.Drawing.Size(541, 586);
            tabControlEx1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = System.Drawing.SystemColors.Control;
            tabPage1.Controls.Add(combinedBeatmapPreviewView1);
            tabPage1.Location = new System.Drawing.Point(0, 22);
            tabPage1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage1.Size = new System.Drawing.Size(539, 563);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "map preview";
            // 
            // combinedBeatmapPreviewView1
            // 
            combinedBeatmapPreviewView1.Dock = System.Windows.Forms.DockStyle.Fill;
            combinedBeatmapPreviewView1.Location = new System.Drawing.Point(4, 3);
            combinedBeatmapPreviewView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            combinedBeatmapPreviewView1.Name = "combinedBeatmapPreviewView1";
            combinedBeatmapPreviewView1.Size = new System.Drawing.Size(531, 557);
            combinedBeatmapPreviewView1.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = System.Drawing.SystemColors.Control;
            tabPage2.Controls.Add(collectionTextView1);
            tabPage2.Location = new System.Drawing.Point(0, 22);
            tabPage2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage2.Size = new System.Drawing.Size(539, 563);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Collection text";
            // 
            // collectionTextView1
            // 
            collectionTextView1.Dock = System.Windows.Forms.DockStyle.Fill;
            collectionTextView1.Location = new System.Drawing.Point(4, 3);
            collectionTextView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            collectionTextView1.Name = "collectionTextView1";
            collectionTextView1.Size = new System.Drawing.Size(531, 557);
            collectionTextView1.TabIndex = 0;
            // 
            // infoTextView1
            // 
            infoTextView1.CMStatusVisiable = true;
            infoTextView1.Dock = System.Windows.Forms.DockStyle.Top;
            infoTextView1.Location = new System.Drawing.Point(0, 27);
            infoTextView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            infoTextView1.Name = "infoTextView1";
            infoTextView1.Size = new System.Drawing.Size(1582, 18);
            infoTextView1.TabIndex = 4;
            // 
            // MainFormView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1582, 635);
            Controls.Add(infoTextView1);
            Controls.Add(splitContainer1);
            Controls.Add(mainSidePanelView1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MainFormView";
            Text = "Collection Manager by Piotrekol";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControlEx1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private Controls.MainSidePanelView mainSidePanelView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Controls.CollectionTextView collectionTextView1;
        private CombinedBeatmapPreviewView combinedBeatmapPreviewView1;
        private InfoTextView infoTextView1;
        private CombinedListingView combinedListingView1;
    }
}