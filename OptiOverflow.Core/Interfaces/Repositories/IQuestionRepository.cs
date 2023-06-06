using DotNetCore.Repositories;
using OptiOverflow.Core.Entities;

namespace OptiOverflow.Core.Interfaces.Repositories;

public interface IQuestionRepository : IBaseRepository<Question>
{
    Task<List<Question>> GetAll();
    Task<Question?> GetById(Guid id);
}