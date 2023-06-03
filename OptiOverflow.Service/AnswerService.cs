using AutoMapper;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Repositories;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Service;

public class AnswerService : IAnswerService
{
    private readonly IMapper _mapper;
    private readonly IAnswerRepository _answerRepository;

    public AnswerService(IMapper mapper,
        IAnswerRepository answerRepository)
    {
        _mapper = mapper;
        _answerRepository = answerRepository;
    }

    public async Task<AnswerDto> Create(AnswerCreateDto answerCreateDto, Guid userId)
    {
        var answerEntity = _mapper.Map<Answer>(answerCreateDto);
        answerEntity.QuestionId = Guid.Parse(answerCreateDto.QuestionId);
        answerEntity.UserId = userId;
        await _answerRepository.AddAsync(answerEntity);
        await _answerRepository.SaveChangesAsync();
        return _mapper.Map<AnswerDto>(answerEntity);
    }

    public async Task<AnswerDto?> Update(AnswerUpdateDto answerUpdateDto, Guid id, Guid userId)
    {
        var answerEntity = _mapper.Map<Answer>(answerUpdateDto);
        answerEntity.Id = id;

        await _answerRepository.UpdateAsync(answerEntity);
        await _answerRepository.SaveChangesAsync();
        return _mapper.Map<AnswerDto>(answerEntity);
    }

    public async Task Delete(Guid id)
    {
        await _answerRepository.SoftDeleteAsync(id);
        await _answerRepository.SaveChangesAsync();
    }
}