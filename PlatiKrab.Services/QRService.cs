

using Microsoft.Maui.Storage;

namespace PlatiKrab.Services
{
    public class QRService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string accountNumber;
        private readonly string bankCode;

        public QRService(string accountNumber, string bankCode)
        {
            this.accountNumber = accountNumber;
            this.bankCode = bankCode;
        }

        public async Task<string> GetQRCodeAsync(DateTime dateTime)
        {
            string formattedDate = dateTime.ToString("dd-MM-yyyy");
            string url = $"https://api.paylibo.com/paylibo/generator/czech/image?accountNumber={accountNumber}&bankCode={bankCode}" +
                         $"&amount=250.00&currency=CZK&vs=100&message={formattedDate}&size=1000&branding=false";

            HttpResponseMessage response = await client.GetAsync(url);
            //smazeni predchozich QR kodu, nekde se to nedela automaticky
            CleanUpOldQRCodes();

            byte[] imageData = await response.Content.ReadAsByteArrayAsync();
            string filePath = await SaveImageAsync(imageData, formattedDate);
            return filePath;
        }

        private async Task<string> SaveImageAsync(byte[] imageData, string formattedDate)
        {
            string fileName = $"QRCode_{formattedDate}.png";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

            await File.WriteAllBytesAsync(filePath, imageData);
            return filePath;
        }

        private void CleanUpOldQRCodes()
        {
            string cacheDir = FileSystem.CacheDirectory;
            foreach (var file in Directory.GetFiles(cacheDir, "QRCode_*.png"))
            {
                File.Delete(file);
            }
        }

    }
}
