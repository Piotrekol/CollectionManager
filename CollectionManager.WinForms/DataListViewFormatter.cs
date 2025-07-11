namespace CollectionManager.WinForms;

using CollectionManager.Core.Modules.Mod;
using System;

internal class DataListViewFormatter
{
    public static DateTimeOffset minDate = new DateTime(2006, 1, 1);

    public static string FormatDateTimeOffset(object cellValue)
    {
        if (cellValue is null)
        {
            return string.Empty;
        }

        DateTimeOffset dateTimeOffset = (DateTimeOffset)cellValue;
        return dateTimeOffset > minDate ? $"{dateTimeOffset.LocalDateTime}" : "Never";
    }

    internal static string FormatMods(object cellValue, ModParser modParser)
    {
        if (cellValue is not int modsInt)
        {
            return string.Empty;
        }

        return modParser.GetModsFromEnum(modsInt, true);
    }
}
