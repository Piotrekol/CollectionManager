namespace GuiComponents.Forms
{
    partial class CollectionAddRenameForm
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
            this.collectionRenameView1 = new GuiComponents.Controls.CollectionRenameView();
            this.SuspendLayout();
            // 
            // collectionRenameView1
            // 
            this.collectionRenameView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collectionRenameView1.IsRenameView = true;
            this.collectionRenameView1.Location = new System.Drawing.Point(0, 0);
            this.collectionRenameView1.Name = "collectionRenameView1";
            this.collectionRenameView1.OrginalCollectionName = "from";
            this.collectionRenameView1.Size = new System.Drawing.Size(378, 115);
            this.collectionRenameView1.TabIndex = 0;
            // 
            // CollectionAddRenameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 115);
            this.Controls.Add(this.collectionRenameView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CollectionAddRenameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CollectionRenameForm";
            this.ResumeLayout(false);

        }

        #endregion

        protected Controls.CollectionRenameView collectionRenameView1;
    }
}