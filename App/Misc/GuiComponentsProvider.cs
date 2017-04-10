using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace App.Misc
{
    public sealed class GuiComponentsProvider
    {
        public static GuiComponentsProvider Instance = new GuiComponentsProvider();

        private GuiComponentsProvider()
        {
            LoadGuiDll();
        }
        private string GuiDllLocation { get; set; }
        private Assembly GuiDllAssembly { get; set; }
        private void LoadGuiDll()
        {
            GuiDllLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GuiComponents.dll");
            GuiDllAssembly = Assembly.LoadFile(GuiDllLocation);
        }

        private IEnumerable<Type> GetTypesWithInterface<T>(Assembly asm)
        {
            var it = typeof(T);
            return asm.GetLoadableTypes().Where(it.IsAssignableFrom).ToList();
        }

        public T GetClassImplementing<T>()
        {
            return GetClassImplementing<T>(GuiDllAssembly);
        }

        private T GetClassImplementing<T>(Assembly asm)
        {
            foreach (var tt in GetTypesWithInterface<T>(asm))
            {
                return (T)Activator.CreateInstance(tt);
            }
            return default(T);
        }
    }
}