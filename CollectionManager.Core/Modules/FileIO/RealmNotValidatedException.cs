namespace CollectionManager.Core.Modules.FileIo;
using System;

public class RealmNotValidatedException(string message)
    : Exception(message)
{
}