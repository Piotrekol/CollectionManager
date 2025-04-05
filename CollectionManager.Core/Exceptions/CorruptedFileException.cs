namespace CollectionManager.Core.Exceptions;

using System;

public class CorruptedFileException : Exception
{
    public CorruptedFileException(string message) : base(message)
    {

    }
}