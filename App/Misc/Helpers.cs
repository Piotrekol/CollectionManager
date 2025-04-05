namespace CollectionManagerApp.Misc;

using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.Downloader.Api;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Models.Controls;
using CollectionManagerApp.Presenters.Forms;
using System.Reflection;

public static class Helpers
{
    public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t != null);
        }
    }

    public static LoginData GetLoginData(this ILoginFormView loginForm,
        IReadOnlyList<IDownloadSource> downloadSources)
    {
        loginForm.SetDownloadSources(downloadSources);
        loginForm.ShowAndBlock();
        LoginData loginData = new();
        if (loginForm.ClickedLogin)
        {
            loginData.Username = loginForm.Login;
            loginData.Password = loginForm.Password;
            loginData.SiteCookies = loginForm.OsuCookies;
            loginData.DownloadSource = loginForm.DownloadSource;
        }

        return loginData;
    }

    public static string GetCollectionName(this ICollectionAddRenameForm form, Func<string, bool> isCollectionNameValid, string orginalName = "",
        bool isRenameForm = false)
    {
        ICollectionAddRenameModel model = new CollectionAddRenameModel(isCollectionNameValid, orginalName);
        _ = new CollectionAddRenameFormPresenter(form, model);
        form.IsRenameForm = isRenameForm;
        form.CollectionRenameView.OrginalCollectionName = orginalName;
        form.CollectionRenameView.NewCollectionName = orginalName;
        form.ShowAndBlock();

        return model.NewCollectionNameIsValid ? model.NewCollectionName : "";
    }

    public static OsuCollection AddSelectedBeatmapsToCollection(this IBeatmapListingModel model, OsuCollection collection)
    {
        foreach (Beatmap beatmap in model.SelectedBeatmaps)
        {
            collection.AddBeatmap(beatmap);
        }

        return collection;
    }

    public static void SetClipboardText(string text)
    {
        try
        {
            System.Windows.Forms.Clipboard.SetText(text);
        }
        catch { }
    }
    public static string GetClipboardText()
    {
        try
        {
            return System.Windows.Forms.Clipboard.GetText();
        }
        catch { }

        return "";
    }
}
