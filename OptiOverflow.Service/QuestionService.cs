using AutoMapper;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Service;

public class QuestionService: IQuestionService
{
    private readonly IMapper _mapper;

    public QuestionService(IMapper mapper)
    {
        _mapper = mapper;
    }
}