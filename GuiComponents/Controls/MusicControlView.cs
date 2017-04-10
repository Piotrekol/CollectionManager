using System;
using System.Windows.Forms;
using Gui.Misc;
using GuiComponents.Interfaces;

namespace GuiComponents.Controls
{
    public partial class MusicControlView : UserControl, IMusicControlView
    {
        public float Volume => trackBar_Volume.Value / 100f;
        public int Position
        {
            get { return trackBar_position.Value; }
            set
            {
                if (IsHandleCreated)
                    try
                    {
                        Invoke((MethodInvoker)(() =>
                       {
                           trackBar_position.Value = value;
                       }));
                    }
                    catch { }
            }
        }
        public bool IsMusicPlayerMode => checkBox_musicPlayer.Checked;
        public bool IsAutoPlayEnabled => checkBox_autoPlay.Checked;
        public bool IsDTEnabled => checkBox_DT.Checked;
        public bool IsUserSeeking { get; set; }
        private bool DTIsAvaliable = true;

        public event EventHandler<IntEventArgs> PositionChanged;
        public event EventHandler CheckboxChanged;
        public event EventHandler PlayPressed;
        public event EventHandler PausePressed;
        public event EventHandler<FloatEventArgs> VolumeChanged;



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
                disableDt();
        }

        private void TrackBarPositionOnScroll(object sender, EventArgs eventArgs)
        {
            PositionChanged?.Invoke(this, new IntEventArgs(Position));
        }

        private void TrackBarPositionOnMouseUp(object sender, MouseEventArgs mouseEventArgs)
        {
            PositionChanged?.Invoke(this, new IntEventArgs(Position));
            IsUserSeeking = false;
        }

        private void TrackBar_position_MouseDown(object sender, MouseEventArgs e)
        {
            IsUserSeeking = true;
        }

        private void TrackBarVolumeChanged(object sender, EventArgs e)
        {
            VolumeChanged?.Invoke(this, new FloatEventArgs(Volume));
        }

        private void CheckBoxChanged(object sender, EventArgs eventArgs)
        {
            CheckboxChanged?.Invoke(this, null);
        }

        public void disableDt()
        {
            DTIsAvaliable = false;
            if (IsHandleCreated)
                Invoke((MethodInvoker)(() =>
               {
                   checkBox_DT.Enabled = false;
                   checkBox_DT.Checked = false;
                   checkBox_DT.Visible = false;
               }));
        }

        protected virtual void OnPlayPressed()
        {
            PlayPressed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPausePressed()
        {
            PausePressed?.Invoke(this, EventArgs.Empty);
        }
    }
}
