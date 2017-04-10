using System;

namespace App.Interfaces
{
    public interface IMusicControlModel : IGenericMapSetterModel, IFormEvents
    {
        event EventHandler NextMapRequest;
        void EmitNextMapRequest();
    }
}