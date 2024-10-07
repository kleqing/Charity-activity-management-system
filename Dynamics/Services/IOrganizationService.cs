﻿using Dynamics.Models.Models;
using Dynamics.Models.Models.DTO;

namespace Dynamics.Services;

public interface IOrganizationService
{
    OrganizationOverviewDto MapToOrganizationOverviewDto(Organization organization);
    List<OrganizationOverviewDto> MapToOrganizationOverviewDtoList(List<Organization> organizations);

}