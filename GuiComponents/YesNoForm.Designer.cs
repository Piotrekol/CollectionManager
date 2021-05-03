namespace GuiComponents
{
    partial class YesNoForm
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
            this.label_text = new System.Windows.Forms.Label();
            this.checkBox_doNotAskAgain = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_text
            // 
            this.label_text.Location = new System.Drawing.Point(14, 15);
            this.label_text.MaximumSize = new System.Drawing.Size(415, 91);
            this.label_text.Name = "label_text";
            this.label_text.Size = new System.Drawing.Size(415, 81);
            this.label_text.TabIndex = 3;
            this.label_text.Text = "Text\r\n\r\nText\r\n\r\nText\r\n\r\nText\r\n";
            this.label_text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox_doNotAskAgain
            // 
            this.checkBox_doNotAskAgain.AutoSize = true;
            this.checkBox_doNotAskAgain.Location = new System.Drawing.Point(15, 110);
            this.checkBox_doNotAskAgain.Name = "checkBox_doNotAskAgain";
            this.checkBox_doNotAskAgain.Size = new System.Drawing.Size(127, 19);
            this.checkBox_doNotAskAgain.TabIndex = 2;
            this.checkBox_doNotAskAgain.Text = "Don\'t ask me again";
            this.checkBox_doNotAskAgain.UseVisualStyleBackColor = true;
            this.checkBox_doNotAskAgain.CheckedChanged += new System.EventHandler(this.CheckBox_doNotAskAgain_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.No;
            this.button2.Location = new System.Drawing.Point(313, 136);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 27);
            this.button2.TabIndex = 1;
            this.button2.Text = "No";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.button1.Location = new System.Drawing.Point(14, 136);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "Yes";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // YesNoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(443, 177);
            this.Controls.Add(this.label_text);
            this.Controls.Add(this.checkBox_doNotAskAgain);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YesNoForm";
            this.Text = "Collection Manager - YesNoForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.CheckBox checkBox_doNotAskAgain;
        public System.Windows.Forms.Label label_text;
    }
}