using BugBusters.Server.Core.Entities;
using DotNetCore.Repositories;

namespace BugBusters.Server.Core.Interfaces.Repositories;

public interface IVoteRepository : IBaseRepository<Vote>
{
}