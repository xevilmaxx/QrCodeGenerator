//
// QR code generator library (.NET)
// https://github.com/manuelbl/QrCodeGenerator
//
// Copyright (c) 2021 Manuel Bleichenbacher
// Licensed under MIT License
// https://opensource.org/licenses/MIT
//

using Net.Codecrete.QrCodeGenerator;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using LibQrGenerator.Enums;

namespace LibQrGenerator.QrDrawerFactory
{
    public class ImageSharp : GenericDrawer
    {

        private Color ConvForeColor = Color.Black;
        private Color ConvBackColor = Color.White;

        /// <inheritdoc cref="ToBitmap(QrCode, int, int)"/>
        /// <param name="background">The background color.</param>
        /// <param name="foreground">The foreground color.</param>
        private Image BuildImage()
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
            Image<Rgb24> image = new Image<Rgb24>(dim, dim);

            image.Mutate(img =>
            {
                // draw background
                img.Fill(ConvBackColor);

                // draw modules
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        if (QrCode.GetModule(x, y))
                        {
                            img.Fill(
                                ConvForeColor, 
                                new Rectangle((x + Border) * Scale, (y + Border) * Scale,
                                Scale,
                                Scale
                                ));
                        }
                    }
                }
            });

            return image;
        }

        private void ConvertColor()
        {

            Color.TryParseHex(ForegroundColor, out ConvForeColor);
            Color.TryParseHex(BackgroundColor, out ConvBackColor);

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

            using Image image = BuildImage();
            using MemoryStream ms = new MemoryStream();

            if (ImageFormat == CommonImageFormat.Jpeg)
            {
                image.SaveAsJpeg(ms);
            }
            else if (ImageFormat == CommonImageFormat.Bmp)
            {
                image.SaveAsBmp(ms);
            }
            else if (ImageFormat == CommonImageFormat.Webp)
            {
                image.SaveAsWebp(ms);
            }
            else
            {
                image.SaveAsPng(ms);
            }            

            return ms.ToArray();
        }

        public override bool Save(string Path)
        {
            using Image image = BuildImage();
            using MemoryStream ms = new MemoryStream();

            if (ImageFormat == CommonImageFormat.Jpeg)
            {
                image.SaveAsJpeg(Path);
            }
            else if (ImageFormat == CommonImageFormat.Bmp)
            {
                image.SaveAsBmp(Path);
            }
            else if (ImageFormat == CommonImageFormat.Webp)
            {
                image.SaveAsWebp(Path);
            }
            else
            {
                image.SaveAsPng(Path);
            }

            return true;
        }
    }
}
