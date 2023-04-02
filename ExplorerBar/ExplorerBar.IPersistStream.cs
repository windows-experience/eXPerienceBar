using ExplorerBar.Interop;
using System;

namespace ExplorerBar;

sealed partial class ExplorerBar : IPersistStream
{
    public int GetClassID(out Guid pClassID)
    {
        pClassID = provider.Guid;
        return HRESULT.S_OK;
    }

    public int GetSizeMax(out ulong pcbSize)
    {
        pcbSize = 0;
        return HRESULT.S_OK;
    }

    public int IsDirty()
    {
        return HRESULT.S_OK;
    }

    public int Load(object pStm)
    {
        return HRESULT.S_OK;
    }

    public int Save(IntPtr pStm, bool fClearDirty)
    {
        return HRESULT.S_OK;
    }
}
