using AutoMapper;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Repositories;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Service;

public class QuestionService: IQuestionService
{
    private readonly IMapper _mapper;
    private readonly IQuestionRepository _questionRepository;
    private readonly IVoteRepository _voteRepository;

    public QuestionService(IMapper mapper, 
        IQuestionRepository questionRepository, 
        IVoteRepository voteRepository)
    {
        _mapper = mapper;
        _questionRepository = questionRepository;
        _voteRepository = voteRepository;
    }

    public async Task<List<QuestionDto>> GetAll()
    {
        var questions = await _questionRepository.ListAsync();
        var questionsDto = _mapper.Map<List<QuestionDto>>(questions);
        return questionsDto;
    }

    public async Task<QuestionDto?> GetById(Guid id)
    {
        var question = await _questionRepository.GetAsync(id);
        var questionDto = _mapper.Map<QuestionDto?>(question);
        return questionDto;
    }

    public async Task<QuestionDto> Create(QuestionCreateDto questionCreateDto, string userId)
    {
        var questionEntity = _mapper.Map<Question>(questionCreateDto);
        questionEntity.CreatedById = new Guid(userId);
        questionEntity.LastUpdatedById = new Guid(userId);
        await _questionRepository.AddAsync(questionEntity);
        //var vote = new Vote{QuestionId = questionEntity.Id, VoteType = }  // Todo: Handle Vote Create.

        await _questionRepository.SaveChangesAsync();
        return _mapper.Map<QuestionDto>(questionEntity);
    }

    public async Task<QuestionDto?> Update(QuestionUpdateDto questionUpdateDto, Guid id, string? userId)
    {
        var questionEntity = _mapper.Map<Question>(questionUpdateDto);
        questionEntity.Id = id;
        questionEntity.LastUpdatedById = new Guid(userId);
        
        await _questionRepository.UpdateAsync(questionEntity);
        await _questionRepository.SaveChangesAsync();
        return _mapper.Map<QuestionDto>(questionEntity);
    }

    public async Task Delete(Guid id)
    {
        await _questionRepository.SoftDeleteAsync(id);
        await _questionRepository.SaveChangesAsync();
    }
}