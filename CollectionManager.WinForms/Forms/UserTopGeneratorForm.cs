namespace GuiComponents.Forms;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.WinForms.Forms;

public partial class UserTopGeneratorForm : BaseForm, IUserTopGeneratorForm
{
    public UserTopGeneratorForm()
    {
        InitializeComponent();
    }

    public IUserTopGenerator UserTopGeneratorView => userTopGenerator1;
}
