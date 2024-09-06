using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace PortraitMaker2;

public partial class Portrait : UserControl
{


    public Portrait()
    {
        InitializeComponent();
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        // Соотношение сторон 45:64
        var aspectRatio = 45d / 64d;

        var width = availableSize.Width;
        var height = availableSize.Height;

        if (width / height > aspectRatio)
        {
            width = height * aspectRatio;
        }
        else
        {
            height = width / aspectRatio;
        }
        return base.MeasureOverride(new Size(width, height));
    }

    protected override Size ArrangeOverride(Size finalSize)
    { 
        // Соотношение сторон 45:64
        var aspectRatio = 45d / 64d;

        var width = finalSize.Width;
        var height = finalSize.Height;

        if (width / height > aspectRatio)
        {
            width = height * aspectRatio;
        }
        else
        {
            height = width / aspectRatio;
        }

        return base.ArrangeOverride(new Size(width, height));
    }
}