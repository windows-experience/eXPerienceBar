using ExplorerBar.Interop;
using System;
using System.Runtime.InteropServices;
using static ExplorerBar.Interop.DESKBANDINFO;

namespace ExplorerBar;

// IDeskBand implementations for ExplorerBar.
sealed partial class ExplorerBar : IDeskBand
{
    public int GetWindow(out IntPtr pHwnd)
    {
        if (null != provider.Handle)
        {
            pHwnd = (IntPtr)provider.Handle;
            return HRESULT.S_OK;
        }
        else
        {
            pHwnd = new IntPtr(0);
            return HRESULT.E_FAIL;
        }
    }

    public int GetBandInfo(uint dwBandId, DBIF dwViewMode, ref DESKBANDINFO pDeskbandInfo)
    {
        if (pDeskbandInfo.dwMask.HasFlag(DBIM.DBIM_MINSIZE))
        {
            pDeskbandInfo.ptMinSize.X = 200;
            pDeskbandInfo.ptMinSize.Y = 30;
        }

        if (pDeskbandInfo.dwMask.HasFlag(DBIM.DBIM_MAXSIZE))
        {
            pDeskbandInfo.ptMaxSize.Y = -1;
        }

        if (pDeskbandInfo.dwMask.HasFlag(DBIM.DBIM_INTEGRAL))
        {
            pDeskbandInfo.ptIntegral.Y = 1;
        }

        if (pDeskbandInfo.dwMask.HasFlag(DBIM.DBIM_ACTUAL))
        {
            pDeskbandInfo.ptActual.X = 200;
            pDeskbandInfo.ptActual.Y = 30;
        }

        if (pDeskbandInfo.dwMask.HasFlag(DBIM.DBIM_TITLE))
        {
            pDeskbandInfo.dwMask &= ~DBIM.DBIM_TITLE;
        }

        if (pDeskbandInfo.dwMask.HasFlag(DBIM.DBIM_MODEFLAGS))
        {
            pDeskbandInfo.dwModeFlags = DBIMF.DBIMF_NORMAL | DBIMF.DBIMF_VARIABLEHEIGHT;
        }

        if (pDeskbandInfo.dwMask.HasFlag(DBIM.DBIM_BKCOLOR))
        {
            pDeskbandInfo.dwMask &= ~DBIM.DBIM_BKCOLOR;
        }

        return HRESULT.S_OK;
    }

    public int ContextSensitiveHelp(bool a)
    {
        return HRESULT.E_NOTIMPL;
    }

    public int ShowDW(bool fShow)
    {
        return HRESULT.S_OK;
    }

    public int CloseDW(uint dwReserved)
    {
        OnClosed?.Invoke(this, null);
        return HRESULT.S_OK;
    }

    public int ResizeBorderDW(RECT prcBorder, [In, MarshalAs(UnmanagedType.IUnknown)] IntPtr pUnkToolbarSite, bool fReserved)
    {
        return HRESULT.E_NOTIMPL;
    }
}
