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
            FormClosing += (s, e) => Closing?.Invoke(this, e);
            StartPosition = FormStartPosition.CenterParent;
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Font = new Font("Segoe UI", 9f);
        }

        public void ShowAndBlock()
        {
            ShowDialog();
        }

        public event EventHandler Closing;
    }
}