using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    private readonly IQuestionRepository _questionRepository;
    private readonly IAnswerRepository _answerRepository;
    private readonly IVoteRepository _voteRepository;

    public UserService(IMapper mapper, 
        UserManager<ApplicationUser> userManager, 
        IQuestionRepository questionRepository,
        IAnswerRepository answerRepository,
        IVoteRepository voteRepository)
    {
        _mapper = mapper;
        _userManager = userManager;
        _questionRepository = questionRepository;
        _answerRepository = answerRepository;
        _voteRepository = voteRepository;
    }

    public async Task<LoggedInProfileResponseDto> Profile(Guid userId)
    {
        // Todo: Get All the data from Repository by a Linq Join operation.
        var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
        var questionCount = await _questionRepository.CountAsync(x => x.CreatedById == userId);
        var answerCount = await _answerRepository.CountAsync(x => x.CreatedById == userId);
        var upVoteCount = await _voteRepository.CountAsync(x => x.UserId == userId && x.IsUpVote);
        var downVoteCount = await _voteRepository.CountAsync(x => x.UserId == userId && !x.IsUpVote);
        
        var response = _mapper.Map<LoggedInProfileResponseDto>(user);
        response.QuestionAsked = questionCount;
        response.Answered = answerCount;
        response.UpVoteCount = upVoteCount;
        response.DownVoteCount = downVoteCount;
        return response;
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
        
        var askedQuestions = await _questionRepository.GetByUserId(applicationUser.Id);
        var userResponseDto = _mapper.Map<UserResponseDto>(applicationUser);
        userResponseDto.Questions = _mapper.Map<List<QuestionResponseDto>>(askedQuestions);

        foreach (var askedQuestion in askedQuestions)
        {
            if (askedQuestion.Votes != null)
            {
                userResponseDto.Questions.First(x => x.Id == askedQuestion.Id).UpVoteCount = askedQuestion.Votes.Count(v => v.IsUpVote);
                userResponseDto.Questions.First(x => x.Id == askedQuestion.Id).DownVoteCount = askedQuestion.Votes.Count(v => !v.IsUpVote);
            }
        }
        

        
        return userResponseDto;
    }

    public async Task<ProfileResponseDto?> UpdateProfile(ProfileUpdateDto profileResponseDto, Guid userId)
    {
        var existingProfile = await _userManager.FindByIdAsync(userId.ToString());
        if(existingProfile == null)
            return null;

        existingProfile.FirstName = profileResponseDto.FirstName;
        existingProfile.MiddleName = profileResponseDto.MiddleName;
        existingProfile.LastName = profileResponseDto.LastName;
        existingProfile.DateOfBirth = profileResponseDto.DateOfBirth;
        existingProfile.Address =  profileResponseDto.Address;
        existingProfile.PhoneNumber = profileResponseDto.PhoneNumber;
        existingProfile.LastUpdate = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(existingProfile);
        return result.Succeeded ? _mapper.Map<ProfileResponseDto>(existingProfile) : null;
    }
}