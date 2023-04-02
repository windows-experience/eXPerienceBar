using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eXPerienceBar.UI.Behaviors;

/// <summary>
///     Controller code for the collapsible sections, as seen on the XP Explorer sidebar.
/// </summary>
[TemplatePart(Name = "PartHeader", Type = typeof(ToggleButton))]
[TemplatePart(Name = "PartContent", Type = typeof(CollapsibleDecorator))]
public class CollapsibleSection : HeaderedContentControl
{
    /// <summary>
    ///     Initialises the component.
    /// </summary>
    static CollapsibleSection()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(CollapsibleSection), 
            new FrameworkPropertyMetadata( typeof(CollapsibleSection) )
        );
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        ToggleButton header = GetTemplateChild("PartHeader") as ToggleButton;
        if (null != header)
        {
            Binding isChecked = new Binding();
            isChecked.RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent);
            isChecked.Path = new PropertyPath("IsExpanded");
            isChecked.Mode = BindingMode.TwoWay;

            header.SetBinding(ToggleButton.IsCheckedProperty, isChecked);
        }

        CollapsibleDecorator content = GetTemplateChild("PartContent") as CollapsibleDecorator;
        if (null != content)
        {
            Binding isExpanded = new Binding();
            isExpanded.RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent);
            isExpanded.Path = new PropertyPath("IsExpanded");

            content.SetBinding(CollapsibleDecorator.IsExpandedProperty, isExpanded);
        }
    }

    #region IsExpanded property
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
        "IsExpanded",
        typeof(bool),
        typeof(CollapsibleSection),
        new PropertyMetadata(false, new PropertyChangedCallback(OnIsExpandedChanged))
    );

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    private static void OnIsExpandedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        CollapsibleSection section = (CollapsibleSection)sender;

        if ((null == section) || (null == section.Template)) return;

        bool newValue = (bool)e.NewValue;

        if (newValue) section.RaiseEvent(
            new RoutedEventArgs(ExpandedEvent, section)
        );
    }
    #endregion IsExpanded property

    #region Expanded event
    public static readonly RoutedEvent ExpandedEvent = EventManager.RegisterRoutedEvent(
        "OnExpanded",
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(CollapsibleSection)
    );

    public event RoutedEventHandler OnExpanded
    {
        add => AddHandler(ExpandedEvent, value);
        remove => RemoveHandler(ExpandedEvent, value);
    }
    #endregion Expanded event
}
