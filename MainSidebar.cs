using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using eXPerienceBar.DebuggerLib;
using eXPerienceBar.Utils;
using eXPerienceBar.Utils.COM;
using ExplorerBar;
using ExplorerBar.Interop;
using System.Windows.Controls;
using eXPerienceBar.Shell;
using System.Security.Policy;

namespace eXPerienceBar;

/**
 * Implements the XP Sidebar proper. This is the primary insertion point for the program.
 * 
 * An ELI5 of how this works for the noobs looking through this source code (I know
 * I needed documentation when I was making this):
 * 
 *      - The sidebar is based on COM. This is an ancient, low-level Windows technology
 *        that everyone worked hard to replace in their own way, but remnants of it still
 *        remain in Windows programming. Although .net is built with interfacing with COM
 *        in mind, it's still a bit clunky. COM works through these weird ass unique GUIDs
 *        that identify each class. Apart from that, there's some evil configuration in the
 *        Solution file for this, and two DLLs will actually be spat out after compiling:
 *        .dll & .comhost.dll
 *        
 *              - The ComVisible and Guid attributes on this class help expose it to the
 *                COM world. This extends an ExplorerBarWpfProvider, which implements the
 *                actual COM registration functions (see also: ExplorerBar.RegistrationHelper).
 *                In order to register the toolbar at all, you need to run regsvr32 on the
 *                .comhost.dll binary after building. You can't register the plain .dll.
 *                
 *              - As such, this program doesn't have a Main function or anything of the sort.
 *                This class serves as the insertion point, and it is constructed by Explorer
 *                when accessing the GUID. Generally it's a good idea to avoid static properties 
 *                as they can cause memory access violations @_@
 *                
 *              - Although this program uses WPF, you'll actually notice that it isn't compiled
 *                as a Windows GUI program at all, also because this relies on COM. The main
 *                window is a user control that displays with a HwndSource.
 *                
 *      - There is no such thing as Windows Explorer. Get that in your head. It's Internet Explorer
 *        all the way down, and you'd be foolish to think otherwise. The "Desk Band" control that
 *        this implements is actually an "Explorer Bar" for Internet Explorer, and all APIs that
 *        can be used to interface with Windows Explorer are actually made for IE.
 *        
 *      - The Explorer Bar this implements actually cannot open by itself every time a new Explorer
 *        window is opened. In order to work around this, a Browser Helper Object (also an IE thing)
 *        has also been implemented, which simply hooks every Explorer window and opens the Explorer
 *        Bar for every new window. This workaround was inspired by that seen in FindeXer, an ancient
 *        custom sidebar for Windows Explorer made back in the Windows XP days.
 *                
 *      - Windows actually doesn't support custom Desk Bands (Toolbars, Explorer Bars, etc.)
 *        natively anymore. In order to get the option to enable them, you will need to install
 *        Classic Shell/Open Shell to patch Explorer to enable the option.
 * 
 * @author Taniko Yamamoto <kirasicecreamm@gmail.com>
 */
[ComVisible(true)] // <- Exports this class to the hell of classes that is COM.
[Guid("09B24772-ADCF-4C1F-A23D-7B625A7FE635")] // <- Randomly generated unique ID for this class.
[ComRegistrationInfo(DisplayName = "eXPerienceBar")]
public class MainSidebar : ExplorerBarWpfProvider, IMainSidebarComServer
{
    private readonly UI.Sidebar sidebarWindow;

    public MainSidebar() : base()
    {
        AppDomain.CurrentDomain.UnhandledException += CatchUnhandledException;

        Debug = new(this);
        sidebarWindow = new(this);

        Debug.AddMessage("Started sidebar process.");
    }

    private void CatchUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        MessageBox.Show(e.ExceptionObject.ToString());
    }

    protected override void HandleWindowCreated()
    {
        try
        {
            Debug.AddMessage($"Sidebar window created; handling...");
            Debug.AddMessage($"ParentWindowHandle: {ParentWindowHandle:X8}");

            try
            {
                ShellController = new(ParentWindowHandle, Debug);
            }
            catch (Exception e)
            {
                Debug.AddMessage("Failed to construct ShellController: " + e.ToString());
            }

            if (null != ShellController)
            {
                ShellController.OnNavigate += HandleNavigate;
                Debug.AddMessage($"Navigate event handler installed.");
            }
            else
            {
                Debug.AddMessage("ShellController not loaded.");
            }

            Debug.AddMessage($"Finished sidebar installation.");
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
    }

    internal Debugger Debug { get; private set; }
    internal ShellController? ShellController { get; private set; } 

    private void HandleNavigate(object? sender, ShellController.NavigateEventArgs e)
    {
        Debug.AddMessage($"Navigated to: {e.NewPath ?? "<unknown path>"}");
    }

    public override UI.Sidebar UiElement => sidebarWindow;

    protected override IntPtr HwndSourceHook(
            IntPtr hwnd, 
            int msg, 
            IntPtr wparam, 
            IntPtr lparam, 
            ref bool handled
    )
    {
        // Native styling needs to be reapplied every paint procedure, otherwise redrawing the
        // window will override the sizing.
        if (msg == (int)WindowMessages.WM_PAINT) // try WM_SIZING too for some artifact removal
        {
            ExplorerBarNativeStyle.ApplyNativeStyling(
                this, ParentWindowHandle, BasebarHandle     
            );
        }

        handled = false;
        return IntPtr.Zero;
    }
}
