namespace CollectionManager.WinForms;

partial class TextBoxForm
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
        richTextBox_contents = new System.Windows.Forms.RichTextBox();
        SuspendLayout();
        // 
        // richTextBox_contents
        // 
        richTextBox_contents.BorderStyle = System.Windows.Forms.BorderStyle.None;
        richTextBox_contents.Dock = System.Windows.Forms.DockStyle.Fill;
        richTextBox_contents.Location = new System.Drawing.Point(0, 0);
        richTextBox_contents.Name = "richTextBox_contents";
        richTextBox_contents.ReadOnly = true;
        richTextBox_contents.Size = new System.Drawing.Size(1065, 948);
        richTextBox_contents.TabIndex = 0;
        richTextBox_contents.Text = "text\n\ntext\n\ntext";
        // 
        // TextBoxForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1065, 948);
        Controls.Add(richTextBox_contents);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "TextBoxForm";
        Text = "TextBoxForm";
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.RichTextBox richTextBox_contents;
}