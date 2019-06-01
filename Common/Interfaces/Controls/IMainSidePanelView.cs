using System;
using Gui.Misc;

namespace GuiComponents.Interfaces
{
    public interface IMainSidePanelView
    {
        event GuiHelpers.SidePanelActionsHandlerArgs SidePanelOperation;
    }
}