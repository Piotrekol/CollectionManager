using System.Timers;
using Collection_Editor.code.Modules.MusicPlayer.SoundTouch;
using NAudio.Wave;

namespace MusicPlayer
{
    internal class SoundTouchPlayer : AudioPlayer
    {
        public override bool IsSpeedControlAvaliable => true;
        private VarispeedSampleProvider _speedControl;
        private readonly SoundTouchProfile _soundTouchProfile = new SoundTouchProfile(true, false);
        private float playbackSpeed = 1f;
        private readonly Timer _timer;

        internal SoundTouchPlayer()
        {
            _timer = new Timer(500);
            _timer.Elapsed += Timer_Elapsed;
        }
        public override void SetSpeed(float speed)
        {
            playbackSpeed = speed;
            lock (_lockingObject)
            {
                if (_speedControl != null)
                    _speedControl.PlaybackRate = speed;
            }
        }

        public override void Play(string audioFile, int startTime)
        {
            var audioReader = CreateAudioReader(audioFile);
            SetAudioReader(audioReader);
            SetSpeedReader(audioReader);

            Play(startTime);
        }

        protected override void Play(int startTime)
        {
            lock (_lockingObject)
            {
                if (_audioFileReader == null)
                    return;
                StopPlayback();
                _speedControl.PlaybackRate = playbackSpeed;
                _waveOutDevice.Init(_speedControl);
                _audioFileReader.Volume = SoundVolume;
                _audioFileReader.Skip(startTime);
                _waveOutDevice.Play();

                _playbackAborted = false;
                _timer.Start();
            }
        }

        private void SetSpeedReader(AudioFileReader audio)
        {
            lock (_lockingObject)
            {
                _speedControl = new VarispeedSampleProvider(audio, 100, _soundTouchProfile);
            }
        }
        private double LastTime = 0.0;
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //For some reason VarispeedSampleProvider doesn't ever "finish" playing,
            //so we have to workaround it. (dirty and I don't like it, but it works)
            var delta = TotalTime - CurrentTime;
            if (CurrentTime >= TotalTime || (LastTime.Equals(CurrentTime) && delta < 0.16))
            {
                _timer?.Stop();
                _waveOutDevice?.Stop();
            }
            LastTime = CurrentTime;
        }
    }
}