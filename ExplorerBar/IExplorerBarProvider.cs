using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExplorerBar;

/**
 * Interface for a window implementing an ExplorerBar.
 */
public interface IExplorerBarProvider
{
    IntPtr ParentWindowHandle { get; }
    IntPtr BasebarHandle { get; }

    IntPtr? Handle { get; }
    Guid Guid { get; }
    bool HasFocus { get; set; }

    void CreateWindow();
}
