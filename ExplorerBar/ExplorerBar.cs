using ExplorerBar.Interop;
using System;
using System.Runtime.InteropServices;

namespace ExplorerBar;

internal sealed partial class ExplorerBar : IExplorerBar
{
    internal IntPtr ParentWindowHandle;
    internal IntPtr BasebarHandle;
    private readonly IExplorerBarProvider provider;
    private object parentSite;

    internal event EventHandler OnClosed;

    public ExplorerBar(IExplorerBarProvider p)
    {
        provider = p;
    }

    public int InvokeCommand(IntPtr pici)
    {
        var commandInfo = Marshal.PtrToStructure<CMINVOKECOMMANDINFO>(pici);
        #pragma warning disable CS0219 // Variable is assigned but its value is never used
        var isUnicode = false;
        var isExtended = false;
        #pragma warning restore CS0219 // Variable is assigned but its value is never used
        var verbPtr = commandInfo.lpVerb;

        if (commandInfo.cbSize == Marshal.SizeOf<CMINVOKECOMMANDINFOEX>())
        {
            isExtended = true;

            var extended = Marshal.PtrToStructure<CMINVOKECOMMANDINFOEX>(pici);
            if (extended.fMask.HasFlag(CMINVOKECOMMANDINFOEX.CMIC.CMIC_MASK_UNICODE))
            {
                isUnicode = true;
                verbPtr = extended.lpVerbW;
            }
        }

        if (User32.HiWord(commandInfo.lpVerb.ToInt32()) != 0)
        {
            // TODO verbs
            return HRESULT.E_FAIL;
        }

        var cmdIndex = User32.LoWord(verbPtr.ToInt32());

        return HRESULT.S_OK;
    }
}