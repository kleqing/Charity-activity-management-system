using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Dynamics.Utility
{
    public static class Util
    {
        //
        /**
         * Save an image to local file <br />
         * Follow this path: wwwroot/folderpath/id.ext
         */
        public static string UploadImage(IFormFile image, string folder, string id)
        {
            try
            {
                // Get the ID of the user to mark it as filename 
                string fileName = Convert.ToString(id);
                string fileNameExtension = Path.GetExtension(image.FileName);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName + fileNameExtension);
                using (var myfile = new FileStream(fullPath, FileMode.Create))
                {
                    image.CopyTo(myfile);
                }
                string imagePath = Path.Combine(folder, fileName + fileNameExtension);
                return "/" + imagePath.Replace('\\', '/');
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        // upload multiple images for request controller
        public static string UploadMultiImage(List<IFormFile> images, string folder, Guid id)
        {
            try
            {
                // List to hold the individual image paths
                List<string> imagesPath = new List<string>();
                // Process each image
                foreach (var image in images)
                {
                    if (image != null && image.Length > 0)
                    {
                        // create folder for each individual request
                        string folderName = id.ToString();
                        string filenameExtension = image.FileName;
                        bool folderExists = Directory.Exists(folder);
                        if(!folderExists) { Directory.CreateDirectory(@"wwwroot\" + folder); }
                        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder,
                            filenameExtension);
                        using (var myFile = new FileStream(fullPath, FileMode.Create))
                        {
                            image.CopyTo(myFile);
                        }

                        string imagePath = Path.Combine(folder,filenameExtension);
                        imagesPath.Add("/" + imagePath.Replace('\\', '/'));
                    }
                }

                // Return all image paths joined by ","
                return string.Join(",", imagesPath);
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}
