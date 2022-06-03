using GuiComponents.Interfaces;
using System;
using System.ComponentModel;

namespace GuiComponents.Forms
{
    public partial class StartupForm : BaseForm, IStartupForm
    {
        public StartupForm()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        public IStartupView StartupView => startupView1;

        event EventHandler IForm.Closing
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public void ShowAndBlock()
        {
            this.ShowDialog();
        }
    }
}
