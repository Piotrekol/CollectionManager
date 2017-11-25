namespace CollectionManagerExtensionsDll.DataTypes.MessageBus
{
    public abstract class MessageBusBase
    {
        public string Source { get; }

        protected MessageBusBase(string source)
        {
            Source = source;
        }
    }
}