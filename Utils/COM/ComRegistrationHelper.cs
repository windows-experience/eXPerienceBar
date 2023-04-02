using Microsoft.Win32;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;

namespace eXPerienceBar.Utils.COM;

/// <summary>
///     A basic helper class for COM class registration and unregistration.
/// </summary>
public static class ComRegistrationHelper
{
    /// <summary>
    ///     Called whenever the COM class is registered via regsvr32.
    /// </summary>
    /// 
    /// <param name="t">
    ///     The typename of the class being registered.
    /// </param>
    public static RegistryKey Register(Type t, ComRegistrationInfo regInfo)
    {
        Guid clsid = t.GUID;
        string displayName = GetDisplayName(t, regInfo);

        try
        {
            RegistryKey clsidKey = CreateComClsid(clsid, displayName);
            PushImplementedCategories(regInfo, clsidKey);

            return clsidKey;
        }
        catch
        {
            throw;
        }
    }

    public static RegistryKey Register(Type t) => Register(t, new ComRegistrationInfo());

    /// <summary>
    ///     Called whenever the COM class is unregistered via regsvr32.
    /// </summary>
    /// 
    /// <param name="t">
    ///     The typename of the class being unregistered.
    /// </param>
    public static void Unregister(Type t)
    {
        Guid clsid = t.GUID;

        try
        {
            DeleteComClsid(clsid);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    ///     Gets the display name set for a COM class.
    /// </summary>
    /// 
    /// <param name="t">
    ///     The typename of the class being registered.
    /// </param>
    internal static string GetDisplayName(Type t, ComRegistrationInfo regInfo)
    {
        return t.GetCustomAttribute<ComRegistrationInfoAttribute>(true)?.DisplayName ??
               regInfo?.DisplayName ??
               "experienceBar internal class " + t.Name
        ;
    }

    /// <summary>
    ///     Creates the CLSID registry key for a COM class. This is called during registration.
    /// </summary>
    /// 
    /// <param name="clsid">
    ///     The CLSID of the class.
    /// </param>
    /// 
    /// <param name="displayName">
    ///     The display name of the 
    /// </param>
    internal static RegistryKey CreateComClsid(Guid clsid, string displayName = "")
    {
        string clsidStr = clsid.ToString("B");

        try
        {
            RegistryKey clsidKey = Registry.ClassesRoot.CreateSubKey($@"CLSID\{clsidStr}");
            clsidKey.SetValue(null, displayName);

            return clsidKey;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    ///     Deletes the CLSID registry key for a COM class. This is called during unregistration.
    /// </summary>
    /// 
    /// <param name="clsid">
    ///     The CLSID of the class.
    /// </param>
    internal static void DeleteComClsid(Guid clsid)
    {
        string clsidStr = clsid.ToString("B");

        try
        {
            Registry.ClassesRoot.OpenSubKey(@"CLSID", true)?.DeleteSubKeyTree(clsidStr);
        }
        catch (ArgumentException)
        {
            Console.Error.WriteLine($"COM class \"{clsidStr}\" is not registered, skipping...");
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    ///     Pushes the implemented categories of a COM class to the registry.
    /// </summary>
    /// 
    /// <param name="regInfo">
    ///     The registration info.
    /// </param>
    /// 
    /// <param name="classKey">
    ///     The registry key to write to.
    /// </param>
    internal static void PushImplementedCategories(
            ComRegistrationInfo regInfo, 
            RegistryKey classKey
    )
    {
        if (regInfo?.ImplementedCategories != null)
        {
            try
            {
                RegistryKey impl = classKey.CreateSubKey("Implemented Categories");

                foreach (Guid catId in regInfo.ImplementedCategories)
                {
                    impl.CreateSubKey(catId.ToString("B"));
                }
            }
            catch
            {
                throw;
            }
        }
    }
}