namespace CollectionManager.Common.Interfaces.Forms;

using CollectionManager.Common.Interfaces.Controls;

public interface IUsernameGeneratorForm : IForm
{
    IUsernameGeneratorView view { get; }
}