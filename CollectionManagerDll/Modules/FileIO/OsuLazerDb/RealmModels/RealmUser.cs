﻿using Realms;
using System;

namespace CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
internal partial class RealmUser
    : IEmbeddedObject
{
    public int OnlineID { get; set; } = 1;

    public string Username { get; set; } = string.Empty;

    [Ignored]
    public CountryCode CountryCode
    {
        get => Enum.TryParse(CountryString, out CountryCode country) ? country : CountryCode.Unknown;
        set => CountryString = value.ToString();
    }

    [MapTo(nameof(CountryCode))]
    public string CountryString { get; set; } = "N/A";

    public bool IsBot => false;
}

public enum CountryCode
{
    Unknown = 0,

}