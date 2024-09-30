using System.Security.Cryptography;

namespace Dynamics.Helps
{
    public class Util
    {
        public static string UploadImage(IFormFile image, string folder, string userId)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(image.FileName) + userId;
                string fileNameExtension = Path.GetExtension(image.FileName);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folder, fileName + fileNameExtension);
                using (var myfile = new FileStream(fullPath, FileMode.Create))
                {
                    image.CopyTo(myfile);
                }
                
                return fileName+ fileNameExtension;
            }catch (Exception e)
            {
                return string.Empty;
            }

        }
    }
}
