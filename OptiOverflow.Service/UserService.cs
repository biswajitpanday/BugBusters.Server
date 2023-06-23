using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Repositories;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Service;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserProfileRepository _profileRepository;

    public UserService(IMapper mapper, 
        UserManager<ApplicationUser> userManager, 
        IUserProfileRepository profileRepository)
    {
        _mapper = mapper;
        _userManager = userManager;
        _profileRepository = profileRepository;
    }


    public async Task<List<UserResponseDto>?> Get()
    {
        var applicationUsers = await _userManager.Users.ToListAsync();
        var userProfile = await _profileRepository.ListAsync();

        if (!applicationUsers.Any() || !userProfile.Any())
            return null;

        var userList = new List<UserResponseDto>();
        foreach (var applicationUser in applicationUsers)
        {
            var profile = userProfile.First(x => x.AccountId == applicationUser.Id);
            var userResponseDto = _mapper.Map<UserResponseDto>(profile);
            MapApplicationUser(userResponseDto, applicationUser);
            userList.Add(userResponseDto);
        }
        return userList;
    }

    public async Task<UserResponseDto?> GetById(Guid id)
    {
        var applicationUser = await _userManager.FindByIdAsync(id.ToString());
        var userProfile = await _profileRepository.Queryable.FirstOrDefaultAsync(x => x.AccountId == applicationUser.Id);

        if(applicationUser == null || userProfile == null) 
            return null;

        var userResponseDto = _mapper.Map<UserResponseDto>(userProfile);
        MapApplicationUser(userResponseDto, applicationUser);
        return userResponseDto;
    }

    #region Private Methods

    private static void MapApplicationUser(UserResponseDto userResponseDto, ApplicationUser applicationUser)
    {
        userResponseDto.UserName = applicationUser.UserName;
        userResponseDto.Email = applicationUser.Email;
        userResponseDto.EmailConfirmed = applicationUser.EmailConfirmed;
        userResponseDto.LockoutEnabled = applicationUser.LockoutEnabled;
    }

    #endregion
}