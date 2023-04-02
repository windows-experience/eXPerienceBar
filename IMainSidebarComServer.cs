using System;
using System.Runtime.InteropServices;

namespace eXPerienceBar;

/// <summary>
///     Implements the COM server interface (IUnknown) for the main sidebar.
/// </summary>
[ComVisible(true)]
[Guid("09B24772-ADCF-4C1F-A23D-7B625A7FE635")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMainSidebarComServer {}