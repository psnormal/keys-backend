using KeyBooking_backend.Dto;
using KeyBooking_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace KeyBooking_backend.Services
{
    public class KeyService : IKeyService
    {
        private readonly ApplicationDbContext _context;

        public KeyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateKey(KeyCreateDto model)
        {
            var infoKey = _context.Keys.FirstOrDefault(x => x.Number == model.Number);

            if (infoKey != null)
            {
                throw new ValidationException("This key already exists");
            }

            var newKey = new Key
            {
                Number = model.Number,
                State = KeyState.Deanery
            };

            await _context.Keys.AddAsync(newKey);
            await _context.SaveChangesAsync();
        }

        public InfoKeyDto GetKeyInfo(int number)
        {
            var infoKey = _context.Keys.FirstOrDefault(x => x.Number == number);

            if (infoKey == null)
            {
                throw new ValidationException("This key does not exist");
            }

            InfoKeyDto result = new InfoKeyDto(infoKey);
            return result;
        }

        public InfoKeysDto GetKeysInfo()
        {
            var keys = _context.Keys.ToList();
            var result = new InfoKeysDto(keys);
            return result;
        }

        public async Task DeleteKey(int number)
        {
            var keyInfo = _context.Keys.FirstOrDefault(x => x.Number == number);

            if (keyInfo == null)
            {
                throw new ValidationException("This key does not exist");
            }

            _context.Keys.Remove(keyInfo);
            await _context.SaveChangesAsync();
        }

        public InfoKeysDto GetKeysUser(Guid UserId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == UserId.ToString());

            if (user == null)
            {
                throw new ValidationException("This user does not exist");
            }

            var keys = _context.Keys.Where(x => x.UserId == UserId).ToList();
            var result = new InfoKeysDto(keys);
            return result;
        }

        public async Task TransferKey(Key model)
        {
            var keyInfo = _context.Keys.FirstOrDefault(x => x.Number == model.Number);

            if (keyInfo == null)
            {
                throw new ValidationException("This key does not exist");
            }

            if (keyInfo.State == 0 && model.State == 0)
            {
                throw new ValidationException("This key alredy in deanery");
            }
            else if (keyInfo.State == KeyState.OnHands && model.State == 0)
            {
                keyInfo.State = model.State;
                keyInfo.UserId = new Guid("00000000-0000-0000-0000-000000000000");
                await _context.SaveChangesAsync();
            }
            else if (model.State == KeyState.OnHands)
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == model.UserId.ToString());

                if (user == null)
                {
                    throw new ValidationException("This user does not exist");
                }

                keyInfo.State = model.State;
                keyInfo.UserId = model.UserId;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ValidationException("Something went wrong");
            }
        }

        public InfoKeyAvailabilityDto GetKeyAvailability(int number, DateTime date)
        {
            var infoKey = _context.Keys.FirstOrDefault(x => x.Number == number);

            if (infoKey == null)
            {
                throw new ValidationException("This key does not exist");
            }

            var allPeriods = _context.Periods.ToList();
            var allPeriods2 = new List<Period>();
            var month = date.Month;
            var day = date.Day;
            var year = date.Year;
            var applicationsToDateAndKey = _context.Applications.Where(x => x.KeyId == number && x.Date.Year == year && x.Date.Month == day && x.Date.Day == month).ToList();

            /*foreach ( var application in applicationsToDateAndKey )
            {
                foreach (var per in allPeriods )
                {
                    if (per.Id != application.PeriodId)
                        allPeriods2.Add(per);
                }
            }*/

            foreach (var per in allPeriods)
            {
                foreach (var application in applicationsToDateAndKey)
                {
                    if (per.Id == application.PeriodId)
                        allPeriods2.Add(per);
                }
            }

            foreach (var per in allPeriods2)
            {
                allPeriods.Remove(per);
            }

            var result1 = new List<int>();
            foreach (var item in allPeriods)
            {
                result1.Add(item.Id);
            }

            InfoKeyAvailabilityDto result = new InfoKeyAvailabilityDto(result1);

            return result;
        }
    }
}
