namespace CollectionManager.Extensions.Modules.Exporter;
using System;

public class BeatmapSetExportException : Exception
{
    public BeatmapSetExportException(string message, Exception innerException)
        : base(message, innerException) { }
}