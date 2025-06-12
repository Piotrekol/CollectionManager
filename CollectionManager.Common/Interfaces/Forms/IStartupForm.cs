namespace CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Common.Interfaces.Controls;
public interface IStartupForm : IForm
{
    IStartupView StartupView { get; }
}