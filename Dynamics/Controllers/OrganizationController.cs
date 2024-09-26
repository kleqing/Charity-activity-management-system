using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Dynamics.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dynamics.Controllers
{
    public class OrganizationController : Controller
    {

        IOrganizationRepository _organizationRepository;
        IUserRepository _userRepository;

        public OrganizationController(IOrganizationRepository organizationRepository, IUserRepository userRepository)
        {
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
        }


        //GET: /Organization
        
        public async Task<IActionResult> Index()
        {
            var organizationMembers = await _organizationRepository.GetAllOrganizationMemberAsync();
            HttpContext.Session.Set<List<OrganizationMember>>(MySettingSession.SESSION_Organization_Member_KEY, organizationMembers);


            var organizations = await _organizationRepository.GetAllOrganizationsAsync();
            return View(organizations);
        }

        //GET: /Organization/Create
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View(new Organization());
        }

        //POST: /Organizations/Create
        [HttpPost]
        public async Task<IActionResult> Create(Organization organization, IFormFile image)
        {
            if (image != null)
            {
                organization.OrganizationPictures = Util.UploadImage(image, @"images\Organization", organization.OrganizationID + "");
            }

            await _organizationRepository.AddAsync(organization);
           
            return RedirectToAction(nameof(JoinOrganization), new { organizationId = organization.OrganizationID });
        }


        

        public async Task<IActionResult> MyOrganization(string userId)
        {
            var organizationMembers1 = await _organizationRepository.GetAllOrganizationMemberAsync();
            //create session for _DetailOrganization
            HttpContext.Session.Set<List<OrganizationMember>>(MySettingSession.SESSION_Organization_Member_KEY, organizationMembers1);

            //find table organization member
            var organizationMembers = await _organizationRepository.GetAllOrganizationMemberByIDAsync(om => om.UserID == userId);
            List<Organization> organizations = new List<Organization>();
            foreach (var item in organizationMembers)
            {
                var organization = await _organizationRepository.GetAsync(o => o.OrganizationID == item.OrganizationID);
                organizations.Add(organization);
            }

            return View(organizations);
        }

        public async Task<IActionResult> Detail(int organizationId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.OrganizationID == organizationId);
            if (organization == null)
            {
                return NotFound();  
            }

            //Create session
            HttpContext.Session.SetString("organization", JsonConvert.SerializeObject(organization));


            var userCeo = await _userRepository.Get(u => u.UserID.Equals(organization.CEOID));
            @ViewBag.Ceo = userCeo;
            return View(organization);
        }

        public async Task<IActionResult> Edit(int organizationId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.OrganizationID == organizationId);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Organization organization, IFormFile image)
        {
            if (organization != null)
            {
                if (image != null)
                {
                    organization.OrganizationPictures = Util.UploadImage(image, @"images\Organization", organization.OrganizationID + "");
                    
                }

                //var oldOrganization = await _organizationRepository.GetAsync(o => o.OrganizationID == organization.OrganizationID);
                //organization.OrganizationPictures = oldOrganization.OrganizationPictures;
                await _organizationRepository.UpdateAsync(organization);
                return RedirectToAction("Detail", new { organizationId = organization.OrganizationID });

            }
            return View(organization);
        }

        public async Task<IActionResult> ManageOrganizationMember(int? organizationId)
        {
            //find table organization member
            var organizationMembers = await _organizationRepository.GetAllOrganizationMemberByIDAsync(om => om.OrganizationID == organizationId);
            List<User> users = new List<User>();
            foreach(var item in organizationMembers)
            {
                var user = await _userRepository.Get(u => u.UserID.Equals(item.UserID));
                users.Add(user);
            }

            //find organization
            var organization = await _organizationRepository.GetAsync(o => o.OrganizationID == organizationId);
            if (organization == null)
            {
                return NotFound();
            }

            //get CEO
            var userCeo = await _userRepository.Get(u => u.UserID.Equals(organization.CEOID));
            @ViewBag.Ceo = userCeo;
            return View(users);
        }



        public async Task<IActionResult> JoinOrganization(int organizationId)
        {
            //get current user
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }


            //join organization
            var organizationMember = new OrganizationMember() {
                UserID = currentUser.UserID, 
                OrganizationID = organizationId,
            };
            await _organizationRepository.AddOrganizationMemberSync(organizationMember);



            //find table organization member
            var organizationMembers = await _organizationRepository.GetAllOrganizationMemberAsync();
            //Create session
            HttpContext.Session.Set<List<OrganizationMember>>(MySettingSession.SESSION_Organization_Member_KEY, organizationMembers);
            return RedirectToAction(nameof(Detail), new { organizationId = organizationId});
        }

        [HttpPost]
        public async Task<IActionResult> OutOrganization(int organizationId, string userId)
        {
            //out organization
            await _organizationRepository.DeleteOrganizationMemberByOrganizationIDAndUserIDAsync(organizationId, userId);


            //find table organization member
            var organizationMembers = await _organizationRepository.GetAllOrganizationMemberAsync();
            //Create session
            HttpContext.Session.Set<List<OrganizationMember>>(MySettingSession.SESSION_Organization_Member_KEY, organizationMembers);


            //get current user
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }

            //currentOrganization
            var currentOrganization = await _organizationRepository.GetAsync(o => o.OrganizationID.Equals(organizationId));

            if (currentUser.UserID.Equals(currentOrganization.CEOID))
            {
                return RedirectToAction(nameof(ManageOrganizationMember), new { organizationId = organizationId });
            }
            return RedirectToAction(nameof(Detail), new { organizationId = organizationId });
        }



        public async Task<IActionResult> TransferCeoOrganization(int organizationId)
        {
            // send to form to save vallue not change 
            var organization = await _organizationRepository.GetAsync(o => o.OrganizationID == organizationId);
            if (organization == null)
            {
                return NotFound();
            }


            //find table organization member
            var organizationMembers = await _organizationRepository.GetAllOrganizationMemberByIDAsync(om => om.OrganizationID == organizationId);
            List<User> users = new List<User>();
            foreach (var item in organizationMembers)
            {
                var user = await _userRepository.Get(u => u.UserID.Equals(item.UserID));
                users.Add(user);
            }

            @ViewBag.OrganizationMembers = users;
            return View(organization);
        }

        [HttpPost]
        public async Task<IActionResult> TransferCeoOrganization(Organization organization)
        {
            if (organization != null)
            {
                await _organizationRepository.UpdateAsync(organization);
                return RedirectToAction("Index", "EditUser");
            }
            return View(organization);
        }
    }
}
