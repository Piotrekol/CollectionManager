namespace CollectionManagerExtensionsDll.DataTypes.MessageBus
{
    public class CollectionNameExists : MessageBusBase
    {
        public string CollectionName { get; }
        public object Response { get; set; }
        public CollectionNameExists(string collectionName, string source) : base(source)
        {
            CollectionName = collectionName;
        }
    }
}