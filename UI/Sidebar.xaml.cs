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
    /// Interaction logic for Sidebar.xaml
    /// </summary>
    public partial class Sidebar : UserControl
    {
        private MainSidebar app;

        public Sidebar(MainSidebar app)
        {
            InitializeComponent();

            this.app = app;

            // Since the debug log is wrote to by multiple threads, it needs to be synchronised like this:
            object lockObj = new { };
            BindingOperations.EnableCollectionSynchronization(app.Debug.Messages, lockObj);
            PartDebugBox.ItemsSource = app.Debug.Messages;
        }

        /// <summary>
        ///     Add a section to the sidebar.
        /// </summary>
        public void AddSection(CollapsibleSection section)
        {
            PartSections.Children.Add(section);
        }
    }
}
