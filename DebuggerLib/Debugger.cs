using eXPerienceBar.Utils;
using ExplorerBar.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eXPerienceBar.DebuggerLib;

/// <summary>
///     A very simple debugger for logging messages during runtime.
/// </summary>
/// 
/// <author>
///     Taniko Yamamoto (kirasicecreamm@gmail.com)
/// </author>
public class Debugger
{
    private readonly MainSidebar app;

    public ObservableCollection<DebuggerLog> Messages { get; private set; } = new();

    public Debugger(MainSidebar app)
    {
        this.app = app;
    }

    public void AddMessage(string message)
    {
        Messages.Add(new DebuggerLog(message));
    }

    public void AddMessage(object message) => AddMessage(message?.ToString() ?? "<null>");
}
