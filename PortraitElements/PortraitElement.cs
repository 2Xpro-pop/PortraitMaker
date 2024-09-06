using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace PortraitMaker2.PortraitElements;
public abstract class PortraitElement: UserControl
{
    public static readonly StyledProperty<double> XProperty = AvaloniaProperty.Register<PortraitElement, double>(nameof(X));

    public static readonly StyledProperty<double> YProperty = AvaloniaProperty.Register<PortraitElement, double>(nameof(Y));

    public static readonly StyledProperty<double> SizeProperty = AvaloniaProperty.Register<PortraitElement, double>(nameof(Size));

    public static readonly StyledProperty<double> OverrideWidthProperty = AvaloniaProperty.Register<PortraitElement, double>(nameof(OverrideWidth));

    public static readonly StyledProperty<double> OverrideHeightProperty = AvaloniaProperty.Register<PortraitElement, double>(nameof(OverrideHeight));

    public static readonly StyledProperty<IImage> ImageProperty = AvaloniaProperty.Register<PortraitElement, IImage>(nameof(Image));

    public static readonly StyledProperty<IImage> MaskSourceProperty = AvaloniaProperty.Register<PortraitElement, IImage>(nameof(MaskSource));

    public static readonly StyledProperty<bool> IsChildrenRequestPositionVisibleProperty = AvaloniaProperty.Register<PortraitElement, bool>(nameof(IsChildrenRequestPositionVisible));

    public double X
    {
        get => GetValue(XProperty);
        set => SetValue(XProperty, value);
    }

    public double Y
    {
        get => GetValue(YProperty);
        set => SetValue(YProperty, value);
    }

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public IImage Image
    {
        get => GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public bool IsChildrenRequestPositionVisible
    {
        get => GetValue(IsChildrenRequestPositionVisibleProperty);
        set => SetValue(IsChildrenRequestPositionVisibleProperty, value);
    }

    public double OverrideWidth
    {
        get => GetValue(OverrideWidthProperty);
        set => SetValue(OverrideWidthProperty, value);
    }

    public double OverrideHeight
    {
        get => GetValue(OverrideHeightProperty);
        set => SetValue(OverrideHeightProperty, value);
    }

    public IImage MaskSource
    {
        get => GetValue(MaskSourceProperty);
        set => SetValue(MaskSourceProperty, value);
    }
}
