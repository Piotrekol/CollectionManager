namespace CollectionManager.WinForms;

using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

public sealed class FormServices
{
    public static void RegisterServices(ServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddSingleton<IUserDialogs, UserDialogs>();

        Dictionary<Type, IEnumerable<Type>> formTypes = typeof(FormServices)
            .Assembly
            .GetTypes()
            .Where(x =>
                !x.IsAbstract
                && x.IsClass
                && x.GetInterface(nameof(IForm)) == typeof(IForm))
            .ToDictionary(
                formType => formType,
                (formType) => formType
                    .GetInterfaces()
                    .Where(interfaceType => interfaceType.Namespace.StartsWith(nameof(CollectionManager), StringComparison.InvariantCulture))
                    .Except([typeof(IForm)]));

        foreach ((Type formType, IEnumerable<Type> interfaces) in formTypes)
        {
            foreach (Type interfaceType in interfaces)
            {
                _ = serviceCollection.AddTransient(interfaceType, formType);
            }
        }
    }
}
