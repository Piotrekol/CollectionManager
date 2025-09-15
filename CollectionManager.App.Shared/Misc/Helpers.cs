namespace CollectionManager.App.Shared.Misc;

using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.App.Shared.Models.Controls;
using CollectionManager.App.Shared.Presenters.Forms;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.Downloader.Api;
using System.Collections.Specialized;
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
        CollectionAddRenameModel model = new(isCollectionNameValid, orginalName);
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

    public static void SetClipboardText(string text) => Initalizer.Clipboard.SetText(text);
    public static string GetClipboardText() => Initalizer.Clipboard.GetText();
    public static void SetFileDropList(StringCollection value) => Initalizer.Clipboard.SetFileDropList(value);
}
