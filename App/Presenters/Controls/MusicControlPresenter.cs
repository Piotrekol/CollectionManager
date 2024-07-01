using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Timers;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.Utils;
using App.Interfaces;
using App.Properties;
using Gui.Misc;
using GuiComponents.Interfaces;
using MusicPlayer;
using Timer = System.Timers.Timer;
using System.Runtime.InteropServices;

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

            var settings = Settings.Default;
            _view.IsAutoPlayEnabled = settings.Audio_autoPlay;
            _view.IsMusicPlayerMode = settings.Audio_playerMode;

            _view.CheckboxChanged += ViewOnCheckboxChanged;

            _view.Volume = settings.Audio_volume;

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

            Settings.Default.Audio_autoPlay = _view.IsAutoPlayEnabled;
            Settings.Default.Audio_playerMode = _view.IsMusicPlayerMode;
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
            Settings.Default.Audio_volume = floatEventArgs.Value;
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
            if (ShouldSkipTrack(map, audioLocation))
            {
                //Run as worker to avoid eventual stack overflow exception (eg. too many maps with no audio file in a row)
                RunAsWorker(() => _model.EmitNextMapRequest());
                return;
            }
            _lastAudioFileLocation = audioLocation;
            PlayBeatmap(map);
        }

        private bool ShouldSkipTrack(Beatmap map, string audioLocation)
        {
            //Skip repeating _audio files_(diferent diffs in same mapset) whenever _playing_ in _music player mode_
            if (_view.IsMusicPlayerMode && _lastAudioFileLocation == audioLocation && !musicPlayer.IsPlaying)
                return true;

            //Play if we have _audio file_ localy
            if (File.Exists(audioLocation))
                return false;

            //Skip since we don't have local audio file in _music player mode_
            if (_view.IsMusicPlayerMode)
                return true;

            //this is user-invoked play request (_music player mode_ check above)
            //Is there a chance that preview of this beatmap exists in online source?
            return map.MapSetId <= 20;
        }

        private void PlayBeatmap(Beatmap map)
        {
            var audioLocation = ((BeatmapExtension)map).FullAudioFileLocation();
            bool onlineSource = false;
            if (audioLocation == string.Empty)
            {
                if (map.MapSetId <= 20)
                    return;

                onlineSource = true;
                audioLocation = $@"https://b.ppy.sh/preview/{map.MapSetId}.mp3";
            }

            RunAsWorker(() =>
            {
                resetEvent.WaitOne(250);
                if (map.Equals(_model.CurrentBeatmap))
                {
                    try
                    {
                        musicPlayer.Play(
                            audioLocation,
                            _view.IsMusicPlayerMode || onlineSource ? 0 : Convert.ToInt32(Math.Round(map.PreviewTime / 1000f)));
                    }
                    catch (COMException)
                    {
                        musicPlayer.Pause();

                        if (_view.IsMusicPlayerMode)
                        {
                            RunAsWorker(() => _model.EmitNextMapRequest());
                        }
                    }
                }
            });
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