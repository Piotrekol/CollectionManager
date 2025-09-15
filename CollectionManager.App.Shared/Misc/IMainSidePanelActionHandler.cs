namespace CollectionManager.App.Shared.Misc;

using CollectionManager.Common;
using System.Threading.Tasks;

public interface IMainSidePanelActionHandler
{
    public MainSidePanelActions Action { get; }
    public Task HandleAsync(object sender, object data);
}
