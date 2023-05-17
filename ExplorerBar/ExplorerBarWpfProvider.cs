using ExplorerBar.Interop;
using SHDocVw;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using MSG = ExplorerBar.Interop.MSG;
using eXPerienceBar.Utils.COM;

namespace ExplorerBar;

public abstract class ExplorerBarWpfProvider : IExplorerBar, IExplorerBarProvider
{
    private readonly ExplorerBar impl;

    public abstract UserControl UiElement { get; }

    public IntPtr ParentWindowHandle => impl.ParentWindowHandle;
    public IntPtr BasebarHandle => impl.BasebarHandle;

    internal HwndSource? HwndSource { get; private set; }

    public ExplorerBarWpfProvider()
    {
        impl = new ExplorerBar(this);
        impl.OnClosed += (o, e) => HandleClosed();
    }

    [ComRegisterFunction]
    private static void Register(Type t)
    {
        ComRegistrationHelper.Register(t, new ComRegistrationInfo {
            ImplementedCategories = { ComponentCategoryManager.CATID_InfoBand }
        });
    }

    [ComUnregisterFunction]
    private static void Unregister(Type t)
    {
        ComRegistrationHelper.Unregister(t);
    }

    /**
     * Unlike the Desk Band implementations, this spawns a child window which requires a
     * parent to display at all.
     * 
     * As such, the creation of the window must be delayed until the parent handle is
     * retrieved.
     */
    public void CreateWindow()
    {
        try
        {
            var sourceParams = new HwndSourceParameters {
                TreatAsInputRoot = true,
                // needs WS_CHILD or focus will detach from explorer when clicked
                WindowStyle = unchecked((int)(
                    WindowStyles.WS_OVERLAPPED |
                    WindowStyles.WS_VISIBLE |
                    WindowStyles.WS_CHILD |
                    WindowStyles.WS_CLIPCHILDREN |
                    WindowStyles.WS_CLIPSIBLINGS
                )),
                HwndSourceHook = HwndSourceHook,
                ParentWindow = impl.ParentWindowHandle
            };

            HwndSource = new HwndSource(sourceParams);
            HwndSource.RootVisual = UiElement;
            HwndSource.CompositionTarget.BackgroundColor = Colors.White;

            HandleWindowCreated();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    protected abstract void HandleWindowCreated();

    protected virtual IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
    {
        handled = false;
        return IntPtr.Zero;
    }

    public InternetExplorer? shellWindow = null;

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    // THIS IS A WORKING IMPLEMENTATION YAYY
    // dev note: move THIS implementation to separate Shell module later, this is very
    // useful thank youuu
    public string? GetActiveExplorerPath()
    {
        if (shellWindow == null)
        {
            // get the active window
            IntPtr handle = GetForegroundWindow();

            // Required ref: SHDocVw (Microsoft Internet Controls COM Object) - C:\Windows\system32\ShDocVw.dll
            ShellWindows shellWindows = new SHDocVw.ShellWindows();

            // loop through all windows
            foreach (InternetExplorer window in shellWindows)
            {
                // match active window
                if (window.HWND == (int)handle)
                {
                    // Required ref: Shell32 - C:\Windows\system32\Shell32.dll
                    shellWindow = window;
                    break;
                }
            }

            return null;
        }

        // Item without an index returns the current object
        Shell32.IShellFolderViewDual2 folderView = shellWindow.Document;
        if (folderView?.Folder != null)
        {
            return folderView.Folder.Items().Item().Path;
        }

        return shellWindow.LocationURL;
    }

    // COM Imports

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    protected virtual void HandleClosed() {}

    protected static int LowWord(IntPtr value)
    {
        return unchecked((short)(long)value);
    }

    protected static int HighWord(IntPtr value)
    {
        return unchecked((short)(long)value >> 16);
    }

    public IntPtr? Handle
    {
        get
        {
            if (null == HwndSource) return null;

            return HwndSource.Handle;
        }
    }

    public Guid Guid => GetType().GUID;

    public bool HasFocus
    {
        get => UiElement?.IsKeyboardFocusWithin ?? false;
        set
        {
            if (value)
            {
                UiElement?.Focus();
            }
        }
    }

    public int UIActivateIO(int fActivate, ref MSG msg)
    {
        return impl.UIActivateIO(fActivate, ref msg);
    }

    public int HasFocusIO()
    {
        return impl.HasFocusIO();
    }

    public int TranslateAcceleratorIO(ref MSG msg)
    {
        return impl.TranslateAcceleratorIO(ref msg);
    }

    #region ExplorerBarProvider implementations
    public int GetWindow(out IntPtr phwnd)
    {
        return impl.GetWindow(out phwnd);
    }

    public int ContextSensitiveHelp(bool fEnterMode)
    {
        return impl.ContextSensitiveHelp(fEnterMode);
    }

    public int ShowDW([In] bool fShow)
    {
        return impl.ShowDW(fShow);
    }

    public int CloseDW([In] uint dwReserved)
    {
        return impl.CloseDW(dwReserved);
    }

    public int ResizeBorderDW(RECT prcBorder, [In, MarshalAs(UnmanagedType.IUnknown)] IntPtr punkToolbarSite, bool fReserved)
    {
        return impl.ResizeBorderDW(prcBorder, punkToolbarSite, fReserved);
    }

    public int GetBandInfo(uint dwBandID, DESKBANDINFO.DBIF dwViewMode, ref DESKBANDINFO pdbi)
    {
        return impl.GetBandInfo(dwBandID, dwViewMode, ref pdbi);
    }

    public int CanRenderComposited(out bool pfCanRenderComposited)
    {
        return impl.CanRenderComposited(out pfCanRenderComposited);
    }

    public int SetCompositionState(bool fCompositionEnabled)
    {
        return impl.SetCompositionState(fCompositionEnabled);
    }

    public int GetCompositionState(out bool pfCompositionEnabled)
    {
        return impl.GetCompositionState(out pfCompositionEnabled);
    }

    public int SetSite([In, MarshalAs(UnmanagedType.IUnknown)] object pUnkSite)
    {
        return impl.SetSite(pUnkSite);
    }

    public int GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out IntPtr ppvSite)
    {
        return impl.GetSite(ref riid, out ppvSite);
    }

    public int InvokeCommand(IntPtr pici)
    {
        return impl.InvokeCommand(pici);
    }

    public int GetCommandString(ref uint idcmd, uint uflags, ref uint pwReserved, [MarshalAs(UnmanagedType.LPTStr)] out string pcszName, uint cchMax)
    {
        return impl.GetCommandString(ref idcmd, uflags, ref pwReserved, out pcszName, cchMax);
    }

    public int HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam)
    {
        return impl.HandleMenuMsg(uMsg, wParam, lParam);
    }

    public int HandleMenuMsg2(uint uMsg, IntPtr wParam, IntPtr lParam, out IntPtr plResult)
    {
        return impl.HandleMenuMsg2(uMsg, wParam, lParam, out plResult);
    }

    public int GetClassID(out Guid pClassID)
    {
        return impl.GetClassID(out pClassID);
    }

    public int GetSizeMax(out ulong pcbSize)
    {
        return impl.GetSizeMax(out pcbSize);
    }

    public int IsDirty()
    {
        return impl.IsDirty();
    }

    public int Load(object pStm)
    {
        return impl.Load(pStm);
    }

    public int Save(IntPtr pStm, bool fClearDirty)
    {
        return impl.Save(pStm, fClearDirty);
    }

    public int QueryContextMenu(IntPtr hMenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, QueryContextMenuFlags uFlags)
    {
        return impl.QueryContextMenu(hMenu, indexMenu, idCmdFirst, idCmdLast, uFlags);
    }
    #endregion
}