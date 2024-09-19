namespace Dynamics.Helps
{
    public class Util
    {
        public static string UploadImage(IFormFile image, string folder)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folder, image.FileName);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    image.CopyTo(myfile);
                }

                return image.FileName;
            }
            catch (Exception e)
            {
                return string.Empty;
            }

        }
    }
}
