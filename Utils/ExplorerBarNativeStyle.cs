using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ExplorerBar;
using ExplorerBar.Interop;
using static eXPerienceBar.Native;

namespace eXPerienceBar.Utils;

/// <summary>
///     Helper class for applying the "native styling"; to blend in with the rest of the OS.
/// </summary>
/// 
/// <remarks>
///     <para>
///         Explorer Bars are usually displayed with some extra UI elements, including a titlebar
///         area with a close button and a resize control. This is because Explorer Bars were meant
///         to be displayed as temporary helpers that could be used in some contexts, rather than
///         as permanent controls.
///     </para>
///     
///     <para>
///         Since we implement a permanently displaying control that emulates the Windows Explorer
///         sidebar seen in older versions, this default behaviour will not do. Fortunately, it's
///         surprisingly easy to mimick a full-height, fixed-size Browser Bar.
///     </para>
/// </remarks>
/// 
/// <author>
///     Taniko Yamamoto (kirasicecreamm@gmail.com)
/// </author>
internal static class ExplorerBarNativeStyle
{
    /**
     * Fill the entire space of the toolbar area, hiding the native controls that
     * display above it.
     * 
     * This allows achieving an XP-like appearance overall.
     * 
     * This function must be ran at least every repaint.
     */
    public static void ApplyNativeStyling(
            ExplorerBarWpfProvider provider, 
            IntPtr parentHwnd, 
            IntPtr baseHwnd
    )
    {
        if (null == provider) return;

        GetWindowRect(baseHwnd, out RECT baseRect);
        int baseWidth = baseRect.right - baseRect.left;
        int baseHeight = baseRect.bottom - baseRect.top;

        GetWindowRect(parentHwnd, out RECT parentRect);
        int parentWidth = parentRect.right - parentRect.left;
        int parentHeight = parentRect.bottom - parentRect.top;

        // int resizerWidth = baseWidth - parentWidth;

        // Since we are working with a UserControl in WPF, the scaling must be set manually for the
        // child window.  This must come first in our order of operations as to not interfere with
        // Explorer's own resize hooks.
        provider.UiElement.Width = 210;
        provider.UiElement.Height = baseHeight;

        GetWindowRect(parentHwnd, out parentRect);
        parentWidth = parentRect.right - parentRect.left;
        parentHeight = parentRect.bottom - parentRect.top;

        if (baseWidth != 210)
        {
            // Resizing the BaseBar allows programmatic scaling of the control, which needs to be
            // 200px.
            SetWindowPos(baseHwnd, 0, baseRect.left, baseRect.top, 210, baseHeight, 0);
        }

        if (parentWidth != 210)
        {
            // The resizer can be covered up by making the ReBarWindow32 (parentHwnd)
            // the same width as its parent BaseBar control. This effectively makes it unresizable.
            SetWindowPos(parentHwnd, 0, 0, 0, 210, parentHeight, 0);
        }

        if (null != provider.Handle)
        {
            // Finally, moving the actual sidebar window to {0, 0} relative to the parent makes it
            // cover up the titlebar that is normally displayed.
            SetWindowPos((IntPtr)provider.Handle, 0, 0, 0, 210, baseHeight, 0);
        }
    }

    private static bool NativeStyleIsNotApplied(ExplorerBarWpfProvider provider)
    {
        return false;
        // Should not happen
        if (null == provider.Handle) throw new Exception("wtf lol");

        GetWindowRect((IntPtr)provider.Handle, out RECT info);

        return info.top != 0;
    }

    /**
     * Lazy race condition avoidance: apply the native styling on initial load.
     * 
     * Explorer wants to set the toolbar to be small and show the toolbar titlebar, which
     * I don't want. Hooking onto WM_PAINT works for repaints, but the ExplorerBar doesn't
     * load fast enough for the hook to work out. Likewise, a simple synchronous call after
     * the window creation doesn't suffice.
     * 
     * There's probably a better way to do this, but this works well enough.
     * 
     * @param provider    A ExplorerBar WPF provider instance to act upon.
     * @param parentHwnd  The parent window handle.
     * @param baseHwnd    A handle to the BaseBar window which hosts the ExplorerBar parent 
     *                    container.
     */
    private static async void InitialApplyNativeStyling(
            ExplorerBarWpfProvider provider,
            IntPtr parentHwnd,
            IntPtr baseHwnd
    )
    {
        if (null == provider.Handle) return;

        do
        {
            ApplyNativeStyling(provider, parentHwnd, baseHwnd);
            await Task.Delay(1);
        }
        while (NativeStyleIsNotApplied(provider));
    }
}
