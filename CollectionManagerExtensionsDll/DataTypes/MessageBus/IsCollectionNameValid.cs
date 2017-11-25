namespace CollectionManagerExtensionsDll.DataTypes.MessageBus
{
    public class IsCollectionNameValid: MessageBusBase
    {
        public string CollectionName { get; }
        public object Response { get; set; }
        public IsCollectionNameValid(string collectionName,string source) : base(source)
        {
            CollectionName = collectionName;
        }
    }
}
