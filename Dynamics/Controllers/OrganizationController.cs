﻿using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Dynamics.Services;
using Dynamics.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Resources;

namespace Dynamics.Controllers
{
    public class OrganizationController : Controller
    {

        IOrganizationRepository _organizationRepository;
        IUserRepository _userRepository;
        IProjectRepository _projectRepository;
        IOrganizationVMService _organizationService;
        IUserToOragnizationTransactionHistoryVMService _userToOragnizationTransactionHistoryVMService;
        IProjectVMService _projectVMService;
        IOrganizationToProjectHistoryVMService _organizationToProjectHistoryVMService;
        private readonly CloudinaryUploader _cloudinaryUploader;
        private readonly IOrganizationService _orgDisplayService;
        private readonly IOrganizationMemberRepository _organizationMemberRepository;

        public OrganizationController(IOrganizationRepository organizationRepository,
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            IOrganizationVMService organizationService,
            IUserToOragnizationTransactionHistoryVMService userToOragnizationTransactionHistoryVMService,
            IProjectVMService projectVMService,
            IOrganizationToProjectHistoryVMService organizationToProjectHistoryVMService,
            CloudinaryUploader cloudinaryUploader, IOrganizationService orgDisplayService, IOrganizationMemberRepository organizationMemberRepository)
        {

            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _organizationService = organizationService;
            _userToOragnizationTransactionHistoryVMService = userToOragnizationTransactionHistoryVMService;
            _projectVMService = projectVMService;
            _organizationToProjectHistoryVMService = organizationToProjectHistoryVMService;
            _cloudinaryUploader = cloudinaryUploader;
            _orgDisplayService = orgDisplayService;
            _organizationMemberRepository = organizationMemberRepository;
        }

        //The index use the cards at homepage to display instead - Kiet
        public async Task<IActionResult> Index()
        {
            var orgs = _organizationRepository.GetAll();
            var organizationVMs = _orgDisplayService.MapToOrganizationOverviewDtoList(orgs.ToList());
            return View(organizationVMs);
        }

        //GET: /Organization/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //get current user
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }


            var organization = new Organization()
            {
                OrganizationID = Guid.NewGuid(),
                StartTime = DateOnly.FromDateTime(DateTime.UtcNow),
                OrganizationEmail = currentUser.UserEmail,
                OrganizationPhoneNumber = currentUser.UserPhoneNumber,
                OrganizationAddress = currentUser.UserAddress,
            };


            return View(organization);
        }

        //POST: /Organizations/Create
        [HttpPost]
        public async Task<IActionResult> Create(Organization organization, IFormFile image)
        {
            //set picture for Organization
            if (image != null)
            {
                // organization.OrganizationPictures = Util.UploadImage(image, @"images\Organization");
                organization.OrganizationPictures = await _cloudinaryUploader.UploadImageAsync(image);
            }

            //get current user
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }

            //set contact for Organization
            if (organization.OrganizationEmail == null)
            {
                organization.OrganizationEmail = currentUser.UserEmail;
            }

            if (organization.OrganizationPhoneNumber == null)
            {
                organization.OrganizationPhoneNumber = currentUser.UserPhoneNumber;
            }

            if (organization.OrganizationAddress == null)
            {
                organization.OrganizationAddress = currentUser.UserAddress;
            }

            if (await _organizationRepository.AddOrganizationAsync(organization))
            {
                var organizationResource = new OrganizationResource()
                {
                    OrganizationID = organization.OrganizationID,
                    ResourceName = "Money",
                    Quantity = 0,
                    Unit = "VND",
                };
                await _organizationRepository.AddOrganizationResourceSync(organizationResource);

                return RedirectToAction(nameof(JoinOrganization), new { organizationId = organization.OrganizationID, status = 2, userId = currentUser.UserID });//status 2 : CEOID   0 : processing   1 : membert
            }
            else
                return View(organization);
        }

        public async Task<IActionResult> MyOrganization(Guid userId)
        {
            // All organizations
            var orgs = _organizationRepository.GetAll();
            var organizationVMs = _orgDisplayService.MapToOrganizationOverviewDtoList(orgs.ToList());
            // My organizations only
            var myOrganizationMembers = await _organizationMemberRepository.GetAllAsync(om => om.UserID == userId);
            var myOrgs = new List<Organization>();
            foreach (var organizationMember in myOrganizationMembers)
            {
                myOrgs.Add(organizationMember.Organization);
            }
            var MyOrgDtos = _orgDisplayService.MapToOrganizationOverviewDtoList(myOrgs);
            ViewBag.MyOrgs = MyOrgDtos;
            return View(organizationVMs);

        }//fix session done

        public async Task<IActionResult> Detail(Guid organizationId)
        {
            //Creat current organization
            var organizationVM = await _organizationService.GetOrganizationVMAsync(o => o.OrganizationID.Equals(organizationId));
            HttpContext.Session.Set<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY, organizationVM);

            return View(organizationVM);
        }

        public async Task<IActionResult> Edit(Guid organizationId)
        {
            // can be use Session current organization
            var organization = await _organizationRepository.GetOrganizationAsync(o => o.OrganizationID.Equals(organizationId));
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
                    // organization.OrganizationPictures = Util.UploadImage(image, @"images\Organization");
                    organization.OrganizationPictures = await _cloudinaryUploader.UploadImageAsync(image);
                }
                if (await _organizationRepository.UpdateOrganizationAsync(organization))
                    return RedirectToAction("Detail", new { organizationId = organization.OrganizationID });

            }
            return View(organization);
        }


        //Organization Member
        public async Task<IActionResult> ManageOrganizationMember()
        {
            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);
            //Creat current organization
            var organizationVM = await _organizationService.GetOrganizationVMAsync(o => o.OrganizationID.Equals(currentOrganization.OrganizationID));
            HttpContext.Session.Set<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY, organizationVM);

            return View(organizationVM);
        }

        public async Task<IActionResult> sendRequestJoinOrganization(Guid organizationId, Guid userId)
        {
            // Get the id from session here, no need to pass it from the view - Kiet
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }
            return RedirectToAction(nameof(JoinOrganization), new { organizationId = organizationId, status = 0, userId = currentUser.UserID });
        }

        public async Task<IActionResult> ManageRequestJoinOrganization(Guid organizationId)
        {
            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);
            return View(currentOrganization);
        }


        public async Task<IActionResult> AcceptRquestJoin(Guid organizationId, Guid userId)
        {
            return RedirectToAction(nameof(JoinOrganization), new { organizationId = organizationId, status = 1, userId = userId });
        }


        public async Task<IActionResult> JoinOrganization(Guid organizationId, int status, Guid userId)
        {

            var organizationMember = new OrganizationMember()
            {
                UserID = userId,
                OrganizationID = organizationId,
                Status = status,
            };

            if (status == 2 || status == 0)
            {
                await _organizationRepository.AddOrganizationMemberSync(organizationMember);
            }
            else
            {
                await _organizationRepository.UpdateOrganizationMemberAsync(organizationMember);
            }

            var organizationVM = await _organizationService.GetOrganizationVMAsync(o => o.OrganizationID.Equals(organizationId));
            HttpContext.Session.Set<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY, organizationVM);

            if (status == 1)
            {
                return RedirectToAction(nameof(ManageRequestJoinOrganization), new { organizationId = organizationId });
            }

            return RedirectToAction(nameof(Detail), new { organizationId = organizationId });
        }

        [HttpPost]
        public async Task<IActionResult> OutOrganization(Guid organizationId, Guid userId)
        {
            //user out or ban or deny
            var organizationMember = await _organizationRepository.GetOrganizationMemberAsync(om => om.OrganizationID == organizationId && om.UserID == userId);
            var statusUserOut = organizationMember.Status;
            //out organization
            await _organizationRepository.DeleteOrganizationMemberByOrganizationIDAndUserIDAsync(organizationId, userId);

            var organizationVM = await _organizationService.GetOrganizationVMAsync(o => o.OrganizationID.Equals(organizationId));
            HttpContext.Session.Set<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY, organizationVM);

            //get current user
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }

            //deny or deny request
            if (statusUserOut == 0 && currentUser.UserID.Equals(organizationVM.CEO.UserID))
            {
                return RedirectToAction(nameof(ManageRequestJoinOrganization), new { organizationId = organizationId });
            }
            else if (statusUserOut == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //return RedirectToAction(nameof(Detail), new { organizationId = organizationId });
                return RedirectToAction(nameof(ManageOrganizationMember), new { organizationId = organizationId });
            }

        }

        public async Task<IActionResult> TransferCeoOrganization()
        {
            // send to form to save vallue not change
            // can be use Session current organization
            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);
            return View(currentOrganization);
        }

        [HttpPost]
        public async Task<IActionResult> TransferCeoOrganization(Guid organizationId, Guid currentCEOID, Guid newCEOID)
        {
            // can be use Session current organization
            if (!newCEOID.Equals(currentCEOID))
            {
                var organizationMemberCurrent = new OrganizationMember()
                {
                    UserID = currentCEOID,
                    OrganizationID = organizationId,
                    Status = 1,
                };
                await _organizationRepository.UpdateOrganizationMemberAsync(organizationMemberCurrent);

                var organizationMemberNew = new OrganizationMember()
                {
                    UserID = newCEOID,
                    OrganizationID = organizationId,
                    Status = 2,
                };
                await _organizationRepository.UpdateOrganizationMemberAsync(organizationMemberNew);
                return RedirectToAction("Index", "EditUser");
            }
            return RedirectToAction(nameof(ManageOrganizationMember), new { organizationId = organizationId });
        }



        ////Manage Project
        public async Task<IActionResult> ManageOrganizationProject()
        {

            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);
            //Creat current organization
            var organizationVM = await _organizationService.GetOrganizationVMAsync(o => o.OrganizationID.Equals(currentOrganization.OrganizationID));
            HttpContext.Session.Set<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY, organizationVM);

            return View(organizationVM);
        }


        //Manage history
        public async Task<IActionResult> ManageOrganizationTranactionHistory()
        {

            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);
            //requets donate is accpected
            var UserToOrganizationTransactionHistoryInAOrganizations = await _userToOragnizationTransactionHistoryVMService.GetTransactionHistoryIsAccept(currentOrganization.OrganizationID);

            var OrganizationToProjectHistorysPending = await _organizationToProjectHistoryVMService.GetAllOrganizationToProjectHistoryByPendingAsync(currentOrganization.OrganizationID);
            var OrganizationToProjectHistorysAccepting = await _organizationToProjectHistoryVMService.GetAllOrganizationToProjectHistoryByAcceptingAsync(currentOrganization.OrganizationID);

            HttpContext.Session.Set<List<OrganizationToProjectHistory>>(MySettingSession.SESSION_OrganizzationToProjectHistory_For_Organization_Pending_Key, OrganizationToProjectHistorysPending);
            HttpContext.Session.Set<List<OrganizationToProjectHistory>>(MySettingSession.SESSION_OrganizzationToProjectHistory_For_Organization_Accepting_Key, OrganizationToProjectHistorysAccepting);


            return View(UserToOrganizationTransactionHistoryInAOrganizations);
        }

        //Manage Resource
        public async Task<IActionResult> ManageOrganizationResource()
        {
            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);
            var organizationVM = await _organizationService.GetOrganizationVMAsync(o => o.OrganizationID.Equals(currentOrganization.OrganizationID));
            HttpContext.Session.Set<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY, organizationVM);
            
            var OrganizationToProjectHistorysPending = await _organizationToProjectHistoryVMService.GetAllOrganizationToProjectHistoryByPendingAsync(currentOrganization.OrganizationID);
            HttpContext.Session.Set<List<OrganizationToProjectHistory>>(MySettingSession.SESSION_OrganizzationToProjectHistory_For_Organization_Pending_Key, OrganizationToProjectHistorysPending);
            return View(organizationVM);
        }

        [HttpGet]
        public async Task<IActionResult> AddNewOrganizationResource()
        {
            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);

            var organizationResource = new OrganizationResource()
            {
                ResourceID = new Guid(),
                OrganizationID = currentOrganization.OrganizationID,
                Quantity = 0,
            };

            return View(organizationResource);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewOrganizationResource(OrganizationResource organizationResource)
        {
            if (organizationResource != null)
            {
                if (await _organizationRepository.AddOrganizationResourceSync(organizationResource))
                {
                    var organizationVM = await _organizationService.GetOrganizationVMAsync(o => o.OrganizationID.Equals(organizationResource.OrganizationID));
                    HttpContext.Session.Set<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY, organizationVM);
                    return RedirectToAction(nameof(ManageOrganizationResource));
                }

            }
            return View(organizationResource);

        }

        public async Task<IActionResult> RemoveOrganizationResource(Guid resourceId)
        {
            await _organizationRepository.DeleteOrganizationResourceAsync(resourceId);

            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);
            var organizationVM = await _organizationService.GetOrganizationVMAsync(o => o.OrganizationID.Equals(currentOrganization.OrganizationID));
            HttpContext.Session.Set<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY, organizationVM);
            return RedirectToAction(nameof(ManageOrganizationResource));
        }

        [HttpGet]
        public async Task<IActionResult> SendResoueceOrganizationToProject(Guid resourceId)
        {
            // get current resource
            var currentResource = await _organizationRepository.GetOrganizationResourceAsync(or => or.ResourceID.Equals(resourceId));
            HttpContext.Session.Set<OrganizationResource>(MySettingSession.SESSION_Current_Organization_Resource_KEY, currentResource);

            var organizationToProjectHistory = new OrganizationToProjectHistory()
            {
                OrganizationResourceID = currentResource.ResourceID,
                Status = 0,
                Time = DateOnly.FromDateTime(DateTime.UtcNow),
            };

            return View(organizationToProjectHistory);
        }

        [HttpPost]
        public async Task<IActionResult> SendResoueceOrganizationToProject(OrganizationToProjectHistory organizationToProjectHistory, Guid projectId)
        {
            var ResourceSend = await _organizationRepository.GetOrganizationResourceAsync(or => or.ResourceID.Equals(organizationToProjectHistory.OrganizationResourceID));

            if (organizationToProjectHistory != null && projectId != Guid.Empty && organizationToProjectHistory.Amount > 0 && organizationToProjectHistory.Amount <= ResourceSend.Quantity)
            {
                var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);


                var Project = await _projectVMService.GetProjectAsync(p => p.ProjectID.Equals(projectId));

                bool duplicate = false;

                var projectResource = new ProjectResource();

                foreach (var item in Project.ProjectResource)
                {
                    if (item.ResourceName.ToUpper().Contains(ResourceSend.ResourceName.ToUpper()) && item.Unit.ToUpper().Contains(ResourceSend.Unit.ToUpper()))
                    {
                        duplicate = true;
                        projectResource = item;
                        break;
                    }
                }

                if (duplicate)
                {
                    organizationToProjectHistory.ProjectResourceID = projectResource.ResourceID;
                }
                else
                {
                    var newProjectResource = new ProjectResource()
                    {
                        ProjectID = Project.ProjectID,
                        ResourceName = ResourceSend.ResourceName,
                        Quantity = 0,
                        ExpectedQuantity = organizationToProjectHistory.Amount,
                        Unit = ResourceSend.Unit,
                    };

                    await _projectRepository.AddProjectResourceAsync(newProjectResource);

                    Project = await _projectVMService.GetProjectAsync(p => p.ProjectID.Equals(projectId));
                    var projectResource1 = new ProjectResource();

                    foreach (var item in Project.ProjectResource)
                    {
                        if (item.ResourceName.ToUpper().Contains(ResourceSend.ResourceName.ToUpper()) && item.Unit.ToUpper().Contains(ResourceSend.Unit.ToUpper()))
                        {
                            duplicate = true;
                            projectResource1 = item;
                            break;
                        }
                    }

                    if (duplicate)
                    {
                        organizationToProjectHistory.ProjectResourceID = projectResource1.ResourceID;
                    }
                }
                if (await _organizationRepository.AddOrganizationToProjectHistoryAsync(organizationToProjectHistory))
                {
                    ResourceSend.Quantity -= organizationToProjectHistory.Amount;

                    if (await _organizationRepository.UpdateOrganizationResourceAsync(ResourceSend))
                        return RedirectToAction(nameof(ManageOrganizationResource));
                }

            }
            if(organizationToProjectHistory.Amount <= 0 || organizationToProjectHistory.Amount > ResourceSend.Quantity)

                ViewBag.MessageExcessQuantity = $"*Quantity more than 0 and less than equal {ResourceSend.Quantity}";

            if(projectId == Guid.Empty)
            {
                ViewBag.MessageProject = "*Choose project to send";
            }
            return View(organizationToProjectHistory);
        }

        public async Task<IActionResult> CancelSendResource(Guid transactionId)
        {
            var transactionHistory = await _organizationRepository.GetOrganizationToProjectHistoryAsync(otp => otp.TransactionID.Equals(transactionId));

            var OrganizationResource = await _organizationRepository.GetOrganizationResourceAsync(or => or.ResourceID.Equals(transactionHistory.OrganizationResourceID));

            OrganizationResource.Quantity += transactionHistory.Amount;

            await _organizationRepository.UpdateOrganizationResourceAsync(OrganizationResource);

            await _organizationRepository.DeleteOrganizationToProjectHistoryAsync(transactionId);

            return RedirectToAction(nameof(ManageOrganizationResource));
        }

        public async Task<IActionResult> DonateByMoney()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DonateByResource(Guid resourceId)
        {
            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);

            //get current user
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }

            // get current resource
            var currentResource = await _organizationRepository.GetOrganizationResourceAsync(or => or.ResourceID.Equals(resourceId));
            HttpContext.Session.Set<OrganizationResource>(MySettingSession.SESSION_Current_Organization_Resource_KEY, currentResource);

            var userToOrganizationTransactionHistory = new UserToOrganizationTransactionHistory()
            {
                ResourceID = resourceId,
                UserID = currentUser.UserID,
                Status = 0,
                Time = DateOnly.FromDateTime(DateTime.UtcNow),
            };
            return View(userToOrganizationTransactionHistory);
        }
        [HttpPost]
        public async Task<IActionResult> DonateByResource(UserToOrganizationTransactionHistory transactionHistory)
        {
            if (transactionHistory != null)
            {
                if (await _organizationRepository.AddUserToOrganizationTransactionHistoryASync(transactionHistory))
                {
                    return RedirectToAction(nameof(ManageOrganizationResource));
                }
            }

            return View(transactionHistory);
        }

        public async Task<IActionResult> ReviewDonateRequest()
        {
            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);

            var userToOrganizationTransactionHistoryInAOrganizations = await _userToOragnizationTransactionHistoryVMService.GetTransactionHistory(currentOrganization.OrganizationID);

            return View(userToOrganizationTransactionHistoryInAOrganizations);
        }

        public async Task<IActionResult> DenyRequestDonate(Guid transactionId)
        {

            await _organizationRepository.DeleteUserToOrganizationTransactionHistoryByTransactionIDAsync(transactionId);

            return RedirectToAction(nameof(ReviewDonateRequest));
        }
        public async Task<IActionResult> AcceptRquestDonate(Guid transactionId)
        {
            //update table UserToOrganizationTransactionHistory
            var userToOrganizationTransactionHistory = await _organizationRepository.GetUserToOrganizationTransactionHistoryByTransactionIDAsync(uto => uto.TransactionID.Equals(transactionId));
            userToOrganizationTransactionHistory.Status = 1;
            await _organizationRepository.UpdateUserToOrganizationTransactionHistoryAsync(userToOrganizationTransactionHistory);

            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);

            //update table resource
            var organizationResource = await _organizationRepository.GetOrganizationResourceByOrganizationIDAndResourceIDAsync(currentOrganization.OrganizationID, userToOrganizationTransactionHistory.ResourceID);
            organizationResource.Quantity += userToOrganizationTransactionHistory.Amount;
            await _organizationRepository.UpdateOrganizationResourceAsync(organizationResource);

            return RedirectToAction(nameof(ManageOrganizationResource));
        }

    }
}