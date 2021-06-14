using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using App.Interfaces;
using App.Models;
using App.Presenters.Forms;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;
using Common.Interfaces;
using GuiComponents.Interfaces;

namespace App.Misc
{
    public static class Helpers
    {
        public static string StripInvalidCharacters(string name)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(invalidChar.ToString(), string.Empty);
            }
            return name.Replace(".", string.Empty);
        }
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
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
            var loginData = new LoginData();
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
            ICollectionAddRenameModel model = new CollectionAddRenameModel(isCollectionNameValid,orginalName);
            new CollectionAddRenameFormPresenter(form, model);
            form.IsRenameForm = isRenameForm;
            form.CollectionRenameView.OrginalCollectionName = orginalName;
            form.CollectionRenameView.NewCollectionName = orginalName;
            form.ShowAndBlock();

            return model.NewCollectionNameIsValid ? model.NewCollectionName : "";
        }

        public static Collection AddSelectedBeatmapsToCollection(this IBeatmapListingModel model,Collection collection)
        {
            foreach (var beatmap in model.SelectedBeatmaps)
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
}
