using KeyBooking_backend.Dto;

namespace KeyBooking_backend.Services
{
    public interface IApplicationService
    {
        Task CreateApplication(CreateApplicationDto model, string userEmail);
    }
}
