using System;

namespace CollectionManagerExtensionsDll.Modules.BeatmapExporter;

public class BeatmapSetExportException : Exception
{
    public BeatmapSetExportException(string message, Exception innerException)
        : base(message, innerException) { }
}