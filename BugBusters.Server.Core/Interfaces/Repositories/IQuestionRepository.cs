using BugBusters.Server.Core.Dtos;
using BugBusters.Server.Core.Entities;

namespace BugBusters.Server.Core.Interfaces.Repositories;

public interface IQuestionRepository : IBaseRepository<Question>
{
    Task<List<Question>> GetByUserId(Guid userId);
    Task<Question?> GetById(Guid id, PagedRequest pagedRequest);
    Task<(List<Question> questions, int totalPages, long itemCount)> GetPagedResults(PagedRequest pagedRequest);
}