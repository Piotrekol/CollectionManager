namespace GuiComponents.Controls
{
    partial class CollectionRenameView
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
            this.label_Error = new System.Windows.Forms.Label();
            this.panel_bottom = new System.Windows.Forms.Panel();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_rename = new System.Windows.Forms.Button();
            this.panel_Top = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label_orginalCollectionName = new System.Windows.Forms.Label();
            this.textBox_newCollectionName = new System.Windows.Forms.TextBox();
            this.panel_bottom.SuspendLayout();
            this.panel_Top.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_Error
            // 
            this.label_Error.AutoSize = true;
            this.label_Error.Location = new System.Drawing.Point(32, 4);
            this.label_Error.Name = "label_Error";
            this.label_Error.Size = new System.Drawing.Size(125, 13);
            this.label_Error.TabIndex = 11;
            this.label_Error.Text = "This name already exists!";
            this.label_Error.Visible = false;
            // 
            // panel_bottom
            // 
            this.panel_bottom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_bottom.Controls.Add(this.label_Error);
            this.panel_bottom.Controls.Add(this.button_cancel);
            this.panel_bottom.Controls.Add(this.button_rename);
            this.panel_bottom.Location = new System.Drawing.Point(31, 66);
            this.panel_bottom.Name = "panel_bottom";
            this.panel_bottom.Size = new System.Drawing.Size(188, 46);
            this.panel_bottom.TabIndex = 10;
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(110, 20);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 5;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // button_rename
            // 
            this.button_rename.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_rename.Enabled = false;
            this.button_rename.Location = new System.Drawing.Point(5, 20);
            this.button_rename.Name = "button_rename";
            this.button_rename.Size = new System.Drawing.Size(75, 23);
            this.button_rename.TabIndex = 4;
            this.button_rename.Text = "Rename";
            this.button_rename.UseVisualStyleBackColor = true;
            // 
            // panel_Top
            // 
            this.panel_Top.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_Top.Controls.Add(this.label2);
            this.panel_Top.Controls.Add(this.label1);
            this.panel_Top.Controls.Add(this.label_orginalCollectionName);
            this.panel_Top.Controls.Add(this.textBox_newCollectionName);
            this.panel_Top.Location = new System.Drawing.Point(0, 0);
            this.panel_Top.MaximumSize = new System.Drawing.Size(254, 63);
            this.panel_Top.Name = "panel_Top";
            this.panel_Top.Size = new System.Drawing.Size(254, 63);
            this.panel_Top.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "New name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Orginal name:";
            // 
            // label_orginalCollectionName
            // 
            this.label_orginalCollectionName.AutoSize = true;
            this.label_orginalCollectionName.Location = new System.Drawing.Point(81, 7);
            this.label_orginalCollectionName.Name = "label_orginalCollectionName";
            this.label_orginalCollectionName.Size = new System.Drawing.Size(27, 13);
            this.label_orginalCollectionName.TabIndex = 2;
            this.label_orginalCollectionName.Text = "from";
            // 
            // textBox_newCollectionName
            // 
            this.textBox_newCollectionName.Location = new System.Drawing.Point(84, 34);
            this.textBox_newCollectionName.Name = "textBox_newCollectionName";
            this.textBox_newCollectionName.Size = new System.Drawing.Size(150, 20);
            this.textBox_newCollectionName.TabIndex = 3;
            // 
            // CollectionRenameView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_bottom);
            this.Controls.Add(this.panel_Top);
            this.Name = "CollectionRenameView";
            this.Size = new System.Drawing.Size(255, 112);
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            this.panel_Top.ResumeLayout(false);
            this.panel_Top.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_Error;
        private System.Windows.Forms.Panel panel_bottom;
        public System.Windows.Forms.Button button_rename;
        private System.Windows.Forms.Panel panel_Top;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_orginalCollectionName;
        private System.Windows.Forms.TextBox textBox_newCollectionName;
        public System.Windows.Forms.Button button_cancel;
    }
}
