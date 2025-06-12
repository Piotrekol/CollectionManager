﻿namespace CollectionManagerApp.Models.Controls;
using CollectionManagerApp.Interfaces.Controls;

public class MusicControlModel : GenericMapSetterModel, IMusicControlModel
{
    public void EmitFormClosing() => FormClosing?.Invoke(this, EventArgs.Empty);
    public void EmitFormClosed() => FormClosed?.Invoke(this, EventArgs.Empty);
    public void EmitNextMapRequest() => NextMapRequest?.Invoke(this, EventArgs.Empty);
    public event EventHandler FormClosed;
    public event EventHandler FormClosing;
    public event EventHandler NextMapRequest;
}