using OptiOverflow.Core.Entities;

namespace OptiOverflow.Core.Interfaces.Repositories;

public interface IQuestionRepository : IBaseRepository<Question>
{
    Task<List<Question>> GetAll();
    Task<List<Question>> GetByUserId(Guid userId);
    Task<Question?> GetById(Guid id);
}