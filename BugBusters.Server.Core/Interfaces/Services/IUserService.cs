using BugBusters.Server.Core.Dtos;

namespace BugBusters.Server.Core.Interfaces.Services;

public interface IUserService
{
    Task<LoggedInProfileResponseDto> Profile(Guid userId);
    Task<List<UserResponseDto>?> Get();
    Task<UserResponseDto?> GetById(Guid id);
    Task<ProfileResponseDto?> UpdateProfile(ProfileUpdateDto profileResponseDto, Guid userId);
}