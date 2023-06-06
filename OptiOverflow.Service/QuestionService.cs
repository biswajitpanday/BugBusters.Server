using AutoMapper;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Repositories;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Service;

public class QuestionService : IQuestionService
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

    public async Task<List<QuestionDto>?> GetAll()
    {
        var questions = await _questionRepository.GetAll();
        if (questions.Count <= 0)
            return null;
        var questionsDto = _mapper.Map<List<QuestionDto>>(questions);
        foreach (var question in questionsDto)
        {
            var vote = questions.First(x => x.Id == question.Id).Votes;
            if (vote == null) continue;
            question.UpVoteCount = vote.Count(v => v.IsUpVote);
            question.DownVoteCount = vote.Count(v => !v.IsUpVote);
        }
        return questionsDto;
    }

    public async Task<QuestionDto?> GetById(Guid id)
    {
        var question = await _questionRepository.GetById(id);
        if (question == null)
            return null;
        var questionDto = _mapper.Map<QuestionDto?>(question);

        if (questionDto != null)
        {
            if (question.Votes != null)
            {
                questionDto.UpVoteCount = question.Votes.Count(v => v.IsUpVote);
                questionDto.DownVoteCount = question.Votes.Count(v => !v.IsUpVote);

                if (question.Answers != null && questionDto.Answers != null)
                {
                    foreach (var answer in questionDto.Answers)
                    {

                        if (answer != null)
                        {
                            var votes = question.Answers.First(x => x.Id == answer.Id).Votes;
                            if(votes == null) continue;
                            answer.UpVoteCount = votes.Count(v => v.IsUpVote);
                            answer.DownVoteCount = votes.Count(v => !v.IsUpVote);
                        }
                    }
                }
            }
        }

        return questionDto;
    }

    public async Task<QuestionDto> Create(QuestionCreateDto questionCreateDto, Guid userId)
    {
        var questionEntity = _mapper.Map<Question>(questionCreateDto);
        questionEntity.CreatedById = userId;
        questionEntity.LastUpdatedById = userId;
        await _questionRepository.AddAsync(questionEntity);
        await _questionRepository.SaveChangesAsync();
        return _mapper.Map<QuestionDto>(questionEntity);
    }

    public async Task<QuestionDto?> Update(QuestionUpdateDto questionUpdateDto, Guid id, Guid userId)
    {
        var questionEntity = _mapper.Map<Question>(questionUpdateDto);
        questionEntity.Id = id;
        questionEntity.LastUpdatedById = userId;
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