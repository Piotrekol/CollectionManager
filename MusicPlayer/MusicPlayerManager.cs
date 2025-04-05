using Common;
using System;
using System.IO;

namespace MusicPlayer
{
    public class MusicPlayerManager : IMusicPlayer
    {

        public bool IsPlaying => musicPlayer.IsPlaying;
        public double TotalTime => musicPlayer.TotalTime;
        public double CurrentTime => musicPlayer.CurrentTime;
        public bool IsSpeedControlAvaliable => musicPlayer.IsSpeedControlAvaliable;
        public bool IsPaused => musicPlayer.IsPaused;
        public float SoundVolume => musicPlayer.SoundVolume;

        private readonly IMusicPlayer musicPlayer;
        private readonly MRUFileCache fileCache;

        public event EventHandler PlaybackFinished
        {
            add { musicPlayer.PlaybackFinished += value; }
            remove { musicPlayer.PlaybackFinished -= value; }
        }

        public MusicPlayerManager()
        {
            if (File.Exists("SoundTouch.dll") && File.Exists("SoundTouch_x64.dll"))
                musicPlayer = new SoundTouchPlayer();
            else
                musicPlayer = new NAudioPlayer();

            fileCache = new MRUFileCache(Path.Combine(Path.GetTempPath(), "CM-audio-previews"));
        }
        public void Seek(double position)
        {
            musicPlayer.Seek(position);
        }

        public void SetVolume(float volume)
        {
            musicPlayer.SetVolume(volume);
        }

        public void SetSpeed(float speed)
        {
            musicPlayer.SetSpeed(speed);
        }

        public void Play(string audioFileOrUrl, int startTime, ReaderType readerType)
        {
            Uri uri = new Uri(audioFileOrUrl);

            if (uri.HostNameType == UriHostNameType.Dns)
                audioFileOrUrl = fileCache.DownloadAndAdd(audioFileOrUrl);

            if (string.IsNullOrEmpty(audioFileOrUrl))
                return;

            musicPlayer.Play(audioFileOrUrl, startTime, readerType);
        }

        public void Pause()
        {
            musicPlayer.Pause();
        }

        public void Resume()
        {
            musicPlayer.Resume();
        }

        public void Dispose()
        {
            musicPlayer.Dispose();
        }
    }
}
