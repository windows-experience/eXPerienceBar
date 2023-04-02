using ExplorerBar.Interop;
using Microsoft.Win32;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using eXPerienceBar.Utils;
using eXPerienceBar.Utils.COM;

namespace eXPerienceBar.LoaderHelper;

/// <summary>
///     Implements a loader Browser Helper Object for displaying the sidebar when Explorer
///     first runs.
/// </summary>
/// 
/// <remarks>
///     <para>
///         This is required because Explorer Bars must be invoked manually, however we need
///         the sidebar to be displayed as an Explorer Bar for implementation convenience
///         
///         Fortunately, there pretty much already is an API for automatically opening the
///         Explorer Bar programmatically, so the only thing that needs to be done is to
///         implement a loader class that calls this method.
///     </para>
///     
///     <para>
///         <see href="https://en.wikipedia.org/wiki/Browser_Helper_Object">Browser Helper Objects</see>
///         are basically the Internet Explorer equivalent to a Chrome extension or Firefox addon,
///         but they are unique in that they are native (not JS) and therefore have virtually
///         no security restrictions. They also run under Windows Explorer because that literally is IE
///         too.
///     </para>
/// </remarks>
/// 
/// <author>
///     Taniko Yamamoto (kirasicecreamm@gmail.com)
/// </author>
[ComVisible(true)]
[Guid("372EB99F-5030-46D8-B181-B9F3C0B7763D")] // Unique class ID for this
[ComRegistrationInfo(DisplayName = "eXPerienceBar Loader Helper Class")]
public class LoaderHelperObject : IObjectWithSite, ILoaderHelperComServer
{
    private InternetExplorer? explorer;

    internal const string BHO_REG_KEY = 
        @"Software\Microsoft\Windows\CurrentVersion\Explorer\Browser Helper Objects";

    internal void HandleExplorerLoaded()
    {
        explorer?.ShowBrowserBar(
            typeof(MainSidebar).GUID.ToString("B"),
            true,
            null
        );
    }

    [ComRegisterFunction]
    public static void RegisterBho(Type type)
    {
        try
        {
            // Register the base COM class for this
            // (not necessary but this is required for the display name)
            ComRegistrationHelper.Register(type);

            // Register the class as a Browser Helper Object
            string guid = type.GUID.ToString("B");

            RegistryKey? regKey = Registry.LocalMachine.OpenSubKey(BHO_REG_KEY, true);

            if (null == regKey)
            {
                Registry.LocalMachine.CreateSubKey(BHO_REG_KEY);
            }

            RegistryKey? newRegistration = regKey?.CreateSubKey(guid);

            newRegistration?.Close();
            regKey?.Close();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
            throw;
        }
    }

    [ComUnregisterFunction]
    public static void UnregisterBho(Type type)
    {
        // Unregister the base COM class for this.
        ComRegistrationHelper.Unregister(type);

        // Unregister the Browser Helper Object
        string guid = type.GUID.ToString("B");
        Registry.LocalMachine.OpenSubKey(BHO_REG_KEY, true)?.DeleteSubKey(guid);
    }

    #region IObjectWithSite implementations
    public int SetSite(object pUnkSite)
    {
        if (null != pUnkSite)
        {
            explorer = (InternetExplorer)pUnkSite;

            HandleExplorerLoaded();
        }
        else
        {
            explorer = null;
        }

        return HRESULT.S_OK;
    }

    public int GetSite(ref Guid riid, out IntPtr ppvSite)
    {
        IntPtr pUnknown = Marshal.GetIUnknownForObject(explorer);
        int hResult = Marshal.QueryInterface(pUnknown, ref riid, out ppvSite);
        Marshal.Release(pUnknown);

        return hResult;
    }
    #endregion
}
