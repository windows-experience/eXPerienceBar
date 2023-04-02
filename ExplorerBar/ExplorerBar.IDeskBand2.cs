using ExplorerBar.Interop;
using System;

namespace ExplorerBar;

sealed partial class ExplorerBar : IDeskBand2
{
    public int CanRenderComposited(out bool a)
    {
        a = true;
        return HRESULT.S_OK;
    }

    public int SetCompositionState(bool a)
    {
        return HRESULT.S_OK;
    }

    public int GetCompositionState(out bool a)
    {
        a = true;
        return HRESULT.S_OK;
    }

    public int GetCommandString(ref uint idcmd, uint uflags, ref uint pwReserved, out string pcszName, uint cchMax)
    {
        pcszName = "";
        return HRESULT.E_NOTIMPL;
    }

    public int HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam)
    {
        return HandleMenuMsg2(uMsg, wParam, lParam, out var i);
    }

    public int HandleMenuMsg2(uint uMsg, IntPtr wParam, IntPtr lParam, out IntPtr plResult)
    {
        plResult = IntPtr.Zero;
        return HRESULT.S_OK;
    }
}
