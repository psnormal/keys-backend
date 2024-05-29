using KeyBooking_backend.Dto;
using KeyBooking_backend.Models;
using Microsoft.AspNetCore.Identity;
using System.CodeDom;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

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

        public async Task ApproveApplication(string id)
        {
            var application = _dbContext.Applications.FirstOrDefault(x => x.Id == int.Parse(id));

            if (application == null)
            {
                throw new ValidationException("This application does not exist!");
            }
            if (application.State != ApplicationState.New)
            {
                throw new ValidationException("You can approve only new applications!");
            }

            application.State = ApplicationState.Approved;


            var sameApplications = _dbContext.Applications
                .Where(x => x.Date == application.Date)
                .Where(x =>
                    x.PeriodId == application.PeriodId &&
                    x.State == Models.ApplicationState.New)
                .Where( x => x != application);

            if (sameApplications != null)
            {
                foreach (var sameApplication in sameApplications)
                {
                    sameApplication.State = ApplicationState.Approved;
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task RejectApplication(string id)
        {
            var application = _dbContext.Applications.FirstOrDefault(x => x.Id == int.Parse(id));

            if (application == null)
            {
                throw new ValidationException("This application does not exist!");
            }
            if (application.State == ApplicationState.Rejected)
            {
                throw new ValidationException("You can't reject already rejected applications!");
            }

            var key = _dbContext.Keys.FirstOrDefault(x => x.Number == application.KeyId);

            if (key == null)
            {
                throw new ValidationException();
            }

            if (application.State == ApplicationState.Approved && key.State == KeyState.OnHands && key.UserId == application.Owner)
            {
                throw new ValidationException("You cannot reject the application for which the key was issued!");
            }

            application.State = ApplicationState.Rejected;

            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateApplication(CreateApplicationDto model, string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new ValidationException("This user does not exist!");
            }

            var key = _dbContext.Keys.FirstOrDefault(x => x.Number == model.KeyId);
            if (key == null)
            {
                throw new ValidationException("Key mentioned in application does not exist!");
            }

            var period = _dbContext.Periods.FirstOrDefault(x => x.Id == model.PeriodId);
            if (period == null)
            {
                throw new ValidationException("Period mentioned in application does not exist!");
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
                throw new ValidationException("This application does not exist!");
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

        public async Task RecallApplication(string id, string userEmail)
        {


            var application = _dbContext.Applications.FirstOrDefault(x => x.Id == int.Parse(id));

            if (application == null)
            {
                throw new ValidationException("This application does not exist!");
            }

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new ValidationException();
            }

            if (application.Owner.ToString() != user.Id)
            {
                throw new ValidationException("The application you are trying to withdraw does not belong to you!");
            }

            var key = _dbContext.Keys.FirstOrDefault(x => x.Number == application.KeyId);
            if (key == null)
            {
                throw new ValidationException();
            }

            if (application.State == ApplicationState.Approved && key.State == KeyState.OnHands && key.UserId == application.Owner)
            {
                throw new ValidationException("You cannot recall the application for which the key was issued!");
            }

            _dbContext.Entry(application).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;


            await _dbContext.SaveChangesAsync();

        }

        public async Task<ApplicationsListDto> GetMyApplicationsInfo(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new ValidationException();
            }

            var applications = _dbContext.Applications.Where(x => x.Owner.ToString() == user.Id).ToList();
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

        public async Task<ApplicationInfoDto> GetMyApplicationInfo(string id, string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new ValidationException();
            }

            var application = _dbContext.Applications.FirstOrDefault(x => x.Id == int.Parse(id) && x.Owner.ToString() == user.Id);

            if (application == null)
            {
                throw new ValidationException("This application does not exist!");
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
    }
}
