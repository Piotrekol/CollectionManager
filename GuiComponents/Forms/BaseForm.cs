using System;
using System.Drawing;
using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Forms
{
    public class BaseForm : Form, IForm
    {
        protected BaseForm()
        {
            FormClosing += (s, a) => Closing?.Invoke(this, EventArgs.Empty);
            StartPosition = FormStartPosition.CenterParent;
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }
        public void ShowAndBlock()
        {
            this.ShowDialog();
        }

        public event EventHandler Closing;
    }
}