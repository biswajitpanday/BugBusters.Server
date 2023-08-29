using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<ApplicationUser> _userManager;

    public QuestionService(IMapper mapper,
        IQuestionRepository questionRepository,
        IVoteRepository voteRepository,
        UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _questionRepository = questionRepository;
        _voteRepository = voteRepository;
        _userManager = userManager;
    }

    public async Task<PagedResponse<List<QuestionResponseDto>>?>? GetAll(PagedRequest pagedRequest)
    {
        var (questions, totalPages, itemCount) = await _questionRepository.GetPagedResults(pagedRequest);
        if (questions.Count <= 0)
            return null;
        var questionsDto = HandleQuestionListResponse(questions);
        return new PagedResponse<List<QuestionResponseDto>>
        {
            Items = questionsDto,
            TotalPages = totalPages,
            ItemCount = itemCount
        };
    }

    public async Task<QuestionResponseDto?> GetById(Guid id, PagedRequest pagedRequest)
    {
        var question = await _questionRepository.GetById(id, pagedRequest);
        if (question == null)
            return null;
        var questionDto = _mapper.Map<QuestionResponseDto?>(question);

        if (questionDto != null)
        {
            questionDto.CreatedBy = _mapper.Map<ProfileResponseDto>(question.CreatedBy);
            if (question.Votes != null)
            {
                questionDto.UpVoteCount = question.Votes.Count(v => v.IsUpVote);
                questionDto.DownVoteCount = question.Votes.Count(v => !v.IsUpVote);
                if (question.Answers != null && question.Answers.Any())
                {
                    var totalAnswerCount = question.Answers.Count;
                    var pagedAnswers = question.Answers;
                    if (!string.IsNullOrEmpty(pagedRequest.Query))
                    {
                        pagedAnswers = pagedAnswers.Where(x => x.Body.ToLower().Contains(pagedRequest.Query.ToLower())).ToList();
                        totalAnswerCount = pagedAnswers.Count;
                    }

                    pagedAnswers = pagedAnswers
                        .Skip((pagedRequest.Page) * pagedRequest.PageSize)
                        .Take(pagedRequest.PageSize).ToList();
                    var totalPageCount = (int)Math.Ceiling(totalAnswerCount / (double)pagedRequest.PageSize);
                    
                    if (questionDto is { Answers: not null })
                    {
                        questionDto.Answers = _mapper.Map<List<AnswerResponseDto>>(pagedAnswers);
                        foreach (var answer in questionDto.Answers)
                        {
                            var votes = question.Answers.First(x => x.Id == answer.Id).Votes;
                            if (votes == null) continue;
                            answer.UpVoteCount = votes.Count(v => v.IsUpVote);
                            answer.DownVoteCount = votes.Count(v => !v.IsUpVote);
                        }

                        questionDto.PagedAnswers = new PagedResponse<List<AnswerResponseDto>?>
                        {
                            Items = questionDto.Answers.OrderByDescending(x => x?.CreatedAt ?? default).ToList(),
                            ItemCount = totalAnswerCount,
                            TotalPages = totalPageCount
                        };
                    }
                }
            }
        }

        return questionDto;
    }

    public async Task<QuestionResponseDto> Create(QuestionCreateDto questionCreateDto, Guid userId)
    {
        var questionEntity = _mapper.Map<Question>(questionCreateDto);
        questionEntity.CreatedById = userId;
        questionEntity.LastUpdatedById = userId;
        await _questionRepository.AddAsync(questionEntity);
        await _questionRepository.SaveChangesAsync();
        return _mapper.Map<QuestionResponseDto>(questionEntity);
    }

    public async Task<QuestionResponseDto?> Update(QuestionUpdateDto questionUpdateDto, Guid id, Guid userId)
    {
        var questionEntity = _mapper.Map<Question>(questionUpdateDto);
        questionEntity.Id = id;
        questionEntity.LastUpdatedById = userId;
        await _questionRepository.UpdateAsync(questionEntity);
        await _questionRepository.SaveChangesAsync();
        return _mapper.Map<QuestionResponseDto>(questionEntity);
    }

    public async Task Delete(Guid id)
    {
        await _questionRepository.SoftDeleteAsync(id);
        await _questionRepository.SaveChangesAsync();
    }

    #region Private Methods

    private List<QuestionResponseDto> HandleQuestionListResponse(List<Question> questions)
    {
        var questionsDto = _mapper.Map<List<QuestionResponseDto>>(questions);
        foreach (var question in questionsDto)
        {
            var questionEntity = questions.First(x => x.Id == question.Id);
            var vote = questionEntity.Votes;
            if (vote == null) continue;
            question.UpVoteCount = vote.Count(v => v.IsUpVote);
            question.DownVoteCount = vote.Count(v => !v.IsUpVote);
            if (question.Answers != null)
            {
                question.AnswerCount = question.Answers.Count;
                question.HasAcceptedAnswer = question.Answers.Any(x => x.IsAccepted);
            }

            question.CreatedBy = _mapper.Map<ProfileResponseDto>(questionEntity.CreatedBy);
            question.Answers = null;
        }

        return questionsDto;
    }

    #endregion
}