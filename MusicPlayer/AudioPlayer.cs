using NAudio.Wave;

namespace MusicPlayer
{
    internal abstract class AudioPlayer : IMusicPlayer
    {
        #region IMusicPlayer members
        public double CurrentTime => _audioFileReader?.WaveStream.CurrentTime.TotalSeconds ?? -1;
        public double TotalTime => _audioFileReader?.WaveStream.TotalTime.TotalSeconds ?? -1;

        public bool IsPlaying => _waveOutDevice?.PlaybackState == PlaybackState.Playing;

        public virtual bool IsSpeedControlAvaliable => false;
        public bool IsPaused => _waveOutDevice?.PlaybackState == PlaybackState.Paused;
        private float _soundVolume = 0.3f;
        protected bool _playbackAborted;
        public float SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                lock (_lockingObject)
                {
                    _soundVolume = value;
                    if (_waveOutDevice != null)
                        _waveOutDevice.Volume = value;
                }
            }
        }
        public event EventHandler PlaybackFinished;

        public void Seek(double position)
        {
            if (position > TotalTime)
                position = TotalTime;
            //Sound samples are read in 100ms intervals, so we need to leave atleast that much 
            //for everything to still work correctly after skipping part of song.
            //This ensures that atleast 150ms is left, assuming that position is within TotalTime.
            var enoughLeft = (TotalTime - position) < 0.15;
            if (enoughLeft)
                position -= 0.15;

            _audioFileReader.WaveStream.SetPosition(position);
        }

        public void SetVolume(float volume)
        {
            SoundVolume = volume;
        }

        public virtual void SetSpeed(float speed)
        {
            throw new NotImplementedException();
        }

        public virtual void Play(string audioFile, int startTime, ReaderType readerType)
        {
            lock (_lockingObject)
            {
                var audioReader = CreateAudioReader(audioFile, readerType);
                if (audioReader != null)
                {
                    SetAudioReader(audioReader);
                    Play(startTime);
                }
                else
                {
                    CleanAfterLastPlayback();
                    WaveOutDevice_PlaybackStopped(this, null);
                }
            }
        }
        public void Resume()
        {
            _waveOutDevice.Play();
        }
        public void Pause()
        {
            _waveOutDevice.Pause();
        }
        #endregion

        protected readonly object _lockingObject = new object();
        protected AudioFileReaderEx _audioFileReader;
        protected IWavePlayer _waveOutDevice;

        protected AudioFileReaderEx CreateAudioReader(string audioFile, ReaderType readerType) 
            => AudioFileReaderEx.Create(_audioFileReader, audioFile, readerType);

        protected void SetAudioReader(AudioFileReaderEx audioReader)
        {
            lock (_lockingObject)
            {
                if (!audioReader.Equals(_audioFileReader))
                    CleanAfterLastPlayback();

                _audioFileReader = audioReader;
                if (_audioFileReader.Reused)
                    return;

                _waveOutDevice = new WaveOutEvent();
                _waveOutDevice.PlaybackStopped += WaveOutDevice_PlaybackStopped;
            }
        }

        protected virtual void Play(int startTime)
        {
            if (_audioFileReader is null)
            {
                return;
            }

            StopPlayback();
            _playbackAborted = false;
            if (!_audioFileReader.Reused)
            {
                _waveOutDevice.Init(_audioFileReader.WaveStream);
            }
            _audioFileReader.WaveStream.Skip(startTime);
            _waveOutDevice.Volume = SoundVolume;
            _waveOutDevice.Play();

        }
        private void CleanAfterLastPlayback()
        {
            lock (_lockingObject)
            {
                if (_waveOutDevice != null)
                    StopPlayback();
                if (_audioFileReader != null)
                {
                    _audioFileReader.Dispose();
                    _audioFileReader = null;
                }
                if (_waveOutDevice != null)
                {
                    _waveOutDevice.PlaybackStopped -= WaveOutDevice_PlaybackStopped;
                    _waveOutDevice.Dispose();
                    _waveOutDevice = null;
                }
            }
            GC.Collect();
        }
        public void StopPlayback()
        {
            lock (_lockingObject)
            {
                if (_waveOutDevice.PlaybackState == PlaybackState.Playing)
                {
                    _playbackAborted = true;
                    var oldVolume = _waveOutDevice.Volume;
                    _waveOutDevice.Volume = 0f;
                    _waveOutDevice.Stop();
                    _waveOutDevice.Volume = oldVolume;
                }
            }
        }
        protected virtual void WaveOutDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (_playbackAborted)
                return;
            PlaybackFinished?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            CleanAfterLastPlayback();
        }
    }
}