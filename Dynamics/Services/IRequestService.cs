using Dynamics.Models.Models;
using Dynamics.Models.Models.Dto;

namespace Dynamics.Services;

public interface IRequestService
{
    RequestOverviewDto MapRequestToRequestOverviewDto(Request request);
    List<RequestOverviewDto> MapToListRequestOverviewDto(List<Request> requests);

}