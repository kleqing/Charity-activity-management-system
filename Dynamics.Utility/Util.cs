using Microsoft.AspNetCore.Http;

namespace Dynamics.Utility
{
    public static class Util
    {
        // TODO: UploadMultiple ?
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
            }catch (Exception e)
            {
                return string.Empty;
            }

        }
    }
}
