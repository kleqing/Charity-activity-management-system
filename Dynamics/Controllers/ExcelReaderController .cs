using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using Dynamics.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Dynamics.Models.Models.ViewModel;
using Dynamics.Utility;
using Newtonsoft.Json;
using Dynamics.DataAccess.Repository;



namespace Dynamics.Controllers
{
    public class ExcelReaderController : Controller
    {

        IOrganizationRepository _organizationRepository;

        public ExcelReaderController(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<ActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);

                    //get current user
                    var userString = HttpContext.Session.GetString("user");
                    User currentUser = null;
                    if (userString != null)
                    {
                        currentUser = JsonConvert.DeserializeObject<User>(userString);
                    }

                    using (var package = new ExcelPackage(file.OpenReadStream()))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet != null)
                        {
                            int rowCount = worksheet.Dimension.Rows;

                            for (int row = 2; row <= rowCount; row++) // Assuming first row is header
                            {
                                var resource = new OrganizationResource()
                                {
                                    ResourceName = worksheet.Cells[row, 1].Value?.ToString(),
                                    Quantity = Convert.ToInt32(worksheet.Cells[row, 2].Value) >= 0 ? Convert.ToInt32(worksheet.Cells[row, 2].Value) : 0,
                                    Unit = worksheet.Cells[row, 3].Value?.ToString(),
                                };

                                if(resource.Quantity == 0)
                                {
                                    continue;
                                }

                                // get current resource
                                var currentResource = await _organizationRepository.GetOrganizationResourceAsync(or => or.ResourceName.Equals(resource.ResourceName) && or.Unit.Equals(resource.Unit));

                                var userToOrganizationTransactionHistory = new UserToOrganizationTransactionHistory()
                                {
                                    ResourceID = currentResource.ResourceID,
                                    UserID = currentUser.UserID,
                                    Status = 0,
                                    Time = DateOnly.FromDateTime(DateTime.UtcNow),
                                    Amount = resource.Quantity,
                                    Message = worksheet.Cells[row, 4].Value?.ToString(),
                                };

                                await _organizationRepository.AddUserToOrganizationTransactionHistoryASync(userToOrganizationTransactionHistory);
                            }
                        }
                    }
                   return RedirectToAction("ManageOrganizationResource", "Organization");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error: " + ex.Message;
                    return RedirectToAction("ManageOrganizationResource", "Organization");
                }
            }

            ViewBag.Error = "Please select a file to upload.";
            return RedirectToAction("ManageOrganizationResource", "Organization");
        }
    }
}
