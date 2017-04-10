using NAudio.Wave;

namespace MusicPlayer
{
    public class AudioFileReaderEx : AudioFileReader
    {
        public AudioFileReaderEx(string fileName) : base(fileName)
        {
            AudioFileLocation = fileName;
        }

        public string AudioFileLocation { get; }
        public bool Reused { get; private set; } = false;
        public AudioFileReaderEx GetAudio(string fileName)
        {
            if (fileName == AudioFileLocation)
            {
                this.Reused = true;
                this.SetPosition(0d);
                return this;
            }
            return new AudioFileReaderEx(fileName);
        }
    }
}