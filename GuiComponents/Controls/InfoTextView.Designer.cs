namespace GuiComponents.Controls
{
    partial class InfoTextView
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
            label_UpdateText = new System.Windows.Forms.Label();
            label_CollectionManagerStatus = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // label_UpdateText
            // 
            label_UpdateText.AutoSize = true;
            label_UpdateText.Location = new System.Drawing.Point(1, 0);
            label_UpdateText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_UpdateText.Name = "label_UpdateText";
            label_UpdateText.Size = new System.Drawing.Size(82, 15);
            label_UpdateText.TabIndex = 13;
            label_UpdateText.Text = "<UpdateText>";
            // 
            // label_CollectionManagerStatus
            // 
            label_CollectionManagerStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label_CollectionManagerStatus.Location = new System.Drawing.Point(212, 0);
            label_CollectionManagerStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_CollectionManagerStatus.Name = "label_CollectionManagerStatus";
            label_CollectionManagerStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label_CollectionManagerStatus.Size = new System.Drawing.Size(966, 20);
            label_CollectionManagerStatus.TabIndex = 10;
            label_CollectionManagerStatus.Text = "<CollectionManagerStatus>";
            // 
            // InfoTextView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label_CollectionManagerStatus);
            Controls.Add(label_UpdateText);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "InfoTextView";
            Size = new System.Drawing.Size(1178, 20);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_UpdateText;
        private System.Windows.Forms.Label label_CollectionManagerStatus;
    }
}
