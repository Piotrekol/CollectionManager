using GuiComponents.Interfaces;
using System.Windows.Forms;

namespace GuiComponents.Forms
{
    public partial class StartupForm : BaseForm, IStartupForm
    {
        public StartupForm()
        {
            InitializeComponent();
        }

        public IStartupView StartupView => startupView1;
    }
}
