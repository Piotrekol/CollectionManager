namespace CollectionManager.Common;
public static class ProcessExtensions
{
    public static Process OpenUrl(string url)
        => Process.Start(new ProcessStartInfo()
        {
            FileName = url,
            UseShellExecute = true
        });
}
