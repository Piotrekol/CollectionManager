namespace CollectionManager.Audio;

using NAudio.Vorbis;
using NAudio.Wave;

public class AudioFileReaderEx : IDisposable
{
    public WaveStream WaveStream { get; init; }
    public ISampleProvider SampleProvider { get; init; }
    public string FileLocation { get; }
    public bool Reused { get; private set; }
    public string FileName { get; }

    public AudioFileReaderEx(string fileName, VorbisWaveReader vorbisWaveReader)
    {
        FileName = fileName;
        WaveStream = vorbisWaveReader;
        SampleProvider = vorbisWaveReader;
    }

    public AudioFileReaderEx(string fileName, AudioFileReader audioFileReader)
    {
        FileLocation = fileName;
        WaveStream = audioFileReader;
        SampleProvider = audioFileReader;
    }

    public static AudioFileReaderEx Create(AudioFileReaderEx reader, string fileName, ReaderType readerType)
    {
        if (reader is not null && fileName == reader.FileLocation)
        {
            reader.Reused = true;
            reader.WaveStream.SetPosition(0d);
            return reader;
        }

        if (readerType == ReaderType.Vorbis)
        {
            VorbisWaveReader vorbisReader = new(fileName);
            return new AudioFileReaderEx(fileName, vorbisReader);
        }

        return new AudioFileReaderEx(fileName, new AudioFileReader(fileName));
    }

    public void Dispose()
    {
        WaveStream?.Dispose();
        GC.SuppressFinalize(this);
    }
}