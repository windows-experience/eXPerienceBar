using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace eXPerienceBar.UI.Behaviors;

[TemplatePart(Name = "PartIcon", Type = typeof(Image))]
[TemplatePart(Name = "PartLabel", Type = typeof(TextBlock))]
public class PanelLinkButton : ButtonBase
{
    static PanelLinkButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(PanelLinkButton),
            new FrameworkPropertyMetadata(typeof(PanelLinkButton))
        );
    }

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        name: "Icon",
        propertyType: typeof(ImageSource),
        ownerType: typeof(PanelLinkButton),
        typeMetadata: new PropertyMetadata(null)
    );

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
        name: "Label",
        propertyType: typeof(string),
        ownerType: typeof(PanelLinkButton),
        typeMetadata: new PropertyMetadata(string.Empty)
    );
}