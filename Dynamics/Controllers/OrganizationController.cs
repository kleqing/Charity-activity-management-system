using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Dynamics.Utility;
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
            var organizations = await _organizationRepository.GetAllOrganizationsAsync();
            return View(organizations);
        }

        //GET: /Organization/Create
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //POST: /Organizations/Create
        [HttpPost]
        public async Task<IActionResult> Create(Organization organization)
        {
            await _organizationRepository.AddAsync(organization);
           
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MyOrganizationCeo(string userId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.CEOID.Equals(userId));
            return View(organization);
        }

        public async Task<IActionResult> Detail(int organizationId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.OrganizationID == organizationId);
            if (organization == null)
            {
                return NotFound();  
            }
            var user= await _userRepository.Get(u => u.UserID.Equals(organization.CEOID));
            @ViewBag.Ceo = user;
            return View(organization);
        }

        public async Task<IActionResult> Edit(int? organizationId)
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
                try
                {
                    if (image != null)
                    {
                        organization.OrganizationPictures = Util.UploadImage(image, @"images\Organization", organization.OrganizationID + "");
                        await _organizationRepository.UpdateAsync(organization);
                        return RedirectToAction("Detail", new { organizationId = organization.OrganizationID });
                    }
                    await _organizationRepository.UpdateAsync(organization);
                    return RedirectToAction("Detail", new { organizationId = organization.OrganizationID });
                }
                catch (Exception ex)
                {

                } 
                
            }
            return View(organization);
        }

        public async Task<IActionResult> ManageOrganizationMember(int? organizationId)
        {
            var organizationMembers = await _organizationRepository.GetAllOrganizationMemberByOrganizationIDAsync(om => om.OrganizationID == organizationId);
            List<User> users = null;
            foreach(var item in organizationMembers)
            {
                var user = await _userRepository.Get(u => u.UserID.Equals(item.UserID));
                users.Add(user);
            }
            return View(users);
        }

    }
}
