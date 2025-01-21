using BugBusters.Server.Core.Dtos;

namespace BugBusters.Server.Core.Interfaces.Services;

public interface IJiraService
{
    Task<JiraTicket?> GetTicketAsync();
}