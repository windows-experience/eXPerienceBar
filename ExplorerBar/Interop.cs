﻿/**
 * Copied from https://github.com/dsafa/CSDeskBand/blob/master/src/CSDeskBand/Interop/NativeMethods.cs
 * as the implementation there was complete.
 */
namespace ExplorerBar.Interop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;
    using System.Threading.Tasks;

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("4CF504B0-DE96-11D0-8B3F-00A0C911E8E5")]
    internal interface IBandSite
    {
        [PreserveSig]
        int AddBand(ref object punk);

        [PreserveSig]
        int EnumBands(int uBand, out uint pdwBandID);

        [PreserveSig]
        int QueryBand(uint dwBandID, out IDeskBand ppstb, out BANDSITEINFO.BSSF pdwState, [MarshalAs(UnmanagedType.LPWStr)] out string pszName, int cchName);

        [PreserveSig]
        int SetBandState(uint dwBandID, BANDSITEINFO.BSIM dwMask, BANDSITEINFO.BSSF dwState);

        [PreserveSig]
        int RemoveBand(uint dwBandID);

        [PreserveSig]
        int GetBandObject(uint dwBandID, ref Guid riid, out IntPtr ppv);

        [PreserveSig]
        int SetBandSiteInfo([In] ref BANDSITEINFO pbsinfo);

        [PreserveSig]
        int GetBandSiteInfo([In, Out] ref BANDSITEINFO pbsinfo);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("012DD920-7B26-11D0-8CA9-00A0C92DBFE8")]
    public interface IDockingWindow : IOleWindow
    {
        [PreserveSig]
        new int GetWindow(out IntPtr phwnd);

        [PreserveSig]
        new int ContextSensitiveHelp(bool fEnterMode);

        [PreserveSig]
        int ShowDW(bool fShow);

        [PreserveSig]
        int CloseDW(uint dwReserved);

        [PreserveSig]
        int ResizeBorderDW(RECT prcBorder, [MarshalAs(UnmanagedType.IUnknown)] IntPtr punkToolbarSite, bool fReserved);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("EB0FE172-1A3A-11D0-89B3-00A0C90A90AC")]
    public interface IDeskBand : IDockingWindow
    {
        [PreserveSig]
        new int GetWindow(out IntPtr phwnd);

        [PreserveSig]
        new int ContextSensitiveHelp(bool fEnterMode);

        [PreserveSig]
        new int ShowDW(bool fShow);

        [PreserveSig]
        new int CloseDW(uint dwReserved);

        [PreserveSig]
        new int ResizeBorderDW(RECT prcBorder, [MarshalAs(UnmanagedType.IUnknown)] IntPtr punkToolbarSite, bool fReserved);

        [PreserveSig]
        int GetBandInfo(uint dwBandID, DESKBANDINFO.DBIF dwViewMode, ref DESKBANDINFO pdbi);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("79D16DE4-ABEE-4021-8D9D-9169B261D657")]
    public interface IDeskBand2 : IDeskBand
    {
        [PreserveSig]
        new int GetWindow(out IntPtr phwnd);

        [PreserveSig]
        new int ContextSensitiveHelp(bool fEnterMode);

        [PreserveSig]
        new int ShowDW(bool fShow);

        [PreserveSig]
        new int CloseDW(uint dwReserved);

        [PreserveSig]
        new int ResizeBorderDW(RECT prcBorder, [MarshalAs(UnmanagedType.IUnknown)] IntPtr punkToolbarSite, bool fReserved);

        [PreserveSig]
        new int GetBandInfo(uint dwBandID, DESKBANDINFO.DBIF dwViewMode, ref DESKBANDINFO pdbi);

        [PreserveSig]
        int CanRenderComposited(out bool pfCanRenderComposited);

        [PreserveSig]
        int SetCompositionState(bool fCompositionEnabled);

        [PreserveSig]
        int GetCompositionState(out bool pfCompositionEnabled);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214e4-0000-0000-c000-000000000046")]
    public interface IContextMenu
    {
        [PreserveSig]
        int QueryContextMenu(IntPtr hMenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, QueryContextMenuFlags uFlags);

        [PreserveSig]
        int InvokeCommand(IntPtr pici);

        [PreserveSig]
        int GetCommandString(ref uint idcmd, uint uflags, ref uint pwReserved, [MarshalAs(UnmanagedType.LPTStr)] out string pcszName, uint cchMax);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214f4-0000-0000-c000-000000000046")]
    public interface IContextMenu2 : IContextMenu
    {
        [PreserveSig]
        new int QueryContextMenu(IntPtr hMenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, QueryContextMenuFlags uFlags);

        [PreserveSig]
        new int InvokeCommand(IntPtr pici);

        [PreserveSig]
        new int GetCommandString(ref uint idcmd, uint uflags, ref uint pwReserved, [MarshalAs(UnmanagedType.LPTStr)] out string pcszName, uint cchMax);

        [PreserveSig]
        int HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("bcfce0a0-ec17-11d0-8d10-00a0c90f2719")]
    public interface IContextMenu3 : IContextMenu2
    {
        [PreserveSig]
        new int QueryContextMenu(IntPtr hMenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, QueryContextMenuFlags uFlags);

        [PreserveSig]
        new int InvokeCommand(IntPtr pici);

        [PreserveSig]
        new int GetCommandString(ref uint idcmd, uint uflags, ref uint pwReserved, [MarshalAs(UnmanagedType.LPTStr)] out string pcszName, uint cchMax);

        [PreserveSig]
        new int HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam);

        [PreserveSig]
        int HandleMenuMsg2(uint uMsg, IntPtr wParam, IntPtr lParam, out IntPtr plResult);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("68284faa-6a48-11d0-8c78-00c04fd918b4")]
    public interface IInputObject
    {
        [PreserveSig]
        int UIActivateIO(int fActivate, ref MSG msg);

        [PreserveSig]
        int HasFocusIO();

        [PreserveSig]
        int TranslateAcceleratorIO(ref MSG msg);
    }

    //https://msdn.microsoft.com/en-us/library/windows/desktop/bb761789(v=vs.85).aspx
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("F1DB8392-7331-11D0-8C99-00A0C92DBFE8")]
    public interface IInputObjectSite
    {
        [PreserveSig]
        int OnFocusChangeIS([MarshalAs(UnmanagedType.IUnknown)] object punkObj, Int32 fSetFocus);
    }

    //https://msdn.microsoft.com/en-us/library/windows/desktop/ms693765(v=vs.85).aspx
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("FC4801A3-2BA9-11CF-A229-00AA003D7352")]
    public interface IObjectWithSite
    {
        [PreserveSig]
        int SetSite([MarshalAs(UnmanagedType.IUnknown)] object pUnkSite);

        [PreserveSig]
        int GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out IntPtr ppvSite);
    }

    //https://msdn.microsoft.com/en-us/library/windows/desktop/ms683797(v=vs.85).aspx
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("b722bccb-4e68-101b-a2bc-00aa00404770")]
    internal interface IOleCommandTarget
    {
        [PreserveSig]
        void QueryStatus(ref Guid pguidCmdGroup, uint cCmds, [MarshalAs(UnmanagedType.LPArray), In, Out] OLECMD[] prgCmds, [In, Out] ref OLECMDTEXT pCmdText);

        [PreserveSig]
        int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdExecOpt, IntPtr pvaIn, [In, Out] IntPtr pvaOut);
    }

    //https://msdn.microsoft.com/en-us/library/windows/desktop/ms680102(v=vs.85).aspx
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000114-0000-0000-C000-000000000046")]
    public interface IOleWindow
    {
        [PreserveSig]
        int GetWindow(out IntPtr phwnd);

        [PreserveSig]
        int ContextSensitiveHelp(bool fEnterMode);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0000010c-0000-0000-C000-000000000046")]
    public interface IPersist
    {
        [PreserveSig]
        int GetClassID(out Guid pClassID);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000109-0000-0000-C000-000000000046")]
    public interface IPersistStream : IPersist
    {
        [PreserveSig]
        new int GetClassID(out Guid pClassID);

        [PreserveSig]
        int GetSizeMax(out ulong pcbSize);

        [PreserveSig]
        int IsDirty();

        [PreserveSig]
        int Load([In, MarshalAs(UnmanagedType.Interface)] object pStm);

        [PreserveSig]
        int Save([In, MarshalAs(UnmanagedType.Interface)] IntPtr pStm, bool fClearDirty);
    }

    [ComImport, Guid("6D67E846-5B9C-4db8-9CBC-DDE12F4254F1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITrayDeskband
    {
        [PreserveSig]
        int ShowDeskBand([In, MarshalAs(UnmanagedType.Struct)] ref Guid clsid);
        [PreserveSig]
        int HideDeskBand([In, MarshalAs(UnmanagedType.Struct)] ref Guid clsid);
        [PreserveSig]
        int IsDeskBandShown([In, MarshalAs(UnmanagedType.Struct)] ref Guid clsid);
        [PreserveSig]
        int DeskBandRegistrationChanged();
    }

    // https://www.productiverage.com/idispatch-iwastedtimeonthis-but-ilearntlots
    [ComImport()]
    [Guid("00020400-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IDispatch
    {
        [PreserveSig]
        int GetTypeInfoCount(out int Count);

        [PreserveSig]
        int GetTypeInfo
        (
          [MarshalAs(UnmanagedType.U4)] int iTInfo,
          [MarshalAs(UnmanagedType.U4)] int lcid,
          out System.Runtime.InteropServices.ComTypes.ITypeInfo typeInfo
        );

        [PreserveSig]
        int GetIDsOfNames
        (
          ref Guid riid,
          [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)]
          string[] rgsNames,
          int cNames,
          int lcid,
          [MarshalAs(UnmanagedType.LPArray)] int[] rgDispId
        );

        [PreserveSig]
        int Invoke
        (
          int dispIdMember,
          ref Guid riid,
          uint lcid,
          ushort wFlags,
          ref System.Runtime.InteropServices.ComTypes.DISPPARAMS pDispParams,
          ref object pVarResult,
          ref System.Runtime.InteropServices.ComTypes.EXCEPINFO pExcepInfo,
          ref UInt32 pArgErr
        );
    }


    internal class User32
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, ref MENUITEMINFO lpmii);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateMenu();

        [DllImport("user32.dll")]
        public static extern bool DestroyMenu(IntPtr hMenu);

        [DllImport("user32.dll")]
        public static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        public static int HiWord(int val)
        {
            return Convert.ToInt32(BitConverter.ToInt16(BitConverter.GetBytes(val), 2));
        }

        public static int LoWord(int val)
        {
            return Convert.ToInt32(BitConverter.ToInt16(BitConverter.GetBytes(val), 0));
        }
    }

    //internal class Shell32
    //{
    //    [DllImport("shell32.dll")]
    //    public static extern IntPtr SHAppBarMessage(APPBARMESSAGE dwMessage, [In] ref APPBARDATA pData);
    //}

    internal enum tagDESKBANDCID
    {
        DBID_BANDINFOCHANGED = 0,
        DBID_SHOWONLY = 1,
        DBID_MAXIMIZEBAND = 2,
        DBID_PUSHCHEVRON = 3
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
    }

    [Flags]
    public enum QueryContextMenuFlags : uint
    {
        CMF_NORMAL = 0x00000000,
        CMF_DEFAULTONLY = 0x00000001,
        CMF_VERBSONLY = 0x00000002,
        CMF_EXPLORE = 0x00000004,
        CMF_NOVERBS = 0x00000008,
        CMF_CANRENAME = 0x00000010,
        CMF_NODEFAULT = 0x00000020,
        CMF_ITEMMENU = 0x00000080,
        CMF_EXTENDEDVERBS = 0x00000100,
        CMF_DISABLEDVERBS = 0x00000200,
        CMF_ASYNCVERBSTATE = 0x00000400,
        CMF_OPTIMIZEFORINVOKE = 0x00000800,
        CMF_SYNCCASCADEMENU = 0x00001000,
        CMF_DONOTPICKDEFAULT = 0x00002000,
        CMF_RESERVED = 0xffff0000,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct OLECMDTEXT
    {
        public uint cmdtextf;
        public uint cwActual;
        public uint cwBuf;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string rgwz;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct OLECMD
    {
        public uint cmdID;
        public uint cmdf;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public uint wParam;
        public int lParam;
        public uint time;
        public POINT pt;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MENUITEMINFO
    {
        public int cbSize;
        public MIIM fMask;
        public MFT fType;
        public MFS fState;
        public uint wID;
        public IntPtr hSubMenu;
        public IntPtr hbmpChecked;
        public IntPtr hbmpUnchecked;
        public IntPtr dwItemData;
        [MarshalAs(UnmanagedType.LPStr)] public string dwTypeData;
        public uint cch;
        public IntPtr hbmpItem;

        [Flags]
        public enum MIIM : uint
        {
            MIIM_BITMAP = 0x00000080,
            MIIM_CHECKMARKS = 0x00000008,
            MIIM_DATA = 0x00000020,
            MIIM_FTYPE = 0x00000100,
            MIIM_ID = 0x00000002,
            MIIM_STATE = 0x00000001,
            MIIM_STRING = 0x00000040,
            MIIM_SUBMENU = 0x00000004,
            MIIM_TYPE = 0x00000010
        }

        [Flags]
        public enum MFT : uint
        {
            MFT_BITMAP = 0x00000004,
            MFT_MENUBARBREAK = 0x00000020,
            MFT_MENUBREAK = 0x00000040,
            MFT_OWNERDRAW = 0x00000100,
            MFT_RADIOCHECK = 0x00000200,
            MFT_RIGHTJUSTIFY = 0x00004000,
            MFT_RIGHTORDER = 0x00002000,
            MFT_SEPARATOR = 0x00000800,
            MFT_STRING = 0x00000000,
        }

        [Flags]
        public enum MFS : uint
        {
            MFS_CHECKED = 0x00000008,
            MFS_DEFAULT = 0x00001000,
            MFS_DISABLED = 0x00000003,
            MFS_ENABLED = 0x00000000,
            MFS_GRAYED = 0x00000003,
            MFS_HILITE = 0x00000080,
            MFS_UNCHECKED = 0x00000000,
            MFS_UNHILITE = 0x00000000,
        }
    }

    internal class HRESULT
    {
        public static readonly int S_OK = 0;
        public static readonly int S_FALSE = 1;
        public static readonly int E_NOTIMPL = unchecked((int)0x80004001);
        public static readonly int E_FAIL = unchecked((int)0x80004005);

        public static int MakeHResult(uint sev, uint facility, uint errorNo)
        {
            uint result = sev << 31 | facility << 16 | errorNo;
            return unchecked((int)result);
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DESKBANDINFO
    {
        public DBIM dwMask;
        public POINT ptMinSize;
        public POINT ptMaxSize;
        public POINT ptIntegral;
        public POINT ptActual;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)] public String wszTitle;
        public DBIMF dwModeFlags;
        public COLORREF crBkgnd;

        [Flags]
        public enum DBIF : uint
        {
            DBIF_VIEWMODE_NORMAL = 0x0000,
            DBIF_VIEWMODE_VERTICAL = 0x0001,
            DBIF_VIEWMODE_FLOATING = 0x0002,
            DBIF_VIEWMODE_TRANSPARENT = 0x0004
        }

        [Flags]
        public enum DBIM : uint
        {
            DBIM_MINSIZE = 0x0001,
            DBIM_MAXSIZE = 0x0002,
            DBIM_INTEGRAL = 0x0004,
            DBIM_ACTUAL = 0x0008,
            DBIM_TITLE = 0x0010,
            DBIM_MODEFLAGS = 0x0020,
            DBIM_BKCOLOR = 0x0040
        }

        [Flags]
        public enum DBIMF : uint
        {
            DBIMF_NORMAL = 0x0000,
            DBIMF_FIXED = 0x0001,
            DBIMF_FIXEDBMP = 0x0004,
            DBIMF_VARIABLEHEIGHT = 0x0008,
            DBIMF_UNDELETEABLE = 0x0010,
            DBIMF_DEBOSSED = 0x0020,
            DBIMF_BKCOLOR = 0x0040,
            DBIMF_USECHEVRON = 0x0080,
            DBIMF_BREAK = 0x0100,
            DBIMF_ADDTOFRONT = 0x0200,
            DBIMF_TOPALIGN = 0x0400,
            DBIMF_NOGRIPPER = 0x0800,
            DBIMF_ALWAYSGRIPPER = 0x1000,
            DBIMF_NOMARGINS = 0x2000
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COLORREF
    {
        public byte R;
        public byte G;
        public byte B;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CMINVOKECOMMANDINFOEX
    {
        public uint cbSize;
        public CMIC fMask;
        public IntPtr hwnd;
        public IntPtr lpVerb;
        [MarshalAs(UnmanagedType.LPStr)] public string lpParameters;
        [MarshalAs(UnmanagedType.LPStr)] public string lpDirectory;
        public int nShow;
        public uint dwHotKey;
        public IntPtr hIcon;
        [MarshalAs(UnmanagedType.LPStr)] public string lpTitle;
        public IntPtr lpVerbW;
        [MarshalAs(UnmanagedType.LPWStr)] public string lpParametersW;
        [MarshalAs(UnmanagedType.LPWStr)] public string lpDirectoryW;
        [MarshalAs(UnmanagedType.LPWStr)] public string lpTitleW;
        public POINT ptInvoke;

        [Flags]
        public enum CMIC
        {
            CMIC_MASK_HOTKEY = 0x00000020,
            CMIC_MASK_ICON = 0x00000010,
            CMIC_MASK_FLAG_NO_UI = 0x00000400,
            CMIC_MASK_UNICODE = 0x00004000,
            CMIC_MASK_NO_CONSOLE = 0x00008000,
            CMIC_MASK_ASYNCOK = 0x00100000,
            CMIC_MASK_NOASYNC = 0x00000100,
            CMIC_MASK_SHIFT_DOWN = 0x10000000,
            CMIC_MASK_PTINVOKE = 0x20000000,
            CMIC_MASK_CONTROL_DOWN = 0x40000000,
            CMIC_MASK_FLAG_LOG_USAGE = 0x04000000,
            CMIC_MASK_NOZONECHECKS = 0x00800000,
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class CMINVOKECOMMANDINFO
    {
        public int cbSize;
        public CMIC fMask;
        public IntPtr hwnd;
        public IntPtr lpVerb;
        [MarshalAs(UnmanagedType.LPStr)] public string lpParameters;
        [MarshalAs(UnmanagedType.LPStr)] public string lpDirectory;
        public int nShow;
        public int dwHotKey;
        public IntPtr hIcon;

        [Flags]
        public enum CMIC
        {
            CMIC_MASK_HOTKEY = 0x00000020,
            CMIC_MASK_ICON = 0x00000010,
            CMIC_MASK_FLAG_NO_UI = 0x00000400,
            CMIC_MASK_NO_CONSOLE = 0x00008000,
            CMIC_MASK_ASYNCOK = 0x00100000,
            CMIC_MASK_NOASYNC = 0x00000100,
            CMIC_MASK_SHIFT_DOWN = 0x10000000,
            CMIC_MASK_CONTROL_DOWN = 0x40000000,
            CMIC_MASK_FLAG_LOG_USAGE = 0x04000000,
            CMIC_MASK_NOZONECHECKS = 0x00800000,
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class CATEGORYINFO
    {
        public Guid catid;
        public uint lcidl;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string szDescription;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct BANDSITEINFO
    {
        public BSIM dwMask;
        public BSSF dwState;
        public BSIS dwStyle;

        [Flags]
        public enum BSIM : uint
        {
            BSIM_STATE = 0x00000001,
            BSIM_STYLE = 0x00000002,
        }

        [Flags]
        public enum BSSF : uint
        {
            BSSF_VISIBLE = 0x00000001,
            BSSF_NOTITLE = 0x00000002,
            BSSF_UNDELETEABLE = 0x00001000,
        }

        [Flags]
        public enum BSIS : uint
        {
            BSIS_AUTOGRIPPER = 0x00000000,
            BSIS_NOGRIPPER = 0x00000001,
            BSIS_ALWAYSGRIPPER = 0x00000002,
            BSIS_LEFTALIGN = 0x00000004,
            BSIS_SINGLECLICK = 0x00000008,
            BSIS_NOCONTEXTMENU = 0x00000010,
            BSIS_NODROPTARGET = 0x00000020,
            BSIS_NOCAPTION = 0x00000040,
            BSIS_PREFERNOLINEBREAK = 0x00000080,
            BSIS_LOCKED = 0x00000100,
            BSIS_PRESERVEORDERDURINGLAYOUT = 0x00000200,
            BSIS_FIXEDORDER = 0x00000400,
        }
    }

    internal enum APPBARMESSAGE : uint
    {
        ABM_NEW = 0x00000000,
        ABM_REMOVE = 0x00000001,
        ABM_QUERYPOS = 0x00000002,
        ABM_SETPOS = 0x00000003,
        ABM_GETSTATE = 0x00000004,
        ABM_GETTASKBARPOS = 0x00000005,
        ABM_ACTIVATE = 0x00000006,
        ABM_GETAUTOHIDEBAR = 0x00000007,
        ABM_SETAUTOHIDEBAR = 0x00000008,
        ABM_WINDOWPOSCHANGED = 0x00000009,
        ABM_SETSTATE = 0x0000000A,
        ABM_GETAUTOHIDEBAREX = 0x0000000B,
        ABM_SETAUTOHIDEBAREX = 0x0000000C,
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct APPBARDATA
    {
        public int cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public uint uEdge;
        public RECT rc;
        public int lParam;
    }

    internal class ComponentCategoryManager
    {
        public static readonly Guid CATID_DESKBAND = new Guid("00021492-0000-0000-C000-000000000046");
        public static readonly Guid CATID_InfoBand = new Guid("00021493-0000-0000-C000-000000000046");

        private static readonly Guid _componentCategoryManager = new Guid("0002e005-0000-0000-c000-000000000046");
        private static readonly ICatRegister _catRegister;
        private Guid _classId;

        static ComponentCategoryManager()
        {
            _catRegister = Activator.CreateInstance(Type.GetTypeFromCLSID(_componentCategoryManager, true)) as ICatRegister;
        }

        private ComponentCategoryManager(Guid classId)
        {
            _classId = classId;
        }

        public static ComponentCategoryManager For(Guid classId)
        {
            return new ComponentCategoryManager(classId);
        }

        public void RegisterCategories(Guid[] categoryIds)
        {
            _catRegister.RegisterClassImplCategories(ref _classId, (uint)categoryIds.Length, categoryIds);
        }

        public void UnRegisterCategories(Guid[] categoryIds)
        {
            _catRegister.UnRegisterClassImplCategories(ref _classId, (uint)categoryIds.Length, categoryIds);
        }
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0002E012-0000-0000-C000-000000000046")]
    internal interface ICatRegister
    {
        [PreserveSig]
        void RegisterCategories(uint cCategories, [MarshalAs(UnmanagedType.LPArray)] CATEGORYINFO[] rgCategoryInfo);

        [PreserveSig]
        void RegisterClassImplCategories([In] ref Guid rclsid, uint cCategories, [MarshalAs(UnmanagedType.LPArray)] Guid[] rgcatid);

        [PreserveSig]
        void RegisterClassReqCategories([In] ref Guid rclsid, uint cCategories, [MarshalAs(UnmanagedType.LPArray)] Guid[] rgcatid);

        [PreserveSig]
        void UnRegisterCategories(uint cCategories, [MarshalAs(UnmanagedType.LPArray)] Guid[] rgcatid);

        [PreserveSig]
        void UnRegisterClassImplCategories([In] ref Guid rclsid, uint cCategories, [MarshalAs(UnmanagedType.LPArray)] Guid[] rgcatid);

        [PreserveSig]
        void UnRegisterClassReqCategories([In] ref Guid rclsid, uint cCategories, [MarshalAs(UnmanagedType.LPArray)] Guid[] rgcatid);
    }

    [Flags]
    internal enum WindowStyles : uint
    {
        WS_BORDER = 0x800000,
        WS_CAPTION = 0xc00000,
        WS_CHILD = 0x40000000,
        WS_CLIPCHILDREN = 0x2000000,
        WS_CLIPSIBLINGS = 0x4000000,
        WS_DISABLED = 0x8000000,
        WS_DLGFRAME = 0x400000,
        WS_GROUP = 0x20000,
        WS_HSCROLL = 0x100000,
        WS_MAXIMIZE = 0x1000000,
        WS_MAXIMIZEBOX = 0x10000,
        WS_MINIMIZE = 0x20000000,
        WS_MINIMIZEBOX = 0x20000,
        WS_OVERLAPPED = 0x0,
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_POPUP = 0x80000000u,
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        WS_SIZEFRAME = 0x40000,
        WS_SYSMENU = 0x80000,
        WS_TABSTOP = 0x10000,
        WS_VISIBLE = 0x10000000,
        WS_VSCROLL = 0x200000
    }

    internal enum WindowMessages
    {
        WM_PAINT = 0x000f,
        WM_NCHITTEST = 0x0084,
    }

    internal enum HitTestMessageResults
    {
        HTCLIENT = 1,
        HTTRANSPARENT = -1,
    }
}
