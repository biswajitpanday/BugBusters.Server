using OptiOverflow.Core.Dtos;

namespace OptiOverflow.Core.Interfaces.Services;

public interface IJiraService
{
    Task<JiraTicket?> GetTicketAsync();
}