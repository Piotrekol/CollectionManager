﻿namespace GuiComponents.Controls;

using CollectionManager.Common.Interfaces.Controls;
using System;
using System.Windows.Forms;

public partial class MusicControlView : UserControl, IMusicControlView
{
    public float Volume
    {
        get => trackBar_Volume.Value / 100f;
        set => trackBar_Volume.Value = Convert.ToInt32(value * 100f);
    }

    public int Position
    {
        get => trackBar_position.Value;
        set
        {
            if (IsHandleCreated)
            {
                try
                {
                    _ = Invoke((MethodInvoker)(() => trackBar_position.Value = value));
                }
                catch { }
            }
        }
    }
    public bool IsMusicPlayerMode
    {
        get => checkBox_musicPlayer.Checked;
        set => checkBox_musicPlayer.Checked = value;
    }

    public bool IsAutoPlayEnabled
    {
        get => checkBox_autoPlay.Checked;
        set => checkBox_autoPlay.Checked = value;
    }

    public bool IsDTEnabled => checkBox_DT.Checked;
    public bool IsUserSeeking { get; set; }
    private bool DTIsAvaliable = true;

    public event EventHandler<int> PositionChanged;
    public event EventHandler CheckboxChanged;
    public event EventHandler PlayPressed;
    public event EventHandler PausePressed;
    public event EventHandler<float> VolumeChanged;

    public MusicControlView()
    {
        InitializeComponent();
        Bind();
    }

    public void Bind()
    {
        checkBox_DT.CheckedChanged += CheckBoxChanged;
        checkBox_autoPlay.CheckedChanged += CheckBoxChanged;
        checkBox_musicPlayer.CheckedChanged += CheckBoxChanged;

        trackBar_Volume.ValueChanged += TrackBarVolumeChanged;
        trackBar_position.MouseDown += TrackBar_position_MouseDown;
        trackBar_position.MouseUp += TrackBarPositionOnMouseUp;
        trackBar_position.Scroll += TrackBarPositionOnScroll;

        button_StartPreview.Click += (s, a) => OnPlayPressed();
        button_StopPreview.Click += (s, a) => OnPausePressed();
        HandleCreated += OnHandleCreated;
    }

    private void OnHandleCreated(object sender, EventArgs eventArgs)
    {
        if (!DTIsAvaliable)
        {
            disableDt();
        }
    }

    private void TrackBarPositionOnScroll(object sender, EventArgs eventArgs) => PositionChanged?.Invoke(this, Position);

    private void TrackBarPositionOnMouseUp(object sender, MouseEventArgs mouseEventArgs)
    {
        PositionChanged?.Invoke(this, Position);
        IsUserSeeking = false;
    }

    private void TrackBar_position_MouseDown(object sender, MouseEventArgs e) => IsUserSeeking = true;

    private void TrackBarVolumeChanged(object sender, EventArgs e) => VolumeChanged?.Invoke(this, Volume);

    private void CheckBoxChanged(object sender, EventArgs eventArgs) => CheckboxChanged?.Invoke(this, null);

    public void disableDt()
    {
        DTIsAvaliable = false;
        if (IsHandleCreated)
        {
            _ = Invoke((MethodInvoker)(() =>
           {
               checkBox_DT.Enabled = false;
               checkBox_DT.Checked = false;
               checkBox_DT.Visible = false;
           }));
        }
    }

    protected virtual void OnPlayPressed() => PlayPressed?.Invoke(this, EventArgs.Empty);

    protected virtual void OnPausePressed() => PausePressed?.Invoke(this, EventArgs.Empty);
}
