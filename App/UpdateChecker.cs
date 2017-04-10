using System;
using App.Interfaces;

namespace App
{
    public class UpdateChecker : IUpdateModel
    {
        private const string UpdateUrl = "http://osustats.ppy.sh/api/ce/version";

        public bool Error { get; private set; }
        public string newVersion { get; private set; }
        public string newVersionLink { get; private set; }
        public string currentVersion { get; set; } = "???";

        public bool IsUpdateAvaliable()
        {
            return CheckForUpdates();
        }
        public void CheckIfUpdateIsAvaliable()
        {
            UpdateVersion();
        }

        private bool CheckForUpdates()
        {
            UpdateVersion();
            if (string.IsNullOrWhiteSpace(newVersion))
            {
                Error = true;
                return false;
            }
            Version verLocal, verOnline;
            try
            {
                verLocal = new Version(currentVersion);
                verOnline = new Version(newVersion);
            }
            catch
            {
                return true;
            }


            return verLocal.CompareTo(verOnline) < 0;
        }
        private void UpdateVersion()
        {
            try
            {
                string contents;
                using (var wc = new System.Net.WebClient())
                    contents = wc.DownloadString(UpdateUrl);
                var splited = contents.Split(new[] { ',' }, 2);

                newVersionLink = splited[1];
                newVersion = splited[0];

            }
            catch (Exception)
            {
            }

        }
    }
}