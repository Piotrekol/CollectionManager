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

        float Volume { get; }
        int Position { get; set; }
        bool IsMusicPlayerMode { get; }
        bool IsAutoPlayEnabled { get; }
        bool IsDTEnabled { get; }
        bool IsUserSeeking { get; }

        void disableDt();

    }
}