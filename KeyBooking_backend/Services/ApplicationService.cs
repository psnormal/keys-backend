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

        public ApplicationInfoDto GetApplicationInfo(string id)
        {

            var application = _dbContext.Applications.FirstOrDefault(x => x.Id == int.Parse(id));

            if (application == null)
            {
                throw new ValidationException("This application does not exist");
            }

            ApplicationInfoDto result = new ApplicationInfoDto
            {
                Id = application.Id,
                Name = application.Name,
                Description = application.Description,
                Date = application.Date,
                PeriodId = application.PeriodId,
                KeyId = application.KeyId,
                Owner = application.Owner,
                State = application.State,
                isRepeated = application.isRepeated
            };
            return result;
        }

        public ApplicationsListDto GetApplicationsInfo()
        {
            var applications = _dbContext.Applications.ToList();
            var listedResult = new List<ApplicationInfoDto>();
            foreach (var application in applications)
            {
                ApplicationInfoDto applicationInfo = new ApplicationInfoDto
                {
                    Id = application.Id,
                    Name = application.Name,
                    Description = application.Description,
                    Date = application.Date,
                    PeriodId = application.PeriodId,
                    KeyId = application.KeyId,
                    Owner = application.Owner,
                    State = application.State,
                    isRepeated = application.isRepeated
                };
                listedResult.Add(applicationInfo);
            }
            var result = new ApplicationsListDto(listedResult);
            return result;
        }
    }
}
