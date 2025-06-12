namespace CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Common.Interfaces.Controls;
public interface IUserTopGeneratorForm : IForm
{
    IUserTopGenerator UserTopGeneratorView { get; }
}