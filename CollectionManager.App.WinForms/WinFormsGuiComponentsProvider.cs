namespace CollectionManager.App.Winforms;

using CollectionManager.App.Shared.Misc;
using CollectionManager.WinForms;
using Microsoft.Extensions.DependencyInjection;

public sealed class WinFormsGuiComponentsProvider : IGuiComponentsProvider
{
    private readonly ServiceProvider serviceProvider;

    public WinFormsGuiComponentsProvider()
    {
        ServiceCollection serviceCollection = new();
        FormServices.RegisterServices(serviceCollection);
        serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public T GetClassImplementing<T>() => serviceProvider.GetRequiredService<T>();
}