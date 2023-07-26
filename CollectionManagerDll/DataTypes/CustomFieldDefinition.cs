using CollectionManager.Enums;

namespace CollectionManager.DataTypes
{
    public class CustomFieldDefinition
    {
        public string Key { get; set; }
        public CustomFieldType Type { get; set; }
        public string DisplayText { get; set; }
    }
}
