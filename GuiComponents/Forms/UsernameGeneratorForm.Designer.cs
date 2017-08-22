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
            this.usernameGeneratorView1 = new GuiComponents.Controls.UsernameGeneratorView();
            this.SuspendLayout();
            // 
            // usernameGeneratorView1
            // 
            this.usernameGeneratorView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usernameGeneratorView1.Location = new System.Drawing.Point(0, 0);
            this.usernameGeneratorView1.Name = "usernameGeneratorView1";
            this.usernameGeneratorView1.Size = new System.Drawing.Size(382, 261);
            this.usernameGeneratorView1.Status = null;
            this.usernameGeneratorView1.TabIndex = 0;
            // 
            // UsernameGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 261);
            this.Controls.Add(this.usernameGeneratorView1);
            this.Name = "UsernameGeneratorForm";
            this.Text = "UsernameGeneratorForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.UsernameGeneratorView usernameGeneratorView1;
    }
}