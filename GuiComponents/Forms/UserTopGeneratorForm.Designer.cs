namespace GuiComponents.Forms
{
    partial class UserTopGeneratorForm
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
            this.userTopGenerator1 = new GuiComponents.Controls.UserTopGeneratorView();
            this.SuspendLayout();
            // 
            // userTopGenerator1
            // 
            this.userTopGenerator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userTopGenerator1.Location = new System.Drawing.Point(0, 0);
            this.userTopGenerator1.Name = "userTopGenerator1";
            this.userTopGenerator1.Size = new System.Drawing.Size(526, 681);
            this.userTopGenerator1.TabIndex = 0;
            // 
            // UserTopGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 681);
            this.Controls.Add(this.userTopGenerator1);
            this.Name = "UserTopGeneratorForm";
            this.Text = "Collection Manager - Collection Generator";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.UserTopGeneratorView userTopGenerator1;
    }
}