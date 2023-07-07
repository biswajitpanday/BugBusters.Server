using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Service;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }


    public async Task<List<UserResponseDto>?> Get()
    {
        var applicationUsers = await _userManager.Users.ToListAsync();
        if (!applicationUsers.Any())
            return null;

        var userList = _mapper.Map<List<UserResponseDto>>(applicationUsers);
        return userList;
    }

    public async Task<UserResponseDto?> GetById(Guid id)
    {
        var applicationUser = await _userManager.FindByIdAsync(id.ToString());
        if (applicationUser == null)
            return null;

        var userResponseDto = _mapper.Map<UserResponseDto>(applicationUser);
        return userResponseDto;
    }
}