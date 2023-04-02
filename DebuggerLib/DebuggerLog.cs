using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eXPerienceBar.DebuggerLib;

public class DebuggerLog
{
    public DebuggerLog(string message)
    {
        Time = DateTime.Now;
        Message = message;
    }

    public DateTime Time { get; private set; }
    public string Message { get; private set; }

    public override string ToString()
    {
        return $"[{Time:HH:mm:ss}] {Message}";
    }
}
