using System;

namespace CollectionManager.Exceptions
{
    public class CorruptedFileException : Exception
    {
        public CorruptedFileException(string message) : base(message)
        {
            
        }
    }
}