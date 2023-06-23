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
        var user = await _userManager.Users.ToListAsync();
        var userProfile = await _profileRepository.ListAsync();

        var userList = new List<UserResponseDto>();
        foreach (var applicationUser in user)
        {
            var profile = userProfile.First(x => x.AccountId == applicationUser.Id);
            var userResponseDto = _mapper.Map<UserResponseDto>(profile);
            userResponseDto.UserName = applicationUser.UserName;
            userResponseDto.Email = applicationUser.Email;
            userResponseDto.EmailConfirmed = applicationUser.EmailConfirmed;
            userResponseDto.LockoutEnabled = applicationUser.LockoutEnabled;
            userList.Add(userResponseDto);
        }
        return userList;
    }
}