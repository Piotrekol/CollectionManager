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
            label_Error = new System.Windows.Forms.Label();
            panel_bottom = new System.Windows.Forms.Panel();
            button_cancel = new System.Windows.Forms.Button();
            button_rename = new System.Windows.Forms.Button();
            panel_Top = new System.Windows.Forms.Panel();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label_orginalCollectionName = new System.Windows.Forms.Label();
            textBox_newCollectionName = new System.Windows.Forms.TextBox();
            panel_bottom.SuspendLayout();
            panel_Top.SuspendLayout();
            SuspendLayout();
            // 
            // label_Error
            // 
            label_Error.AutoSize = true;
            label_Error.Location = new System.Drawing.Point(37, 5);
            label_Error.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_Error.Name = "label_Error";
            label_Error.Size = new System.Drawing.Size(137, 15);
            label_Error.TabIndex = 11;
            label_Error.Text = "This name already exists!";
            label_Error.Visible = false;
            // 
            // panel_bottom
            // 
            panel_bottom.Anchor = System.Windows.Forms.AnchorStyles.None;
            panel_bottom.Controls.Add(label_Error);
            panel_bottom.Controls.Add(button_cancel);
            panel_bottom.Controls.Add(button_rename);
            panel_bottom.Location = new System.Drawing.Point(36, 76);
            panel_bottom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel_bottom.Name = "panel_bottom";
            panel_bottom.Size = new System.Drawing.Size(219, 53);
            panel_bottom.TabIndex = 10;
            // 
            // button_cancel
            // 
            button_cancel.Location = new System.Drawing.Point(128, 23);
            button_cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button_cancel.Name = "button_cancel";
            button_cancel.Size = new System.Drawing.Size(88, 27);
            button_cancel.TabIndex = 5;
            button_cancel.Text = "Cancel";
            button_cancel.UseVisualStyleBackColor = true;
            // 
            // button_rename
            // 
            button_rename.DialogResult = System.Windows.Forms.DialogResult.OK;
            button_rename.Enabled = false;
            button_rename.Location = new System.Drawing.Point(6, 23);
            button_rename.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button_rename.Name = "button_rename";
            button_rename.Size = new System.Drawing.Size(88, 27);
            button_rename.TabIndex = 4;
            button_rename.Text = "Rename";
            button_rename.UseVisualStyleBackColor = true;
            // 
            // panel_Top
            // 
            panel_Top.Anchor = System.Windows.Forms.AnchorStyles.None;
            panel_Top.Controls.Add(label2);
            panel_Top.Controls.Add(label1);
            panel_Top.Controls.Add(label_orginalCollectionName);
            panel_Top.Controls.Add(textBox_newCollectionName);
            panel_Top.Location = new System.Drawing.Point(0, 0);
            panel_Top.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel_Top.MaximumSize = new System.Drawing.Size(296, 73);
            panel_Top.Name = "panel_Top";
            panel_Top.Size = new System.Drawing.Size(296, 73);
            panel_Top.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(20, 43);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(67, 15);
            label2.TabIndex = 1;
            label2.Text = "New name:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 8);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(82, 15);
            label1.TabIndex = 0;
            label1.Text = "Orginal name:";
            // 
            // label_orginalCollectionName
            // 
            label_orginalCollectionName.AutoSize = true;
            label_orginalCollectionName.Location = new System.Drawing.Point(94, 8);
            label_orginalCollectionName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_orginalCollectionName.Name = "label_orginalCollectionName";
            label_orginalCollectionName.Size = new System.Drawing.Size(33, 15);
            label_orginalCollectionName.TabIndex = 2;
            label_orginalCollectionName.Text = "from";
            // 
            // textBox_newCollectionName
            // 
            textBox_newCollectionName.Location = new System.Drawing.Point(98, 39);
            textBox_newCollectionName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox_newCollectionName.Name = "textBox_newCollectionName";
            textBox_newCollectionName.Size = new System.Drawing.Size(174, 23);
            textBox_newCollectionName.TabIndex = 3;
            // 
            // CollectionRenameView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel_bottom);
            Controls.Add(panel_Top);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CollectionRenameView";
            Size = new System.Drawing.Size(298, 129);
            panel_bottom.ResumeLayout(false);
            panel_bottom.PerformLayout();
            panel_Top.ResumeLayout(false);
            panel_Top.PerformLayout();
            ResumeLayout(false);

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
