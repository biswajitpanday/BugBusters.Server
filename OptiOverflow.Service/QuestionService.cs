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

    public async Task<QuestionDto> Create(QuestionCreateDto question, string userId)
    {
        var questionEntity = _mapper.Map<Question>(question);
        questionEntity.CreatedById = new Guid(userId);
        questionEntity.LastUpdatedById = new Guid(userId);
        await _questionRepository.AddAsync(questionEntity);
        //var vote = new Vote{QuestionId = questionEntity.Id, VoteType = }  // Todo: Handle Vote Create.

        await _questionRepository.SaveChangesAsync();
        return _mapper.Map<QuestionDto>(question);
    }

    public async Task<QuestionDto?> Update(QuestionUpdateDto question, Guid id, string? userId)
    {
        var existingQuestion = await _questionRepository.GetAsync(id);
        if (existingQuestion == null) 
            return null;

        existingQuestion.Title = question.Title;
        existingQuestion.Body = question.Body;
        existingQuestion.LastUpdatedById = new Guid(userId);
        await _questionRepository.UpdateAsync(existingQuestion);
        await _questionRepository.SaveChangesAsync();
        return _mapper.Map<QuestionDto>(existingQuestion);
    }

    public async Task<QuestionDto?> Delete(Guid id)
    {
        var existingQuestion = await _questionRepository.GetAsync(id);
        if (existingQuestion == null)
            return null;
        await _questionRepository.DeleteAsync(existingQuestion);
        await _questionRepository.SaveChangesAsync();
        return _mapper.Map<QuestionDto>(existingQuestion);
    }
}