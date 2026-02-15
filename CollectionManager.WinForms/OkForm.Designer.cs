namespace GuiComponents
{
    partial class OkForm
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
            label_text = new System.Windows.Forms.Label();
            checkBox_doNotAskAgain = new System.Windows.Forms.CheckBox();
            button_Ok = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label_text
            // 
            label_text.Location = new System.Drawing.Point(14, 15);
            label_text.MaximumSize = new System.Drawing.Size(415, 91);
            label_text.Name = "label_text";
            label_text.Size = new System.Drawing.Size(415, 81);
            label_text.TabIndex = 1;
            label_text.Text = "Text\r\n\r\nText\r\n\r\nText\r\n\r\nText\r\n";
            label_text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox_doNotAskAgain
            // 
            checkBox_doNotAskAgain.AutoSize = true;
            checkBox_doNotAskAgain.Location = new System.Drawing.Point(15, 110);
            checkBox_doNotAskAgain.Name = "checkBox_doNotAskAgain";
            checkBox_doNotAskAgain.Size = new System.Drawing.Size(229, 19);
            checkBox_doNotAskAgain.TabIndex = 2;
            checkBox_doNotAskAgain.Text = "Don't inform me again in this session";
            checkBox_doNotAskAgain.UseVisualStyleBackColor = true;
            checkBox_doNotAskAgain.CheckedChanged += CheckBox_doNotAskAgain_CheckedChanged;
            // 
            // button_Ok
            // 
            button_Ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            button_Ok.Location = new System.Drawing.Point(163, 136);
            button_Ok.Name = "button_Ok";
            button_Ok.Size = new System.Drawing.Size(117, 27);
            button_Ok.TabIndex = 0;
            button_Ok.Text = "OK";
            button_Ok.UseVisualStyleBackColor = true;
            // 
            // OkForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(443, 177);
            Controls.Add(label_text);
            Controls.Add(checkBox_doNotAskAgain);
            Controls.Add(button_Ok);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OkForm";
            Text = "Collection Manager - OkForm";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Ok;
        public System.Windows.Forms.CheckBox checkBox_doNotAskAgain;
        public System.Windows.Forms.Label label_text;
    }
}
