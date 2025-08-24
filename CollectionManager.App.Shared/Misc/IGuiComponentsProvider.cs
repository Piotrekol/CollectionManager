namespace CollectionManager.App.Shared.Misc;

public interface IGuiComponentsProvider
{
    T GetClassImplementing<T>();
}