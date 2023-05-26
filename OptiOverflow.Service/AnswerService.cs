using AutoMapper;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Service;

public class AnswerService : IAnswerService
{
    private readonly IMapper _mapper;

    public AnswerService(IMapper mapper)
    {
        _mapper = mapper;
    }
}