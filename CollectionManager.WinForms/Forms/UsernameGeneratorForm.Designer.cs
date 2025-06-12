namespace GuiComponents.Forms
{
    partial class UsernameGeneratorForm
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
            usernameGeneratorView1 = new GuiComponents.Controls.UsernameGeneratorView();
            SuspendLayout();
            // 
            // usernameGeneratorView1
            // 
            usernameGeneratorView1.Dock = System.Windows.Forms.DockStyle.Fill;
            usernameGeneratorView1.Location = new System.Drawing.Point(0, 0);
            usernameGeneratorView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            usernameGeneratorView1.Name = "usernameGeneratorView1";
            usernameGeneratorView1.Size = new System.Drawing.Size(446, 301);
            usernameGeneratorView1.Status = null;
            usernameGeneratorView1.TabIndex = 0;
            // 
            // UsernameGeneratorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(446, 301);
            Controls.Add(usernameGeneratorView1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "UsernameGeneratorForm";
            Text = "Collection Manager - Username generator";
            ResumeLayout(false);

        }

        #endregion

        private Controls.UsernameGeneratorView usernameGeneratorView1;
    }
}