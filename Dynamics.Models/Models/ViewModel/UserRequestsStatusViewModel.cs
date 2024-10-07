﻿using Dynamics.Models.Models.DTO;

namespace Dynamics.Models.Models.ViewModel;

public class UserRequestsStatusViewModel
{
    public List<OrganizationMember> OrganizationJoinRequests { get; set; }
    public List<ProjectMember> ProjectJoinRequests { get; set; }
    public List<UserTransactionDto> ResourcesDonationRequests { get; set; }
}