//
// QR code generator library (.NET)
// https://github.com/manuelbl/QrCodeGenerator
//
// Copyright (c) 2021 Manuel Bleichenbacher
// Licensed under MIT License
// https://opensource.org/licenses/MIT
//

using LibQrGenerator.Enums;
using Net.Codecrete.QrCodeGenerator;
using SkiaSharp;

namespace LibQrGenerator.QrDrawerFactory
{
    public class SkiaSharp : GenericDrawer
    {

        private SKColor ConvForeColor = SKColors.Black;
        private SKColor ConvBackColor = SKColors.White;


        /// <inheritdoc cref="ToBitmap(QrCode, int, int)"/>
        /// <param name="background">The background color.</param>
        /// <param name="foreground">The foreground color.</param>
        public SKBitmap BuildImage()
        {
            // check arguments
            if (Scale <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Scale), "Value out of range");
            }
            if (Border < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Border), "Value out of range");
            }

            int size = QrCode.Size;
            int dim = (size + Border * 2) * Scale;

            if (dim > short.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(Scale), "Scale or border too large");
            }

            // create bitmap
            SKBitmap bitmap = new SKBitmap(dim, dim, SKColorType.Rgba8888, SKAlphaType.Opaque);

            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                // draw background
                using (SKPaint paint = new SKPaint { Color = ConvBackColor })
                {
                    canvas.DrawRect(0, 0, dim, dim, paint);
                }

                // draw modules
                using (SKPaint paint = new SKPaint { Color = ConvForeColor })
                {
                    for (int y = 0; y < size; y++)
                    {
                        for (int x = 0; x < size; x++)
                        {
                            if (QrCode.GetModule(x, y))
                            {
                                canvas.DrawRect(
                                    (x + Border) * Scale,
                                    (y + Border) * Scale,
                                    Scale,
                                    Scale,
                                    paint
                                    );
                            }
                        }
                    }
                }
            }

            return bitmap;
        }

        private void ConvertColor()
        {

            SKColor.TryParse(ForegroundColor, out ConvForeColor);
            SKColor.TryParse(BackgroundColor, out ConvBackColor);

        }

        public override GenericDrawer SetQr(QrCode QrCode)
        {
            this.QrCode = QrCode;
            return this;
        }

        public override GenericDrawer SetScale(int Scale)
        {
            this.Scale = Scale;
            return this;
        }

        public override GenericDrawer SetBorder(int Border)
        {
            this.Border = Border;
            return this;
        }

        public override GenericDrawer SetForegroundColor(string Color)
        {
            this.ForegroundColor = Color;

            ConvertColor();

            return this;
        }

        public override GenericDrawer SetBackgroundColor(string Color)
        {
            this.BackgroundColor = Color;

            ConvertColor();

            return this;
        }

        public override GenericDrawer SetImageFormat(CommonImageFormat ImageFormat)
        {
            this.ImageFormat = ImageFormat;
            return this;
        }

        public override byte[] GetBytes()
        {

            using SKBitmap image = BuildImage();
            
            if(ImageFormat == CommonImageFormat.Jpeg)
            {
                using SKData data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
                return data.ToArray();
            }
            else if (ImageFormat == CommonImageFormat.Bmp)
            {
                using SKData data = image.Encode(SKEncodedImageFormat.Bmp, 100);
                return data.ToArray();
            }
            else if (ImageFormat == CommonImageFormat.Webp)
            {
                using SKData data = image.Encode(SKEncodedImageFormat.Webp, 100);
                return data.ToArray();
            }
            else
            {
                using SKData data = image.Encode(SKEncodedImageFormat.Png, 100);
                return data.ToArray();
            }
            
        }

        public override bool Save(string Path)
        {
            using SKBitmap image = BuildImage();

            if(ImageFormat == CommonImageFormat.Jpeg)
            {
                using SKData data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
                using FileStream stream = File.OpenWrite(Path);
                data.SaveTo(stream);
            }
            else if (ImageFormat == CommonImageFormat.Bmp)
            {
                using SKData data = image.Encode(SKEncodedImageFormat.Bmp, 100);
                using FileStream stream = File.OpenWrite(Path);
                data.SaveTo(stream);
            }
            else if (ImageFormat == CommonImageFormat.Webp)
            {
                using SKData data = image.Encode(SKEncodedImageFormat.Webp, 100);
                using FileStream stream = File.OpenWrite(Path);
                data.SaveTo(stream);
            }
            else
            {
                using SKData data = image.Encode(SKEncodedImageFormat.Png, 100);
                using FileStream stream = File.OpenWrite(Path);
                data.SaveTo(stream);
            }

            return true;
        }

    }
}