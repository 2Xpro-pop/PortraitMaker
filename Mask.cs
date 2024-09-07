using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using SkiaSharp;

namespace PortraitMaker2;
public sealed class Mask : Control
{
    public static readonly StyledProperty<IImage> SourceProperty = AvaloniaProperty.Register<Mask, IImage>(nameof(Source));

    public static readonly StyledProperty<Color> PrimaryColorProperty = AvaloniaProperty.Register<Mask, Color>(nameof(PrimaryColor), Colors.White);

    public static readonly StyledProperty<Color> SecondaryColorProperty = AvaloniaProperty.Register<Mask, Color>(nameof(SecondaryColor), Colors.Black);


    static Mask()
    {
        AffectsRender<Mask>(SourceProperty, PrimaryColorProperty, SecondaryColorProperty);
    }

    public IImage Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public Color PrimaryColor
    {
        get => GetValue(PrimaryColorProperty);
        set => SetValue(PrimaryColorProperty, value);
    }

    public Color SecondaryColor
    {
        get => GetValue(SecondaryColorProperty);
        set => SetValue(SecondaryColorProperty, value);
    }

    public Mask()
    {
        RenderOptions.SetBitmapInterpolationMode(this, BitmapInterpolationMode.None);
    }

    public override void Render(DrawingContext context)
    {
        var bounds = Bounds;
        var source = Source;
        var primaryColor = PrimaryColor;
        var secondaryColor = SecondaryColor;
        var stretch = Stretch.UniformToFill;
        var stretchDirection = StretchDirection.Both;

        if (source is not Bitmap bitmapSource)
            return;

        var pixelSize = bitmapSource.Size;

        var stride = (int)pixelSize.Width * 4;
        var bufferSize = stride * (int)pixelSize.Height;
        var bufferPtr = Marshal.AllocHGlobal(bufferSize);
        var targetPtr = Marshal.AllocHGlobal(bufferSize);

        try
        {
            bitmapSource.CopyPixels(new PixelRect(0, 0, (int)pixelSize.Width, (int)pixelSize.Height), bufferPtr, bufferSize, stride);

            unsafe
            {
                var buffer = (byte*)bufferPtr.ToPointer();
                var targetBuffer = (byte*)targetPtr.ToPointer();

                for (var y = 0; y < (int)pixelSize.Height; y++)
                {
                    for (var x = 0; x < (int)pixelSize.Width; x++)
                    {
                        var pixelIndex = (y * stride) + (x * 4);

                        var blue = buffer[pixelIndex];
                        var green = buffer[pixelIndex + 1];
                        var red = buffer[pixelIndex + 2];
                        var alpha = buffer[pixelIndex + 3];

                        if (red == 255 && green == 255 && blue == 255)
                        {
                            targetBuffer[pixelIndex] = primaryColor.B;
                            targetBuffer[pixelIndex + 1] = primaryColor.G;
                            targetBuffer[pixelIndex + 2] = primaryColor.R;
                            targetBuffer[pixelIndex + 3] = primaryColor.A;
                        }
                        else if (red == 0 && green == 0 && blue == 0 && alpha != 0)
                        {
                            targetBuffer[pixelIndex] = secondaryColor.B;
                            targetBuffer[pixelIndex + 1] = secondaryColor.G;
                            targetBuffer[pixelIndex + 2] = secondaryColor.R;
                            targetBuffer[pixelIndex + 3] = secondaryColor.A;
                        }
                        else
                        {
                            targetBuffer[pixelIndex] = 0;
                            targetBuffer[pixelIndex + 1] = 0;
                            targetBuffer[pixelIndex + 2] = 0;
                            targetBuffer[pixelIndex + 3] = 0;
                        }
                    }
                }
            }
            var bytes = new byte[bufferSize];
            
            var resultBitmap = new Bitmap(PixelFormat.Bgra8888, AlphaFormat.Premul, targetPtr, bitmapSource.PixelSize, bitmapSource.Dpi, stride);

            if (resultBitmap != null && bounds.Width > 0 && bounds.Height > 0)
            {
                var viewPort = new Rect(bounds.Size);
                var sourceSize = resultBitmap.Size;

                var scale = stretch.CalculateScaling(Bounds.Size, sourceSize, stretchDirection);
                var scaledSize = sourceSize * scale;
                var destRect = viewPort
                    .CenterRect(new Rect(scaledSize))
                    .Intersect(viewPort);
                var sourceRect = new Rect(sourceSize)
                    .CenterRect(new Rect(destRect.Size / scale));

                context.DrawImage(resultBitmap, sourceRect, destRect);
            }
        }
        finally
        {
            Marshal.FreeHGlobal(bufferPtr);
            Marshal.FreeHGlobal(targetPtr);
        }

        
    }

    private sealed class MaskDrawer : ICustomDrawOperation
    {
        private readonly Rect _bounds;
        private readonly IImage _source;
        private readonly Color _primaryColor;
        private readonly Color _secondaryColor;

        public MaskDrawer(Rect bounds, IImage source, Color primaryColor, Color secondaryColor)
        {
            _bounds = bounds;
            _source = source;
            _primaryColor = primaryColor;
            _secondaryColor = secondaryColor;
        }

        public Rect Bounds => _bounds;

        public void Dispose()
        {
        }
        public bool Equals(ICustomDrawOperation? other) => false;
        public bool HitTest(Point p) => false;
        public void Render(ImmediateDrawingContext context)
        {
            if (context.TryGetFeature<ISkiaSharpApiLeaseFeature>() is not ISkiaSharpApiLeaseFeature skiaSharpApiLeaseFeature)
            {
                return;
            }

            using var skiaSharpApi = skiaSharpApiLeaseFeature.Lease();

            var skiaCanvas = skiaSharpApi.SkCanvas;

            skiaCanvas.Save();

            if (_source is not Bitmap bitmapSource)
                return;

            var pixelSize = bitmapSource.Size;
            var stride = (int)pixelSize.Width * 4;
            var bufferSize = stride * (int)pixelSize.Height;

            var bufferPtr = Marshal.AllocHGlobal(bufferSize);

            try
            {
                bitmapSource.CopyPixels(new PixelRect(0, 0, (int)pixelSize.Width, (int)pixelSize.Height), bufferPtr, bufferSize, stride);
                var skBitmap = new SKBitmap((int)pixelSize.Width, (int)pixelSize.Height, SKColorType.Bgra8888, SKAlphaType.Premul);

                unsafe
                {
                    var buffer = (byte*)bufferPtr.ToPointer();
                    for (var y = 0; y < (int)pixelSize.Height; y++)
                    {
                        for (var x = 0; x < (int)pixelSize.Width; x++)
                        {
                            var pixelIndex = (y * stride) + (x * 4);

                            var blue = buffer[pixelIndex];
                            var green = buffer[pixelIndex + 1];
                            var red = buffer[pixelIndex + 2];
                            var alpha = buffer[pixelIndex + 3];

                            if (red == 255 && green == 255 && blue == 255)
                            {
                                skBitmap.SetPixel(x, y, new SKColor(_primaryColor.R, _primaryColor.G, _primaryColor.B, _primaryColor.A));
                            }
                            else if (red == 0 && green == 0 && blue == 0 && alpha != 0)
                            {
                                skBitmap.SetPixel(x, y, new SKColor(_secondaryColor.R, _secondaryColor.G, _secondaryColor.B, _secondaryColor.A));
                            }
                            else
                            {
                                skBitmap.SetPixel(x, y, SKColors.Transparent);
                            }
                        }
                    }
                }

                using var resultImage = SKImage.FromBitmap(skBitmap);
                skiaCanvas.DrawImage(resultImage, new SKPoint(0, 0));

            }
            finally
            {
                skiaCanvas.Restore();
                Marshal.FreeHGlobal(bufferPtr);
            }
        }
    }
}
