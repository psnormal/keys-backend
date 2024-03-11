using KeyBooking_backend.Dto;
using KeyBooking_backend.Models;
using Microsoft.AspNetCore.Identity;
using System.CodeDom;
using System.ComponentModel.DataAnnotations;

namespace KeyBooking_backend.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public ApplicationService(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task CreateApplication(CreateApplicationDto model, string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new ValidationException("This user does not exist");
            }

            var sameApplication = _dbContext.Applications.FirstOrDefault(x => 
            x.Date == model.Date && 
            x.PeriodId == model.PeriodId && 
            x.State == Models.ApplicationState.Approved);

            if (sameApplication != null)
            {
                throw new ValidationException("This time is already taken!");
            }

            var sameSelfApplication = _dbContext.Applications.FirstOrDefault(x =>
            x.Date == model.Date &&
            x.PeriodId == model.PeriodId &&
            x.State != Models.ApplicationState.Rejected &&
            x.Owner.ToString() == user.Id);

            if (sameSelfApplication != null)
            {
                throw new ValidationException("You have already submitted a request for this time!");
            }

            var newApplication = new Application
            {
                Name = model.Name,
                Description = model.Description,
                Date = model.Date,
                PeriodId = model.PeriodId,
                KeyId = model.KeyId,
                Owner = Guid.Parse(user.Id),
                State = ApplicationState.New,
                isRepeated = false
            };

            await _dbContext.Applications.AddAsync(newApplication);
            await _dbContext.SaveChangesAsync();

        }
    }
}
