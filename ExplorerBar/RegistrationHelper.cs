namespace ExplorerBar
{
    using Interop;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    internal static class RegistrationHelper
    {
        [ComRegisterFunction]
        public static void Register(Type t)
        {
            var guid = t.GUID.ToString("B");
            try
            {
                var registryKey = Registry.ClassesRoot.CreateSubKey($@"CLSID\{guid}");
                registryKey.SetValue(null, GetToolbarName(t));

                // This is the important part for registering the Explorer Bar so that it actually
                // can be enabled within Windows Explorer: implementing the InfoBand category.
                var subkey = registryKey.CreateSubKey("Implemented Categories");
                subkey.CreateSubKey(ComponentCategoryManager.CATID_InfoBand.ToString("B"));
            }
            catch
            {
                throw;
            }
        }

        [ComUnregisterFunction]
        public static void Unregister(Type t)
        {
            var guid = t.GUID.ToString("B");
            try
            {
                Registry.ClassesRoot.OpenSubKey(@"CLSID", true)?.DeleteSubKeyTree(guid);

                Console.WriteLine($"Successfully unregistered deskband `{GetToolbarName(t)}` - GUID: {guid}");
            }
            catch (ArgumentException)
            {
                Console.Error.WriteLine($"Deskband `{GetToolbarName(t)}` is not registered");
            }
            catch (Exception)
            {
                Console.Error.WriteLine($"Failed to unregister deskband `{GetToolbarName(t)}` - GUID: {guid}");
                throw;
            }
        }

        internal static string GetToolbarName(Type t)
        {
            return t.Name;
        }

        internal static bool GetToolbarRequestToShow(Type t)
        {
            return false;
        }
    }
}
