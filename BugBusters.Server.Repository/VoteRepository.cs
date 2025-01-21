using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Repositories;
using BugBusters.Server.Repository.Base;
using BugBusters.Server.Repository.DatabaseContext;

namespace BugBusters.Server.Repository;
public class VoteRepository : BaseRepository<Vote>, IVoteRepository
{
    public VoteRepository(ApplicationDbContext context) : base(context)
    {
    }
}