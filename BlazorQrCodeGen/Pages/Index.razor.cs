using BlazorDownloadFile;
using BlazorMAP.DTO.Enums;
using LibQrGenerator.Enums;
using LibQrGenerator.Payloads;
using LibQrGenerator.QrDrawerFactory;
using Microsoft.AspNetCore.Components;
using SixLabors.ImageSharp.Formats;

namespace BlazorQrCodeGen.Pages
{
    public partial class Index : ComponentBase
    {

        [Inject]
        public IBlazorDownloadFileService BlazorDownloadFileService { get; set; }

        private byte[] Img { get; set; }
        private string Format { get; set; } = ImageFormats.Png;

        private string CustomQrText { get; set; } = "Custom Qr Text";

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        protected override async Task OnInitializedAsync()
        {
            var msg = new PayloadGenerator.ContactData(
                PayloadGenerator.ContactData.ContactOutputType.VCard4,
                GenerateRandomString(5),
                GenerateRandomString(10),
                "EvilMax"
                ).ToString();

            var qr = new QrDrawerFactory().GetDriver(DrawDriverType.SkiaSharp);
            Img = qr
                .SetQr(msg)
                .SetImageFormat(CommonImageFormat.Png)
                .GetBytes();

            //var ttt = qr
            //    .SetQr("Hello Goodbye")
            //    .SetImageFormat(LibQrGenerator.QrDrawerFactory.CommonImageFormat.Png)
            //    .Save("./Test.png");

            //StateHasChanged();
        }

        public async void DownloadQr()
        {

            var downloadRes = await BlazorDownloadFileService.DownloadFile($"QR_{DateTime.Now.ToString("yyyyMMddHHmmss")}.{Format}", Img, "application/octet-stream");

        }

        public async void GenerateCustomQr()
        {

            var qr = new QrDrawerFactory().GetDriver(DrawDriverType.ImageSharp);
            Img = qr
                .SetQr(CustomQrText)
                .SetImageFormat(CommonImageFormat.Png)
                .GetBytes();

        }

    }
}
