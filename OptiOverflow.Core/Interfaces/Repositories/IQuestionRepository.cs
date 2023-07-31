using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Entities;

namespace OptiOverflow.Core.Interfaces.Repositories;

public interface IQuestionRepository : IBaseRepository<Question>
{
    Task<List<Question>> GetByUserId(Guid userId);
    Task<Question?> GetById(Guid id);
    Task<(List<Question> questions, int totalPages, long itemCount)> GetPagedResults(PagedRequest pagedRequest);
}