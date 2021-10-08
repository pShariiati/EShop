using Microsoft.AspNetCore.Http;
using System.IO;

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
    }
}
