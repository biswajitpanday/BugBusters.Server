using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Repositories;
using BugBusters.Server.Repository.Base;
using BugBusters.Server.Repository.DatabaseContext;

namespace BugBusters.Server.Repository;

public class AnswerRepository : BaseRepository<Answer>, IAnswerRepository
{
    public AnswerRepository(ApplicationDbContext context) : base(context)
    {
    }

}