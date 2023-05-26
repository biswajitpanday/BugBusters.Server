using AutoMapper;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Service;

public class VoteService : IVoteService
{
    private readonly IMapper _mapper;

    public VoteService(IMapper mapper)
    {
        _mapper = mapper;
    }
}