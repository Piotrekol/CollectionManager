using Common;

namespace GuiComponents;

internal class CommonModelFilter(ICommonModelFilter filter) : BrightIdeasSoftware.IModelFilter
{
    public bool Filter(object modelObject) 
        => filter.Filter(modelObject);
}
