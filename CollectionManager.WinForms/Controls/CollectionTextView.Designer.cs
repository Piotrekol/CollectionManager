namespace GuiComponents.Controls
{
    partial class CollectionTextView
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
            textBox1 = new System.Windows.Forms.TextBox();
            comboBox_textType = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox1.Location = new System.Drawing.Point(0, 35);
            textBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox1.Size = new System.Drawing.Size(363, 373);
            textBox1.TabIndex = 0;
            // 
            // comboBox_textType
            // 
            comboBox_textType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox_textType.FormattingEnabled = true;
            comboBox_textType.Location = new System.Drawing.Point(84, 3);
            comboBox_textType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBox_textType.Name = "comboBox_textType";
            comboBox_textType.Size = new System.Drawing.Size(178, 23);
            comboBox_textType.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(4, 7);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(70, 15);
            label1.TabIndex = 2;
            label1.Text = "Text format:";
            // 
            // CollectionTextView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label1);
            Controls.Add(comboBox_textType);
            Controls.Add(textBox1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CollectionTextView";
            Size = new System.Drawing.Size(364, 408);
            VisibleChanged += CollectionTextView_VisibleChanged;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox_textType;
        private System.Windows.Forms.Label label1;
    }
}
