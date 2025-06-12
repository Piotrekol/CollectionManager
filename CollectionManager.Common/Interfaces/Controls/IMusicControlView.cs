namespace CollectionManager.Common.Interfaces.Controls;

using System;

public interface IMusicControlView
{
    event EventHandler<int> PositionChanged;
    event EventHandler CheckboxChanged;
    event EventHandler PlayPressed;
    event EventHandler PausePressed;
    event EventHandler<float> VolumeChanged;
    event EventHandler Disposed;

    float Volume { get; set; }
    int Position { get; set; }
    bool IsMusicPlayerMode { get; set; }
    bool IsAutoPlayEnabled { get; set; }
    bool IsDTEnabled { get; }
    bool IsUserSeeking { get; }

    void disableDt();

}