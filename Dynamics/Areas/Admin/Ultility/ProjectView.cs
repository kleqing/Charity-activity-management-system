namespace Dynamics.Areas.Admin.Ultility
{
    public class ProjectView
    {
        public static string GetStatusClass(int status)
        {
            return status switch
            {
                0 => "bg-warning", // Ongoing
                1 => "bg-success", // Finished
                2 => "bg-danger",  // Canceled
                _ => "bg-secondary" // Default
            };
        }

        public static string GetStatusText(int status)
        {
            return status switch
            {
                0 => "Ongoing",
                1 => "Finished",
                2 => "Canceled",
                _ => "Unknown"
            };
        }
    }
}
