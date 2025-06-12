namespace CollectionManager.Audio;

using System;

public interface IMusicPlayer : IDisposable
{
    void Seek(double position);
    void SetVolume(float volume);
    void SetSpeed(float speed);
    void Play(string audioFileOrUrl, int startTime, ReaderType readerType);
    void Pause();
    void Resume();
    bool IsPlaying { get; }
    double TotalTime { get; }
    double CurrentTime { get; }
    bool IsSpeedControlAvaliable { get; }
    bool IsPaused { get; }
    float SoundVolume { get; }
    event EventHandler PlaybackFinished;
}