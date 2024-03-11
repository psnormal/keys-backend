using KeyBooking_backend.Dto;

namespace KeyBooking_backend.Services
{
    public interface IApplicationService
    {
        Task CreateApplication(CreateApplicationDto model, string userEmail);
        ApplicationInfoDto GetApplicationInfo(string id);
        ApplicationsListDto GetApplicationsInfo();
        Task ApproveApplication(string id);
        Task RejectApplication(string id);
        Task RecallApplication(string id, string userEmail);
    }
}
