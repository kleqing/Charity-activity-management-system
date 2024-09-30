using Dynamics.Models.Models;

namespace Dynamics.Areas.Admin.Models
{
    public class Dashboard
    {
        public List<User> TopUser { get; set; }
        public List<Organization> TopOrganization { get; set; }
        public List<Request> GetRecentRequest { get; set; }

        // Count
        public int UserCount { get; set; }
        public int OrganizationCount { get; set; }
        public int RequestCount { get; set; }
        public int ProjectCount { get; set; }
    }
}
