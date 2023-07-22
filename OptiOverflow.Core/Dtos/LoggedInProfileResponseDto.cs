namespace OptiOverflow.Core.Dtos;

public class LoggedInProfileResponseDto: UserResponseDto
{
    public long QuestionAsked { get; set; }
    public long Answered { get; set;}
    public long UpVoteCount { get; set; }
    public long DownVoteCount { get; set; }
}