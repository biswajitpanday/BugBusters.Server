﻿using OptiOverflow.Core.Dtos;

namespace OptiOverflow.Core.Interfaces.Services;

public interface IQuestionService
{
    Task<List<QuestionDto>> GetAll();
    Task<QuestionDto?> GetById(Guid id);
    Task<QuestionDto> Create(QuestionCreateDto question, string userId);
    Task<QuestionDto?> Update(QuestionUpdateDto question, Guid id, string? userId);
    Task<QuestionDto?> Delete(Guid id);
}