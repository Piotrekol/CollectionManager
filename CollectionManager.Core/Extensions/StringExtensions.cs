﻿namespace CollectionManager.Core.Extensions;

using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class StringExtensions
{
    public static string StripInvalidFileNameCharacters(this string fileName, string replacementString = "")
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentNullException(nameof(fileName), "File name cannot be null or empty.");
        }

        IEnumerable<char> invalidCharacters = Path
            .GetInvalidFileNameChars()
            .Where(invalidChar => fileName.Contains(invalidChar));

        foreach (char invalidChar in invalidCharacters)
        {
            fileName = fileName.Replace(invalidChar.ToString(), replacementString);
        }

        return fileName;
    }
}
