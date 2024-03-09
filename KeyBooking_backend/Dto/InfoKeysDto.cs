using KeyBooking_backend.Models;

namespace KeyBooking_backend.Dto
{
    public class InfoKeysDto
    {
        public List<Key> allKeys { get; set; }

        public InfoKeysDto(List<Key> keys)
        {
            allKeys = keys;
        }
    }
}
