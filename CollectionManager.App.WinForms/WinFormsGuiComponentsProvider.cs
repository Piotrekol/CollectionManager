namespace CollectionManager.App.Winforms;

using CollectionManager.App.Shared.Misc;
using System.IO;
using System.Reflection;

public sealed class WinFormsGuiComponentsProvider : IGuiComponentsProvider
{
    public WinFormsGuiComponentsProvider()
    {
        LoadGuiDll();
    }

    private string GuiDllLocation { get; set; }
    private Assembly GuiDllAssembly { get; set; }
    private void LoadGuiDll()
    {
        GuiDllLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CollectionManager.WinForms.dll");
        GuiDllAssembly = Assembly.LoadFile(GuiDllLocation);
    }

    private static IEnumerable<Type> GetTypesWithInterface<T>(Assembly asm)
    {
        Type it = typeof(T);
        return asm.GetLoadableTypes().Where(it.IsAssignableFrom).ToList();
    }

    public T GetClassImplementing<T>() => GetClassImplementing<T>(GuiDllAssembly);

    private static T GetClassImplementing<T>(Assembly asm)
    {
        foreach (Type tt in GetTypesWithInterface<T>(asm))
        {
            return (T)Activator.CreateInstance(tt);
        }

        return default;
    }
}