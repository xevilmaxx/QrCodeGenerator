using LibQrGenerator.Enums;
using Net.Codecrete.QrCodeGenerator;
using static System.Net.Mime.MediaTypeNames;

namespace LibQrGenerator.QrDrawerFactory
{
    public abstract class GenericDrawer
    {

        internal QrCode QrCode { get; set; } = null;
        internal int Scale { get; set; } = 10;
        internal int Border { get; set; } = 4;
        internal string ForegroundColor { get; set; } = "#000000";
        internal string BackgroundColor { get; set; } = "#ffffff";
        internal CommonImageFormat ImageFormat { get; set; } = CommonImageFormat.Png;

        public abstract GenericDrawer SetQr(QrCode QrCode);

        public abstract GenericDrawer SetScale(int Scale);
        public abstract GenericDrawer SetBorder(int Border);
        public abstract GenericDrawer SetForegroundColor(string Color);
        public abstract GenericDrawer SetBackgroundColor(string Color);
        public abstract GenericDrawer SetImageFormat(CommonImageFormat ImageFormat);

        public abstract byte[] GetBytes();
        public abstract bool Save(string Path);

        public GenericDrawer SetQr(string QrText)
        {

            var qr = QrCode.EncodeText(QrText, QrCode.Ecc.Medium);
            SetQr(qr);

            return this;

        }

    }
}
