using ExplorerBar.Interop;
using System;
using System.Runtime.InteropServices;
using static eXPerienceBar.Native;

namespace ExplorerBar;

sealed partial class ExplorerBar : IObjectWithSite
{
    public int SetSite([In, MarshalAs(UnmanagedType.IUnknown)] object pUnkSite)
    {
        parentSite = null;

        if (pUnkSite == null)
        {
            OnClosed?.Invoke(this, null);
            return HRESULT.S_OK;
        }

        try
        {
            var oleWindow = (IOleWindow)pUnkSite;
            oleWindow.GetWindow(out ParentWindowHandle);

            BasebarHandle = GetParent(ParentWindowHandle);

            provider.CreateWindow();

            // Above code always sets provider.handle.
            #pragma warning disable CS8629 // Nullable value type may be null.
            User32.SetParent((IntPtr)provider.Handle, ParentWindowHandle);
            #pragma warning restore CS8629 // Nullable value type may be null.

            //MessageBox.Show("Parent window handle: " + parentWindowHandle.ToString());

            //XPSidebar.XPSidebar.evil?.logState("SetSite successfully called");

            // This is at a race with explorer's initial sizing, so to get it to display
            // right without relying on explorer, why not:
            //initialApplyNativeStyling();

            return HRESULT.S_OK;
        }
        catch
        {
            return HRESULT.E_FAIL;
        }
    }

    public int GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out IntPtr ppvSite)
    {
        if (parentSite == null)
        {
            ppvSite = IntPtr.Zero;
            return HRESULT.E_FAIL;
        }

        //XPSidebar.XPSidebar.evil?.logState("GetSite successfully called");

        return Marshal.QueryInterface(
            Marshal.GetIUnknownForObject(parentSite), ref riid, out ppvSite
        );
    }
}
