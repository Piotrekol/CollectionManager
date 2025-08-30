namespace CollectionManager.App.Shared.Misc;

using CollectionManager.Common.Interfaces;

public static class ResourceStrings
{
    private static string GeneralHelp => CollectionManager_App_Shared_Resources.GeneralHelp.Replace("\\t", "\t");
    public static async Task GeneralHelpDialogAsync(IUserDialogs userDialogs) => await userDialogs.TextMessageBoxAsync(GeneralHelp, "Beatmap search syntax help");
}