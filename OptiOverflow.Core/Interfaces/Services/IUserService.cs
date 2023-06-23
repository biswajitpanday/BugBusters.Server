using OptiOverflow.Core.Dtos;

namespace OptiOverflow.Core.Interfaces.Services;

public interface IUserService
{
    Task<List<UserResponseDto>?> Get();
}