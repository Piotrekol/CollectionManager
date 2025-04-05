namespace GuiComponents.Forms;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.WinForms.Forms;

public partial class UsernameGeneratorForm : BaseForm, IUsernameGeneratorForm
{
    public UsernameGeneratorForm()
    {
        InitializeComponent();
    }

    public IUsernameGeneratorView view => usernameGeneratorView1;
}
