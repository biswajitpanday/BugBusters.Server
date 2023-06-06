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
        var questions = await Queryable
            .Where(x => !x.IsDeleted)
            .Include(x => x.Votes)
            .AsNoTracking()
            .ToListAsync();
        return questions;
    }

    public async Task<Question?> GetById(Guid id)
    {
        var question = await Queryable
            .Include(x => x.Votes)
            .Include(x => x.Answers)
            .ThenInclude(x => x.Votes)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            
        return question;
    }
}