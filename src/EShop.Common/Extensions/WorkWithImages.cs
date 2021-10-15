using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace EShop.Common.Extensions
{
    public static class WorkWithImages
    {
        public static void SaveImage(this IFormFile image, string name, string imageExtension, string folderName)
        {
            var imagePath =
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName, name + imageExtension);

            using var stream = new FileStream(imagePath, FileMode.Create);
            image.CopyTo(stream);
        }

        public static void RemoveImage(string imageName, string folderName)
        {
            var imagePath =
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName, imageName);
            File.Delete(imagePath);
        }

        public static Image FromBase64ToImage(this string input)
        {
            var avatarBytes = Convert.FromBase64String(input);
            using var ms = new MemoryStream(avatarBytes);
            using var image = Image.FromStream(ms);
            return image;
        }

        public static async Task<string> SaveBase64ImageAsync(this string image, string name, string folderName)
        {
            var bytes = Convert.FromBase64String(image);
            await using var ms = new MemoryStream(bytes);
            using var pic = Image.FromStream(ms);
            var imageExtension = "." + pic.RawFormat;
            var imagePath =
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName, name + imageExtension);
            pic.Save(imagePath);
            return imageExtension.ToString();
        }

        public static async Task<string> ConvertToBase64(this IFormFile input)
        {
            await using var ms = new MemoryStream();
            await input.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            return Convert.ToBase64String(fileBytes);
        }
    }
}
