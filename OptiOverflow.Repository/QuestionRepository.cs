using Microsoft.EntityFrameworkCore;
using OptiOverflow.Core.Dtos;
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
            .Include(x => x.Answers)
            .Include(x => x.CreatedBy)
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
        return questions;
    }

    public async Task<(List<Question> questions, int totalPages, long itemCount)> GetPagedResults(PagedRequest pagedRequest)
    {
        var questions = Queryable.Where(x => !x.IsDeleted);
        var totalDataCount = await questions.CountAsync();

        if (!string.IsNullOrEmpty(pagedRequest.Query))
        {
            questions = questions.Where(
                x => x.Title.Contains(pagedRequest.Query) || x.Body.Contains(pagedRequest.Query));
            totalDataCount = await questions.CountAsync();
        }
        else
        {
            questions = questions
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pagedRequest.Page) * pagedRequest.PageSize)
                .Take(pagedRequest.PageSize)
                .Include(x => x.Votes)
                .Include(x => x.Answers)
                .Include(x => x.CreatedBy)
                .AsNoTracking();
        }

        var pagedQuestions = await questions.ToListAsync();

        var totalPageCount = (int)Math.Ceiling(totalDataCount / (double)pagedRequest.PageSize);
        return (pagedQuestions, totalPageCount, totalDataCount);
    }

    public async Task<Question?> GetById(Guid id)
    {
        var question = await Queryable
            .Include(x => x.Votes)
            .Include(x => x.CreatedBy)
            .Include(x => x.Answers).ThenInclude(x => x.CreatedBy)
            .Include(x => x.Answers).ThenInclude(x => x.Votes)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        return question;
    }

    public async Task<List<Question>> GetByUserId(Guid userId)
    {
        var questions = await Queryable
            .Include(x => x.Votes)
            .Where(x => x.CreatedById == userId)
            .AsNoTracking()
            .ToListAsync();
        return questions;
    }
}