using eXPerienceBar.UI.Behaviors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SHDocVw;
using System.Security.Cryptography.X509Certificates;
using System.Linq.Expressions;

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

        private void PanelLinkButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void PanelLinkButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void PanelLinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            string path = @"C:\";
            try
            {
                app.ShellController.NavigateTo(path);
            }
            catch (Exception ex)
            {
                app.Debug.AddMessage(ex.ToString);
                app.Debug.AddMessage(path);
            }
        }

        private void PanelLinkButton_Click_3(object sender, RoutedEventArgs e)
        {
            try
            { 
                string? pathhh = app.ShellController.GetCurrentPath();
                MessageBox.Show(
                string.Format(pathhh),
                "Information Message",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
            catch(Exception exx)
            {
                app.Debug.AddMessage(exx.ToString);
            }
        }
    }
}
