using Common.Interfaces.Controls;

namespace GuiComponents.Interfaces
{
    public interface IUsernameGeneratorForm :IForm
    {
        IUsernameGeneratorView view { get; }
    }
}