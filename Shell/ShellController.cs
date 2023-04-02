using eXPerienceBar.DebuggerLib;
using SHDocVw;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace eXPerienceBar.Shell;

/// <summary>
///     A main controller for Windows Explorer (Shell) interaction, used by toolbar instances.
/// </summary>
/// 
/// <author>
///     Taniko Yamamoto (kirasicecreamm@gmail.com)
/// </author>
public class ShellController
{
    private bool finishedConstruction = false;
    private InternetExplorer? shellWindow;
    private readonly Debugger? debug;

    public event EventHandler<NavigateEventArgs>? OnNavigate;

    public class NavigateEventArgs : EventArgs
    {
        public string? NewPath { get; private set; }

        public NavigateEventArgs(string? newPath)
        {
            NewPath = newPath;
        }
    }

    /// <summary>
    ///     Constructs a new Windows Explorer interaction controller.
    /// </summary>
    /// 
    /// <param name="childHwnd">
    ///     A handle of a child window of an Explorer process, which is used to get a handle to
    ///     the parent Explorer window for control.
    /// </param>
    public ShellController(IntPtr childHwnd, Debugger? debug = null)
    {
        this.debug = debug;
        debug?.AddMessage("Hooked ShellController!");

        ConstructAsync(childHwnd);
    }

    private async void ConstructAsync(IntPtr childHwnd)
    {
        if (finishedConstruction) return;

        shellWindow = await GetExplorerHandler(childHwnd);
        shellWindow.NavigateComplete2 += HandleNavigateComplete2Event;

        finishedConstruction = true;
    }

    public string? GetCurrentPath()
    {
        if (shellWindow == null) return null;

        Shell32.IShellFolderViewDual2 document = shellWindow.Document;

        if (document?.Folder != null)
        {
            return document.Folder.Items().Item().Path;
        }
        else
        {
            return null;
        }
    }

    private async Task<InternetExplorer> GetExplorerHandler(IntPtr childHwnd)
    {
        debug?.AddMessage("Running async ShellController.GetExplorerHandler");
        IntPtr baseHwnd = GetAncestor(childHwnd, GetAncestorFlags.GetRootOwner);

        DateTime timeout = DateTime.Now + TimeSpan.FromSeconds(5);

        while (DateTime.Now < timeout)
        {
            ShellWindows shellWindows = new();
            debug?.AddMessage($"shellWindows.Count = {shellWindows.Count}");

            foreach (InternetExplorer explorer in shellWindows)
            {
                debug?.AddMessage($"Looking at explorer hwnd: {explorer.HWND:X8}");

                if (explorer.HWND == (int)baseHwnd)
                {
                    return explorer;
                }
            }

            await Task.Delay(10);
        }

        debug?.AddMessage("ジョワリだ。");

        throw new Exception(baseHwnd.ToString("X8"));
    }

    /// <summary>
    ///     Handles the Internet Explorer event "NavigateComplete2", which is simply used to message
    ///     our own navigation event.
    /// </summary>
    private void HandleNavigateComplete2Event(object sender, ref object url)
    {
        // The URL given is useless since the Internet Explorer URL format isn't compatible with
        // the File Explorer ones. Therefore, we look up the File Explorer ones on our own.
        OnNavigate?.Invoke(this, new NavigateEventArgs(GetCurrentPath()));
    }

    // move later

    public enum GetAncestorFlags
    {
        GetParent = 1,
        GetRoot = 2,
        GetRootOwner = 3
    }

    [DllImport("user32.dll")]
    public static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags gaFlags);
}
