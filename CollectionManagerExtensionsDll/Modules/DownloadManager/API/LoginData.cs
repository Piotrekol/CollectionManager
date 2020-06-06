namespace CollectionManagerExtensionsDll.Modules.DownloadManager.API
{
    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string OsuCookies { get; set; }

        public bool isValid()
        {
            if ((string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password)) && string.IsNullOrWhiteSpace(OsuCookies))
                return false;

            return (Username.Length > 2 && Password.Length > 5) || !string.IsNullOrWhiteSpace(OsuCookies);
        }
    }
}