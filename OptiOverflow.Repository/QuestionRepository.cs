using Microsoft.EntityFrameworkCore;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Repositories;
using OptiOverflow.Repository.Base;
using OptiOverflow.Repository.DatabaseContext;

namespace OptiOverflow.Repository;

public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
{
    public QuestionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Question>> GetAll()
    {
        var questions = await Queryable.Include(x => x.Votes).AsNoTracking().ToListAsync();
        return questions;
    }
}