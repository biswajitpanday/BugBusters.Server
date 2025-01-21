using AutoMapper;
using BugBusters.Server.Core.Dtos;
using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Repositories;
using BugBusters.Server.Core.Interfaces.Services;

namespace OptiOverflow.Service;

public class AnswerService : IAnswerService
{
    private readonly IMapper _mapper;
    private readonly IAnswerRepository _answerRepository;

    public AnswerService(IMapper mapper, IAnswerRepository answerRepository)
    {
        _mapper = mapper;
        _answerRepository = answerRepository;
    }

    public async Task<AnswerResponseDto?> Accept(Guid id, Guid userId)
    {
        var answerEntity = await _answerRepository.GetAsync(id);
        if (answerEntity == null)
            return null;
        if (answerEntity.CreatedById == userId)
            throw new InvalidOperationException("You can't accept your own answer!");
        answerEntity.IsAccepted = true;
        await _answerRepository.UpdateAsync(answerEntity);
        await _answerRepository.SaveChangesAsync();
        return _mapper.Map<AnswerResponseDto>(answerEntity);
    }

    public async Task<AnswerResponseDto> Create(AnswerCreateDto answerCreateDto, Guid userId)
    {
        var answerEntity = _mapper.Map<Answer>(answerCreateDto);
        answerEntity.QuestionId = Guid.Parse(answerCreateDto.QuestionId);
        answerEntity.CreatedById = userId;
        await _answerRepository.AddAsync(answerEntity);
        await _answerRepository.SaveChangesAsync();
        return _mapper.Map<AnswerResponseDto>(answerEntity);
    }

    public async Task<AnswerResponseDto?> Update(AnswerUpdateDto answerUpdateDto, Guid id, Guid userId)
    {
        var answerEntity = _mapper.Map<Answer>(answerUpdateDto);
        answerEntity.Id = id;

        await _answerRepository.UpdateAsync(answerEntity);
        await _answerRepository.SaveChangesAsync();
        return _mapper.Map<AnswerResponseDto>(answerEntity);
    }

    public async Task Delete(Guid id)
    {
        await _answerRepository.SoftDeleteAsync(id);
        await _answerRepository.SaveChangesAsync();
    }
}