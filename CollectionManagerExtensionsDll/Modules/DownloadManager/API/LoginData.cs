namespace CollectionManagerExtensionsDll.Modules.DownloadManager.API
{
    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string SiteCookies { get; set; }
        public string DownloadSource { get; set; }

        public bool isValid()
        {
            if ((string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password)) && string.IsNullOrWhiteSpace(SiteCookies))
                return false;

            return (Username.Length > 2 && Password.Length > 5) || !string.IsNullOrWhiteSpace(SiteCookies);
        }
    }
}