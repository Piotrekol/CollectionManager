namespace CollectionManager.App.Shared.Misc;

using CollectionManager.Common.Interfaces;

public static class ResourceStrings
{
    private static string GeneralHelp => CollectionManager_App_Shared_Resources.GeneralHelp.Replace("\\t", "\t");
    public static void GeneralHelpDialog(IUserDialogs userDialogs) => userDialogs.TextMessageBox(GeneralHelp, "Beatmap search syntax help");
}