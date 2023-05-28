using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Repositories;
using OptiOverflow.Repository.Base;
using OptiOverflow.Repository.DatabaseContext;

namespace OptiOverflow.Repository;
public class VoteRepository : BaseRepository<Vote>, IVoteRepository
{
    public VoteRepository(ApplicationDbContext context) : base(context)
    {
    }
}