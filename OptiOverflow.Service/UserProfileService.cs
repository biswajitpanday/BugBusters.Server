using AutoMapper;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Repositories;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Service;

public class UserProfileService : IUserProfileService
{
    private readonly IMapper _mapper;
    private readonly IUserProfileRepository _userProfileRepository;

    public UserProfileService(IMapper mapper, IUserProfileRepository userProfileRepository)
    {
        _mapper = mapper;
        _userProfileRepository = userProfileRepository;
    }
    
    public async Task Create(RegistrationDto model, ApplicationUser user)
    {
        var userProfile = _mapper.Map<UserProfile>(model);
        userProfile.AccountId = user.Id;
        await _userProfileRepository.AddAsync(userProfile);
        await _userProfileRepository.SaveChangesAsync();
    }
}