using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Entities;

namespace OptiOverflow.Core.Interfaces.Services;

public interface IUserProfileService
{
    Task Create(RegistrationDto model, ApplicationUser applicationUser);
}