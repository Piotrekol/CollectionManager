using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CollectionManager.Extensions;

public static class StringExtensions
{
    public static string StripInvalidFileNameCharacters(this string fileName, string replacementString = "")
    {
        IEnumerable<char> invalidCharacters = Path
            .GetInvalidFileNameChars()
            .Where(invalidChar => fileName.Contains(invalidChar));

        foreach (var invalidChar in invalidCharacters)
        {
            fileName = fileName.Replace(invalidChar.ToString(), replacementString);
        }

        return fileName;
    }
}
