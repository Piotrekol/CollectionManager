namespace GuiComponents.Forms;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.WinForms.Forms;

public partial class StartupForm : BaseForm, IStartupForm
{
    public StartupForm()
    {
        InitializeComponent();
    }

    public IStartupView StartupView => startupView1;
}
