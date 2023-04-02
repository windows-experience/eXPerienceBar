using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ExplorerBar.Interop;

namespace eXPerienceBar
{
    internal class Native
    {
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        /**
         * Used to set the position of the sidebar window and cover up the native controls
         * displayed.
         */
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        public const int GWL_STYLE = -16;

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        // This static method is required because Win32 does not support
        // GetWindowLongPtr directly
        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        // This static method is required because legacy OSes do not support
        // SetWindowLongPtr
        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        // https://www.appsloveworld.com/csharp/100/1729/get-selected-files-from-desktop
        #region Shell
        [Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IServiceProvider
        {
            [return: MarshalAs(UnmanagedType.IUnknown)]
            object QueryService([MarshalAs(UnmanagedType.LPStruct)] Guid service, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);
        }

        // note: for the following interfaces, not all methods are defined as we don't use them here
        [Guid("000214E2-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellBrowser
        {
            void _VtblGap1_12(); // skip 12 methods https://stackoverflow.com/a/47567206/403671

            [return: MarshalAs(UnmanagedType.IUnknown)]
            object QueryActiveShellView();
        }

        [Guid("cde725b0-ccc9-4519-917e-325d72fab4ce"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFolderView
        {
            void _VtblGap1_5(); // skip 5 methods

            [PreserveSig]
            int Items(SVGIO uFlags, Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object items);
        }

        [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem
        {
            [return: MarshalAs(UnmanagedType.IUnknown)]
            object BindToHandler(System.Runtime.InteropServices.ComTypes.IBindCtx pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid bhid, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);

            IShellItem GetParent();

            [return: MarshalAs(UnmanagedType.LPWStr)]
            string GetDisplayName(SIGDN sigdnName);

            // 2 other methods to be defined
        }

        [Guid("b63ea76d-1f85-456f-a19c-48159efa858b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItemArray
        {
            void _VtblGap1_4(); // skip 4 methods

            int GetCount();
            IShellItem GetItemAt(int dwIndex);
        }

        private enum SIGDN
        {
            SIGDN_NORMALDISPLAY,
            SIGDN_PARENTRELATIVEPARSING,
            SIGDN_DESKTOPABSOLUTEPARSING,
            SIGDN_PARENTRELATIVEEDITING,
            SIGDN_DESKTOPABSOLUTEEDITING,
            SIGDN_FILESYSPATH,
            SIGDN_URL,
            SIGDN_PARENTRELATIVEFORADDRESSBAR,
            SIGDN_PARENTRELATIVE,
            SIGDN_PARENTRELATIVEFORUI
        }

        private enum SVGIO
        {
            SVGIO_BACKGROUND,
            SVGIO_SELECTION,
            SVGIO_ALLVIEW,
            SVGIO_CHECKED,
            SVGIO_TYPE_MASK,
            SVGIO_FLAG_VIEWORDER
        }
        #endregion
    }
}
