namespace CollectionManager.Core.Interfaces;

public interface ILogger
{
    void Log(string logMessage, params string[] vals);
}