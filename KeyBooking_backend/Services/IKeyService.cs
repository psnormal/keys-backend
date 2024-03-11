using KeyBooking_backend.Dto;
using KeyBooking_backend.Models;

namespace KeyBooking_backend.Services
{
    public interface IKeyService
    {
        Task CreateKey(KeyCreateDto model);
        InfoKeyDto GetKeyInfo(int number);
        InfoKeysDto GetKeysInfo();
        Task DeleteKey(int number);
        InfoKeysDto GetKeysUser(Guid UserId);
        Task TransferKey(Key model);
        InfoKeyAvailabilityDto GetKeyAvailability(int number, DateOnly date);
    }
}
