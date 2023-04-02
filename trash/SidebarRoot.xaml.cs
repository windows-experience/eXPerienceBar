using eXPerienceBar.Shell;
using eXPerienceBar.UI.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eXPerienceBar.UI
{
    /// <summary>
    /// Interaction logic for SidebarRoot.xaml
    /// </summary>
    public partial class SidebarRoot : UserControl
    {
        private MainSidebar app;

        public static readonly DependencyProperty DebugProperty = DependencyProperty.Register(
            "Debug",
            typeof(List),
            typeof(CollapsibleSection),
            new PropertyMetadata(false)
        );

        public SidebarRoot(MainSidebar app, ShellController shell)
        {
            InitializeComponent();

            this.app = app;
            GuiController.PartDebugBox.ItemsSource = app.Debug.Messages;
        }

        /// <summary>
        ///     Gets the GUI controller of the sidebar.
        /// </summary>
        public Sidebar GuiController { get => _guiController; private set {} }
    }
}
