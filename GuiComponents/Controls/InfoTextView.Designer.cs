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
            this.label_UpdateText = new System.Windows.Forms.Label();
            this.label_CollectionManagerStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_UpdateText
            // 
            this.label_UpdateText.AutoSize = true;
            this.label_UpdateText.Location = new System.Drawing.Point(1, 0);
            this.label_UpdateText.Name = "label_UpdateText";
            this.label_UpdateText.Size = new System.Drawing.Size(75, 13);
            this.label_UpdateText.TabIndex = 13;
            this.label_UpdateText.Text = "<UpdateText>";
            // 
            // label_CollectionManagerStatus
            // 
            this.label_CollectionManagerStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_CollectionManagerStatus.Location = new System.Drawing.Point(182, 0);
            this.label_CollectionManagerStatus.Name = "label_CollectionManagerStatus";
            this.label_CollectionManagerStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_CollectionManagerStatus.Size = new System.Drawing.Size(828, 17);
            this.label_CollectionManagerStatus.TabIndex = 10;
            this.label_CollectionManagerStatus.Text = "<CollectionManagerStatus>";
            // 
            // InfoTextView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_CollectionManagerStatus);
            this.Controls.Add(this.label_UpdateText);
            this.Name = "InfoTextView";
            this.Size = new System.Drawing.Size(1010, 17);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_UpdateText;
        private System.Windows.Forms.Label label_CollectionManagerStatus;
    }
}
