namespace CollectionManager.Extensions.Modules.API.osustats;

using CollectionManager.Common.Interfaces;

public class UserInformation : IUserInformation
{
    public string UserName { get; set; }
    public int OsuUserId { get; set; }
}