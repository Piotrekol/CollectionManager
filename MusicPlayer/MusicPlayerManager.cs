using System;
using System.IO;
using System.Linq;
using System.Net;

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

        public void Play(string audioFile, int startTime)
        {
            Uri uri = new Uri(audioFile);

            if (uri.HostNameType == UriHostNameType.Dns)
            {
                var tempFolderPath = Path.Combine(Path.GetTempPath(), "CM-previews");
                Directory.CreateDirectory(tempFolderPath);
                var directoryInfo = new DirectoryInfo(tempFolderPath);
                var files = directoryInfo.GetFiles();
                if (files.Length > 100)
                {
                    foreach (var file in files.OrderBy(f => f.LastWriteTimeUtc).Take(30))
                    {
                        file.Delete();
                    }
                }

                var tempFilePath = Path.Combine(tempFolderPath, uri.Segments.Last());
                if (!File.Exists(tempFilePath))
                {
                    using (WebClient ws = new WebClient())
                    {
                        ws.DownloadFile(audioFile, tempFilePath);
                    }
                }

                audioFile = tempFilePath;
            }

            musicPlayer.Play(audioFile, startTime);
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
