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
            collectionRenameView1 = new GuiComponents.Controls.CollectionRenameView();
            SuspendLayout();
            // 
            // collectionRenameView1
            // 
            collectionRenameView1.Dock = System.Windows.Forms.DockStyle.Fill;
            collectionRenameView1.IsRenameView = true;
            collectionRenameView1.Location = new System.Drawing.Point(0, 0);
            collectionRenameView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            collectionRenameView1.Name = "collectionRenameView1";
            collectionRenameView1.NewCollectionName = "";
            collectionRenameView1.OrginalCollectionName = "from";
            collectionRenameView1.Size = new System.Drawing.Size(441, 133);
            collectionRenameView1.TabIndex = 0;
            // 
            // CollectionAddRenameForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(441, 133);
            Controls.Add(collectionRenameView1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "CollectionAddRenameForm";
            Text = "CollectionRenameForm";
            ResumeLayout(false);

        }

        #endregion

        protected Controls.CollectionRenameView collectionRenameView1;
    }
}