using System;
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
        }
        public void ShowAndBlock()
        {
            this.ShowDialog();
        }

        public event EventHandler Closing;
    }
}