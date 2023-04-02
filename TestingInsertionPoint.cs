using eXPerienceBar.UI;
using ExplorerBar.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace eXPerienceBar;

public class TestingInsertionPoint
{
    [STAThread]
    public static void Main(string[] args)
    {
        Sidebar app = new(new MainSidebar());
        TestingWindow mainWindow = new(app);

        mainWindow.ShowDialog();
    }
}
