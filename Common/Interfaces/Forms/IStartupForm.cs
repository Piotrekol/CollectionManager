using System;
using System.Collections.Generic;
using Common.Interfaces;

namespace GuiComponents.Interfaces
{
    public interface IStartupForm : IForm
    {
        IStartupView StartupView { get; }
    }
}