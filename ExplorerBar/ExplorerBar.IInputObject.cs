using ExplorerBar.Interop;

namespace ExplorerBar;

sealed partial class ExplorerBar : IInputObject
{
    public int UIActivateIO(int fActivate, ref MSG msg)
    {
        provider.HasFocus = fActivate != 0;
        updateFocus(provider.HasFocus);
        return HRESULT.S_OK;
    }

    public int HasFocusIO()
    {
        return provider.HasFocus ? HRESULT.S_OK : HRESULT.S_FALSE;
    }

    public int TranslateAcceleratorIO(ref MSG msg)
    {
        return HRESULT.S_OK;
    }

    /**
     * Helper function for updating focus.
     */
    private void updateFocus(bool focused)
    {
        (parentSite as IInputObjectSite)?.OnFocusChangeIS(this, focused ? 1 : 0);
    }
}
