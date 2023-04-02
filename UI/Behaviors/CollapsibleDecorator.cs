using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace eXPerienceBar.UI.Behaviors;

class CollapsibleDecorator : Decorator
{
    private Storyboard storyboard = null;

    static CollapsibleDecorator()
    {
        ClipToBoundsProperty.OverrideMetadata(
            typeof(CollapsibleDecorator), new FrameworkPropertyMetadata(true)
        );
        OpacityProperty.OverrideMetadata(
            typeof(CollapsibleDecorator), new FrameworkPropertyMetadata(0.0)
        );
        FocusableProperty.OverrideMetadata(
            typeof(CollapsibleDecorator), new FrameworkPropertyMetadata(false)
        );
    }

    #region IsExpanded property
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
        "IsExpanded",
        typeof(bool),
        typeof(CollapsibleDecorator),
        new PropertyMetadata(false, new PropertyChangedCallback(OnIsExpandedChanged))
    );

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public static void OnIsExpandedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        CollapsibleDecorator elm = (CollapsibleDecorator)sender;

        bool isExpandedValue = (bool)e.NewValue;

        if (elm.storyboard == null)
        {
            elm.storyboard = new Storyboard();
        }
        else
        {
            elm.storyboard.Stop(elm);
        }

        DoubleAnimation animation = new()
        {
            To = (isExpandedValue) ? 1.0 : 0.0,
            Duration = (elm.IsLoaded)
                ? TimeSpan.FromMilliseconds(elm.AnimationDuration)
                : TimeSpan.FromMilliseconds(0),
            AccelerationRatio = (isExpandedValue) ? 0.0 : 0.33,
            DecelerationRatio = (isExpandedValue) ? 0.33 : 0.0,
            FillBehavior = FillBehavior.HoldEnd
        };

        Storyboard.SetTargetProperty(animation, new PropertyPath(AnimationProgressProperty));

        elm.storyboard.Children.Clear();
        elm.storyboard.Children.Add(animation);

        elm.storyboard.Begin(elm, true);
    }
    #endregion IsExpanded property

    #region AnimationProgress property
    public static readonly DependencyProperty AnimationProgressProperty = DependencyProperty.Register(
        "animationProgress",
        typeof(double),
        typeof(CollapsibleDecorator),
        new FrameworkPropertyMetadata(
            0.0,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            new PropertyChangedCallback(OnAnimationProgressChanged),
            new CoerceValueCallback(CoerceAnimationProgress)
        )
    );

    public double AnimationProgress
    {
        get => (double)GetValue(AnimationProgressProperty);
        set => SetValue(AnimationProgressProperty, value);
    }

    private static void OnAnimationProgressChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        CollapsibleDecorator elm = (CollapsibleDecorator)sender;

        elm.Visibility = elm.AnimationProgress > 0.0
            ? Visibility.Visible
            : Visibility.Hidden;

        elm.Opacity = elm.AnimationProgress;
    }

    private static object CoerceAnimationProgress(DependencyObject sender, object value)
    {
        double animationProgress = (double)value;

        if (animationProgress < 0.0)
        {
            animationProgress = 0.0;
        }
        else if (animationProgress > 1.0)
        {
            animationProgress = 1.0;
        }

        return animationProgress;
    }
    #endregion AnimationProgress property

    #region AnimationDuration property
    public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register(
        "animationDuration",
        typeof(double),
        typeof(CollapsibleDecorator),
        new FrameworkPropertyMetadata(300.0)
    );

    public double AnimationDuration
    {
        get => (double)GetValue(AnimationDurationProperty);
        set => SetValue(AnimationDurationProperty, value);
    }
    #endregion AnimationDuration property

    /**
     * Do NOT read from the constraint as this will cause very serious bugs that are very hard to debug
     * regarding an infinite size. ;-;
     */
    protected override Size MeasureOverride(Size constraint)
    {
        UIElement child = Child;

        if (child != null)
        {
            child.Measure(constraint);
            double animatedHeight = child.DesiredSize.Height * AnimationProgress;

            return new Size(child.DesiredSize.Width, animatedHeight);
        }

        return new Size(0, 0);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        UIElement child = Child;

        if (child != null)
        {
            double newY = child.DesiredSize.Height * (AnimationProgress - 1.0);

            child.Arrange(
                new Rect(new Point(0.0, newY), new Size(arrangeSize.Width, child.DesiredSize.Height))
            );

            return arrangeSize;
        }

        return new Size();
    }
}
