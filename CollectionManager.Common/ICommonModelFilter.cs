namespace CollectionManager.Common;

public interface ICommonModelFilter
{
    /// <summary> 
    /// Should the given model be included when this filter is installed 
    /// </summary> 
    /// <param name="modelObject">The model object to consider</param> 
    /// <returns>Returns true if the model will be included by the filter</returns> 
    bool Filter(object modelObject);
}
