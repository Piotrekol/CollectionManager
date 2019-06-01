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
            this.mainSidePanelView1 = new GuiComponents.Controls.MainSidePanelView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.combinedListingView1 = new GuiComponents.Controls.CombinedListingView();
            this.tabControlEx1 = new GuiComponents.Controls.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.combinedBeatmapPreviewView1 = new GuiComponents.Controls.CombinedBeatmapPreviewView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.collectionTextView1 = new GuiComponents.Controls.CollectionTextView();
            this.infoTextView1 = new GuiComponents.Controls.InfoTextView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlEx1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSidePanelView1
            // 
            this.mainSidePanelView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainSidePanelView1.Location = new System.Drawing.Point(0, 0);
            this.mainSidePanelView1.Name = "mainSidePanelView1";
            this.mainSidePanelView1.Size = new System.Drawing.Size(1356, 23);
            this.mainSidePanelView1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 42);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.combinedListingView1);
            this.splitContainer1.Panel1MinSize = 50;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlEx1);
            this.splitContainer1.Size = new System.Drawing.Size(1351, 508);
            this.splitContainer1.SplitterDistance = 883;
            this.splitContainer1.TabIndex = 3;
            // 
            // combinedListingView1
            // 
            this.combinedListingView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.combinedListingView1.Location = new System.Drawing.Point(0, 0);
            this.combinedListingView1.Name = "combinedListingView1";
            this.combinedListingView1.Size = new System.Drawing.Size(883, 508);
            this.combinedListingView1.TabIndex = 0;
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.Controls.Add(this.tabPage1);
            this.tabControlEx1.Controls.Add(this.tabPage2);
            this.tabControlEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEx1.Location = new System.Drawing.Point(0, 0);
            this.tabControlEx1.Name = "tabControlEx1";
            this.tabControlEx1.SelectedIndex = 0;
            this.tabControlEx1.Size = new System.Drawing.Size(464, 508);
            this.tabControlEx1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.combinedBeatmapPreviewView1);
            this.tabPage1.Location = new System.Drawing.Point(0, 20);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(462, 487);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "map preview";
            // 
            // combinedBeatmapPreviewView1
            // 
            this.combinedBeatmapPreviewView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.combinedBeatmapPreviewView1.Location = new System.Drawing.Point(3, 3);
            this.combinedBeatmapPreviewView1.Name = "combinedBeatmapPreviewView1";
            this.combinedBeatmapPreviewView1.Size = new System.Drawing.Size(456, 481);
            this.combinedBeatmapPreviewView1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.collectionTextView1);
            this.tabPage2.Location = new System.Drawing.Point(0, 20);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(462, 487);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Collection text";
            // 
            // collectionTextView1
            // 
            this.collectionTextView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collectionTextView1.Location = new System.Drawing.Point(3, 3);
            this.collectionTextView1.Name = "collectionTextView1";
            this.collectionTextView1.Size = new System.Drawing.Size(456, 481);
            this.collectionTextView1.TabIndex = 0;
            // 
            // infoTextView1
            // 
            this.infoTextView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoTextView1.Location = new System.Drawing.Point(0, 23);
            this.infoTextView1.Name = "infoTextView1";
            this.infoTextView1.Size = new System.Drawing.Size(1356, 16);
            this.infoTextView1.TabIndex = 4;
            // 
            // MainFormView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1356, 550);
            this.Controls.Add(this.infoTextView1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainSidePanelView1);
            this.Name = "MainFormView";
            this.Text = "Collection Manager by Piotrekol";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlEx1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

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