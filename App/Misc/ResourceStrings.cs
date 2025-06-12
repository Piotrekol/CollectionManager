namespace CollectionManagerApp.Misc;

using CollectionManager.Common.Interfaces;

public static class ResourceStrings
{
    public static string GeneralHelp => CollectionManager_Resources.GeneralHelp.Replace("\\t", "\t");
    public static void GeneralHelpDialog(IUserDialogs userDialogs) => userDialogs.TextMessageBox(CollectionManager_Resources.GeneralHelp.Replace("\\t", "\t"), "Beatmap search syntax help");
}