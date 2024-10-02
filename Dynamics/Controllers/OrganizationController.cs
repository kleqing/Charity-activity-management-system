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
        IProjectRepository _projectRepository;

        public OrganizationController(IOrganizationRepository organizationRepository, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
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
        public async Task<IActionResult> ManageOrganizationTranactionHistory()
        {
            //current Organization
            var organizationString = HttpContext.Session.GetString("organization");
            Organization currentOrganization = null;
            if (organizationString != null)
            {
                currentOrganization = JsonConvert.DeserializeObject<Organization>(organizationString);
            }


            //requets donate is accpected
            var UserToOrganizationTransactionHistoryInAOrganizations = await _organizationRepository.GetAllUserToOrganizationTransactionHistoryByAcceptedAsync(currentOrganization.OrganizationID);


            //list resource in a organization
            var organizationResources = new List<OrganizationResource>();
            var userDonates = new List<User>();
            foreach (var item in UserToOrganizationTransactionHistoryInAOrganizations)
            {
                var organizationResource = await _organizationRepository.GetOrganizationResourceByOrganizationIDAndResourceIDAsync(currentOrganization.OrganizationID, item.ResourceID);
                organizationResources.Add(organizationResource);
                var userDonate = await _userRepository.Get(u => u.UserID.Equals(item.UserID));
                userDonates.Add(userDonate);
            }
            //Create session
            HttpContext.Session.Set<List<OrganizationResource>>(MySettingSession.SESSION_ResourceName_For_UserToOrganizationHistory_Key, organizationResources);
            HttpContext.Session.Set<List<User>>(MySettingSession.SESSION_UserName_For_UserToOrganizationHistory_Key, userDonates);


            return View(UserToOrganizationTransactionHistoryInAOrganizations);
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
            //Resource of a organization
            var organizationResources = await _organizationRepository.GetAllOrganizationResourceByOrganizationIDAsync(or => or.OrganizationID == currentOrganization.OrganizationID);

            //History of a Organnization To project in a organization
            var organizationToProjectHistoryInAOrganizations = await _organizationRepository.GetAllOrganizationToProjectHistoryByProcessingAsync(currentOrganization.OrganizationID);


            //list resource in a organization in processing allocate
            var organizationResourcesInHistory = new List<OrganizationResource>();
            //list project reciver
            var ProjectRecivers = new List<Project>();
            foreach (var item in organizationToProjectHistoryInAOrganizations)
            {
                var organizationResource = await _organizationRepository.GetOrganizationResourceByOrganizationIDAndResourceIDAsync(currentOrganization.OrganizationID, item.OrganizationResourceID);
                organizationResourcesInHistory.Add(organizationResource);
                var project = await _projectRepository.GetProjectByProjectIDAsync(p => p.ProjectID == Convert.ToInt32(item.Message));
                ProjectRecivers.Add(project);
            }
            //Create session
            HttpContext.Session.Set<List<OrganizationResource>>(MySettingSession.SESSION_ResourceName_For_OrganizationToProjectHistory_Key, organizationResourcesInHistory);
            HttpContext.Session.Set<List<Project>>(MySettingSession.SESSION_ProjectName_For_OrganizzationToProjectHistory_Key, ProjectRecivers);
            HttpContext.Session.Set<List<OrganizationToProjectHistory>>(MySettingSession.SESSION_OrganizzationToProjectHistory_For_Organization_Key, organizationToProjectHistoryInAOrganizations);


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

        [HttpGet]
        public async Task<IActionResult> SendResoueceOrganizationToProject(int resourceId)
        {
            //current Organization
            var organizationString = HttpContext.Session.GetString("organization");
            Organization currentOrganization = null;
            if (organizationString != null)
            {
                currentOrganization = JsonConvert.DeserializeObject<Organization>(organizationString);
            }

            //resource user wanto donate session
            var currentOrganizationResource = await _organizationRepository.GetOrganizationResourceByOrganizationIDAndResourceIDAsync(currentOrganization.OrganizationID, resourceId);
            HttpContext.Session.Set<OrganizationResource>(MySettingSession.SESSION_Organization_Resource_Current_Key, currentOrganizationResource);

            //find list project is manage by a Organization
            var projects = await _projectRepository.GetAllProjectsByOrganizationIDAsync(p => p.OrganizationID == currentOrganization.OrganizationID);

            //Create session for list project of a organization
            HttpContext.Session.Set<List<Project>>(MySettingSession.SESSION_Projects_In_A_OrganizationID_Key, projects);

            return View(new OrganizationToProjectHistory());
        }

        [HttpPost]
        public async Task<IActionResult> SendResoueceOrganizationToProject(OrganizationToProjectHistory organizationToProjectHistory, int projectId)
        {
            if(organizationToProjectHistory != null && projectId != null)
            {
                organizationToProjectHistory.Message = projectId+"";// tạm thời
                await _organizationRepository.AddOrganizationToProjectHistoryAsync(organizationToProjectHistory);
                return RedirectToAction(nameof(ManageOrganizationResource));
            }

            return View(organizationToProjectHistory);
        }


        public async Task<IActionResult> DonateByMoney()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DonateByResource(int resourceId)
        {
            //current Organization
            var organizationString = HttpContext.Session.GetString("organization");
            Organization currentOrganization = null;
            if (organizationString != null)
            {
                currentOrganization = JsonConvert.DeserializeObject<Organization>(organizationString);
            }

            //resource user wanto donate session
            var currentOrganizationResource = await _organizationRepository.GetOrganizationResourceByOrganizationIDAndResourceIDAsync(currentOrganization.OrganizationID, resourceId);
            HttpContext.Session.Set<OrganizationResource>(MySettingSession.SESSION_Organization_Resource_Current_Key, currentOrganizationResource);


            return View(new UserToOrganizationTransactionHistory());
        }
        [HttpPost]
        public async Task<IActionResult> DonateByResource(UserToOrganizationTransactionHistory transactionHistory)
        {
            if (transactionHistory != null)
            {
                await _organizationRepository.AddUserToOrganizationTransactionHistoryASync(transactionHistory);
                return RedirectToAction(nameof(ManageOrganizationResource));
            }

            return View(transactionHistory);
        }

        public async Task<IActionResult> ReviewDonateRequest()
        {
            //current Organization
            var organizationString = HttpContext.Session.GetString("organization");
            Organization currentOrganization = null;
            if (organizationString != null)
            {
                currentOrganization = JsonConvert.DeserializeObject<Organization>(organizationString);
            }

            var UserToOrganizationTransactionHistoryInAOrganizations = await _organizationRepository.GetAllUserToOrganizationTransactionHistoryByProcessingAsync(currentOrganization.OrganizationID);


            //list resource in a organization
            var organizationResources = new List<OrganizationResource>();
            var userDonates = new List<User>();
            foreach(var item in UserToOrganizationTransactionHistoryInAOrganizations)
            {
                var organizationResource = await _organizationRepository.GetOrganizationResourceByOrganizationIDAndResourceIDAsync(currentOrganization.OrganizationID, item.ResourceID);
                organizationResources.Add(organizationResource);
                var userDonate = await _userRepository.Get(u => u.UserID.Equals(item.UserID));
                userDonates.Add(userDonate);
            }
            //Create session
            HttpContext.Session.Set<List<OrganizationResource>>(MySettingSession.SESSION_ResourceName_For_UserToOrganizationHistory_Key, organizationResources);
            HttpContext.Session.Set<List<User>>(MySettingSession.SESSION_UserName_For_UserToOrganizationHistory_Key, userDonates);

            return View(UserToOrganizationTransactionHistoryInAOrganizations);
        }

        public async Task<IActionResult> DenyRequestDonate(int transactionId)
        {

            await _organizationRepository.DeleteUserToOrganizationTransactionHistoryByTransactionIDAsync(transactionId);

            return RedirectToAction(nameof(ReviewDonateRequest));
        }
        public async Task<IActionResult> AcceptRquestDonate(int transactionId)
        {
            //update table UserToOrganizationTransactionHistory
            var userToOrganizationTransactionHistory = await _organizationRepository.GetUserToOrganizationTransactionHistoryByTransactionIDAsync(uto => uto.TransactionID == transactionId);
            userToOrganizationTransactionHistory.Status = 1;
            await _organizationRepository.UpdateUserToOrganizationTransactionHistoryAsync(userToOrganizationTransactionHistory);


            //current Organization
            var organizationString = HttpContext.Session.GetString("organization");
            Organization currentOrganization = null;
            if (organizationString != null)
            {
                currentOrganization = JsonConvert.DeserializeObject<Organization>(organizationString);
            }


            //update table resource
            var organizationResource = await _organizationRepository.GetOrganizationResourceByOrganizationIDAndResourceIDAsync(currentOrganization.OrganizationID, userToOrganizationTransactionHistory.ResourceID);
            organizationResource.Quantity += userToOrganizationTransactionHistory.Amount;
            await _organizationRepository.UpdateOrganizationResourceAsync(organizationResource);


            return RedirectToAction(nameof(ManageOrganizationResource));
        }

    }
}


//4 method can be use Session current organization that is: 2 Edit 2 transfer