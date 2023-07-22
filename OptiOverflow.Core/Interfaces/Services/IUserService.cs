using OptiOverflow.Core.Dtos;

namespace OptiOverflow.Core.Interfaces.Services;

public interface IUserService
{
    Task<LoggedInProfileResponseDto> Profile(Guid userId);
    Task<List<UserResponseDto>?> Get();
    Task<UserResponseDto?> GetById(Guid id);
}