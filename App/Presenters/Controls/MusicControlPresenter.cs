using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Timers;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.Utils;
using App.Interfaces;
using Gui.Misc;
using GuiComponents.Interfaces;
using MusicPlayer;
using Timer = System.Timers.Timer;

namespace App.Presenters.Controls
{
    public class MusicControlPresenter : IDisposable
    {
        private IMusicControlView _view;
        private readonly IMusicControlModel _model;
        private IMusicPlayer musicPlayer = new MusicPlayerManager();
        private Timer trackPositionTimer;
        private AutoResetEvent resetEvent = new AutoResetEvent(false);
        private string _lastAudioFileLocation = string.Empty;
        public MusicControlPresenter(IMusicControlView view, IMusicControlModel model)
        {
            _view = view;
            _view.PositionChanged += ViewOnPositionChanged;
            _view.VolumeChanged += ViewOnVolumeChanged;
            _view.PlayPressed += ViewOnPlayPressed;
            _view.PausePressed += ViewOnPausePressed;
            _view.Disposed += (s, a) => this.Dispose();
            if (!musicPlayer.IsSpeedControlAvaliable)
            {
                _view.disableDt();
            }
            _view.CheckboxChanged += ViewOnCheckboxChanged;

            trackPositionTimer = new Timer(500);
            trackPositionTimer.Elapsed += TrackPositionTimer_Elapsed;
            trackPositionTimer.Start();

            _model = model;
            _model.BeatmapChanged += ModelOnBeatmapChanged;
            musicPlayer.PlaybackFinished += MusicPlayer_PlaybackFinished;
        }

        private void ViewOnCheckboxChanged(object sender, EventArgs eventArgs)
        {
            if (musicPlayer.IsSpeedControlAvaliable)
            {
                musicPlayer.SetSpeed(_view.IsDTEnabled ? 1.5f : 1.0f);
            }



        }


        public void Dispose()
        {
            trackPositionTimer.Stop();
            trackPositionTimer.Dispose();
            musicPlayer.Dispose();
        }


        private void MusicPlayer_PlaybackFinished(object sender, EventArgs e)
        {
            if (_view.IsMusicPlayerMode)
                _model.EmitNextMapRequest();
        }

        private void ViewOnPausePressed(object sender, EventArgs eventArgs)
        {
            musicPlayer.Pause();
        }

        private void ViewOnPlayPressed(object sender, EventArgs eventArgs)
        {
            if (_model.CurrentBeatmap != null)
            {
                if (musicPlayer.IsPaused)
                    musicPlayer.Resume();
                else
                    PlayBeatmap(_model.CurrentBeatmap);
            }
        }

        private void ViewOnVolumeChanged(object sender, FloatEventArgs floatEventArgs)
        {
            musicPlayer.SetVolume(floatEventArgs.Value);
        }

        private void ViewOnPositionChanged(object sender, IntEventArgs intEventArgs)
        {
            double pos = (_view.Position / 100d) * musicPlayer.TotalTime;
            musicPlayer.Seek(pos);
        }

        private void ModelOnBeatmapChanged(object sender, EventArgs eventArgs)
        {
            var map = _model.CurrentBeatmap;
            if (map == null || !_view.IsAutoPlayEnabled)
                return;
            var audioLocation = ((BeatmapExtension)map).FullAudioFileLocation();
            if (ShouldSkipTrack(audioLocation))
            {
                //Run as worker to avoid eventual stack overflow exception (eg. too many maps with no audio file in a row)
                RunAsWorker(() => _model.EmitNextMapRequest());
                return;
            }
            _lastAudioFileLocation = audioLocation;
            PlayBeatmap(map);
        }

        private bool ShouldSkipTrack(string audioLocation)
        {
            if ((_lastAudioFileLocation == audioLocation || string.IsNullOrWhiteSpace(audioLocation)) &&
                _view.IsMusicPlayerMode && !musicPlayer.IsPlaying)
                return true;

            return !File.Exists(audioLocation);
        }
        private void PlayBeatmap(Beatmap map)
        {
            var audioLocation = ((BeatmapExtension)map).FullAudioFileLocation();
            if (audioLocation != string.Empty)
            {
                RunAsWorker(() =>
                {
                    resetEvent.WaitOne(250);
                    if (map.Equals(_model.CurrentBeatmap))
                    {
                        musicPlayer.Play(audioLocation,
                            _view.IsMusicPlayerMode ? 0 : Convert.ToInt32(Math.Round(map.PreviewTime / 1000f)));
                    }
                });
            }
        }
        private void RunAsWorker(Action action)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, ee) =>
            {
                action();
            };
            bw.RunWorkerAsync();
        }
        private void TrackPositionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_view.IsUserSeeking) return;
            int precentagePosition = Convert.ToInt32((musicPlayer.CurrentTime / musicPlayer.TotalTime) * 100);
            _view.Position = precentagePosition;
        }

    }
}