using ExplorerBar.Interop;
using System;

namespace ExplorerBar;

sealed partial class ExplorerBar : IContextMenu
{
    public int QueryContextMenu(IntPtr hMenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, QueryContextMenuFlags uFlags)
    {
        return 0;
    }
}