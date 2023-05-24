using OptiOverflow.Core.Dtos;

namespace OptiOverflow.Core.Interfaces.Services;

public interface IUserProfileService
{
    Task Create(RegistrationDto model);
}