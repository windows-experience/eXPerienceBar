using ExplorerBar.Interop;

namespace ExplorerBar;

/// <summary>
///     Composite interface for IDeskBand, IObjectWithSite, IPersistStream, and
///     IInputObject.
/// </summary>
internal interface IExplorerBar : 
        IDeskBand, 
        IDeskBand2, 
        IObjectWithSite, 
        IPersistStream, 
        IInputObject,
        IContextMenu {}