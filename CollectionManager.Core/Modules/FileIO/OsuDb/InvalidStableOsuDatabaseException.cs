namespace CollectionManager.Core.Modules.FileIo.OsuDb;
using System;

public class InvalidStableOsuDatabaseException : Exception
{
    public InvalidStableOsuDatabaseException(string message) : base(message)
    {
    }
}
