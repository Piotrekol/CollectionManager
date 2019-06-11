using System;
using Gui.Misc;

namespace GuiComponents.Interfaces
{
    public interface IMusicControlView
    {
        event EventHandler<IntEventArgs> PositionChanged;
        event EventHandler CheckboxChanged;
        event EventHandler PlayPressed;
        event EventHandler PausePressed;
        event EventHandler<FloatEventArgs> VolumeChanged;
        event EventHandler Disposed;

        float Volume { get; set; }
        int Position { get; set; }
        bool IsMusicPlayerMode { get; set; }
        bool IsAutoPlayEnabled { get; set; }
        bool IsDTEnabled { get; }
        bool IsUserSeeking { get; }

        void disableDt();

    }
}