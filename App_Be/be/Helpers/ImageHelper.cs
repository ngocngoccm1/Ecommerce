using System;
using System.IO;
using Microsoft.AspNetCore.Http;
namespace App.Helpers
{
    public static class ImageHelper
    {
        public static string EncodeImageToBase64(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "";
            }

            using (var memoryStream = new MemoryStream())
            {
                // Sao chép nội dung của file vào memory stream
                file.CopyTo(memoryStream);

                var imageBytes = memoryStream.ToArray();
                // Mã hóa thành chuỗi Base64

                return Convert.ToBase64String(imageBytes);
            }
        }
        public static async Task<string> ConvertImageToBase64Async(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return null;

            byte[] imageBytes;
            using (var webClient = new HttpClient())
            {
                imageBytes = await webClient.GetByteArrayAsync(imageUrl);
            }
            return Convert.ToBase64String(imageBytes);
        }
    }
}
