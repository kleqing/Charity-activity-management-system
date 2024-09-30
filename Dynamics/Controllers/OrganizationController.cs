using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Dynamics.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        //s1-done
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

            var organizationResource = new OrganizationResource()
            {
                OrganizationID = organization.OrganizationID,
                ResourceName = "Money(Tiền)",
                Quantity = 0,
                Unit = "VND",
            };
            if (organizationResource != null)
            {
                await _organizationRepository.AddOrganizationResourceSync(organizationResource); 
            }

            return RedirectToAction(nameof(JoinOrganization), new { organizationId = organization.OrganizationID });
        }


        
        //s1-done
        public async Task<IActionResult> MyOrganization(string userId)
        {

            //find table organization member
            var organizationMembers = await _organizationRepository.GetAllOrganizationMemberByIDAsync(om => om.UserID == userId);
            List<Organization> organizations = new List<Organization>();
            foreach (var item in organizationMembers)
            {
                var organization = await _organizationRepository.GetAsync(o => o.OrganizationID == item.OrganizationID);
                organizations.Add(organization);
            }

            return View(organizations);
        }//fix session done

        public async Task<IActionResult> Detail(int organizationId)
        {
            //find table organization member. member in a organization
            var organizationMembers = await _organizationRepository.GetAllOrganizationMemberByIDAsync(om => om.OrganizationID == organizationId);
            var users = new List<User>();
            foreach (var item in organizationMembers)
            {
                User user = await _userRepository.Get(u => u.UserID.Equals(item.UserID));
                users.Add(user);
            }
            //create session for save user member in specify organizationId - for _DetailOrganization.cshtml
            HttpContext.Session.Set<List<User>>(MySettingSession.SESSION_User_In_A_OrganizationID_KEY, users);


            //Creat current organization
            var organization = await _organizationRepository.GetAsync(o => o.OrganizationID == organizationId);
            if (organization == null)
            {
                return NotFound();  
            }

            //Create session current Organization
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            
            HttpContext.Session.SetString("organization", JsonConvert.SerializeObject(organization, settings));

            return View(organization);
        }

        public async Task<IActionResult> Edit(int organizationId)
        {
            // can be use Session current organization
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
            // can be use Session current organization
            if (organization != null)
            {
                if (image != null)
                {
                    organization.OrganizationPictures = Util.UploadImage(image, @"images\Organization", organization.OrganizationID + "");
                    
                }
                await _organizationRepository.UpdateAsync(organization);
                return RedirectToAction("Detail", new { organizationId = organization.OrganizationID });

            }
            return View(organization);
        }





        /// <summary>
        /// Manager Member
        /// </summary>
        /// <returns></returns>
        //need fix
        public async Task<IActionResult> ManageOrganizationMember()
        {
            return View();
        }


        //s1-done
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


            //re-set session for detail organization
            //find table organization member. member in a organization
            var organizationMembers = await _organizationRepository.GetAllOrganizationMemberByIDAsync(om => om.OrganizationID == organizationId);
            var users = new List<User>();
            foreach (var item in organizationMembers)
            {
                User user = await _userRepository.Get(u => u.UserID.Equals(item.UserID));
                users.Add(user);
            }
            //create session for save user member in specify organizationId - for _DetailOrganization.cshtml
            HttpContext.Session.Set<List<User>>(MySettingSession.SESSION_User_In_A_OrganizationID_KEY, users);


            return RedirectToAction(nameof(Detail), new { organizationId = organizationId});
        }

        [HttpPost]
        //s1-done
        public async Task<IActionResult> OutOrganization(int organizationId, string userId)
        {
            //out organization
            await _organizationRepository.DeleteOrganizationMemberByOrganizationIDAndUserIDAsync(organizationId, userId);

            //find table organization member. member in a organization
            var organizationMembers = await _organizationRepository.GetAllOrganizationMemberByIDAsync(om => om.OrganizationID == organizationId);
            var users = new List<User>();
            foreach (var item in organizationMembers)
            {
                User user = await _userRepository.Get(u => u.UserID.Equals(item.UserID));
                users.Add(user);
            }
            //create session for save user member in specify organizationId - for _DetailOrganization.cshtml
            HttpContext.Session.Set<List<User>>(MySettingSession.SESSION_User_In_A_OrganizationID_KEY, users);


            //get current user session
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
                //remove member by ceo
                return RedirectToAction(nameof(ManageOrganizationMember), new { organizationId = organizationId });
            }
            return RedirectToAction(nameof(Detail), new { organizationId = organizationId });
        }


        //s1
        public async Task<IActionResult> TransferCeoOrganization(int organizationId)
        {
            // send to form to save vallue not change
            // can be use Session current organization
            var organization = await _organizationRepository.GetAsync(o => o.OrganizationID == organizationId);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        [HttpPost]
        public async Task<IActionResult> TransferCeoOrganization(Organization organization)
        {
            // can be use Session current organization
            if (organization != null)
            {
                await _organizationRepository.UpdateAsync(organization);
                return RedirectToAction("Index", "EditUser");
            }
            return View(organization);
        }




        ////Manage Project
        public async Task<IActionResult> ManageOrganizationProject()
        {
            var projects = HttpContext.Session.Get<List<Project>>(MySettingSession.SESSION_Projects_In_A_OrganizationID_Key);
            return View(projects);
        }


        ///manage history
        public async Task<IActionResult> ManageOrganizationTranactionHistory(int organizationId)
        {
            return View();
        }

        //manage Resource
        public async Task<IActionResult> ManageOrganizationResource()
        {
            //current Organization
            var organizationString = HttpContext.Session.GetString("organization");
            Organization currentOrganization = null;
            if (organizationString != null)
            {
                currentOrganization = JsonConvert.DeserializeObject<Organization>(organizationString);
            }

            var organizationResources = await _organizationRepository.GetAllOrganizationResourceByOrganizationIDAsync(or => or.OrganizationID == currentOrganization.OrganizationID);
            return View(organizationResources);
        }

        [HttpGet]
        public async Task<IActionResult> AddNewOrganizationResource()
        {
            
            return View(new OrganizationResource());
        }

        [HttpPost]
        public async Task<IActionResult> AddNewOrganizationResource(OrganizationResource organizationResource)
        {
            if (organizationResource != null)
            {
                await _organizationRepository.AddOrganizationResourceSync(organizationResource);
                return RedirectToAction(nameof(ManageOrganizationResource));
            }
            return View();
            
        }
        public async Task<IActionResult> SendResoueceOrganizationToProject()
        {
            return View();
        }

        public async Task<IActionResult> DonateByMoney()
        {
            return View();
        }
        public async Task<IActionResult> DonateByResource()
        {
            return View();
        }

        public async Task<IActionResult> ReviewDonateRequest()
        {
            return View();
        }

    }
}


//4 method can be use Session current organization that is: 2 Edit 2 transfer