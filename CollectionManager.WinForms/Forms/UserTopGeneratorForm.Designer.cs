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
            userTopGenerator1 = new GuiComponents.Controls.UserTopGeneratorView();
            SuspendLayout();
            // 
            // userTopGenerator1
            // 
            userTopGenerator1.Dock = System.Windows.Forms.DockStyle.Fill;
            userTopGenerator1.Location = new System.Drawing.Point(0, 0);
            userTopGenerator1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userTopGenerator1.Name = "userTopGenerator1";
            userTopGenerator1.Size = new System.Drawing.Size(614, 786);
            userTopGenerator1.TabIndex = 0;
            // 
            // UserTopGeneratorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(614, 786);
            Controls.Add(userTopGenerator1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "UserTopGeneratorForm";
            Text = "Collection Manager - Collection Generator";
            ResumeLayout(false);

        }

        #endregion

        private Controls.UserTopGeneratorView userTopGenerator1;
    }
}