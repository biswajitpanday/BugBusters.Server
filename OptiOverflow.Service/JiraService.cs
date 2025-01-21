using Newtonsoft.Json;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Interfaces.Services;
using Polly;
using System.Net.Http.Headers;

namespace OptiOverflow.Service;

public class JiraService : IJiraService
{
    public async Task<JiraTicket?> GetTicketAsync()
    {
        var retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(2));

        var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get,
            "https://insitesoft.atlassian.net/rest/api/3/search?jql=project%20%3D%20<ProjectNameHere>&ORDER%20BY=Created&maxResults=1000&startAt=0");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "<example@email.com:AuthTokenHere>");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Cookie", "atlassian.xsrf.token=<xsrfTokenHere>");

        try
        {
            var response = await retryPolicy.Execute(() =>
                httpClient.GetAsync(
                    "https://insitesoft.atlassian.net/rest/api/3/search?jql=project%20%3D%<ProjectNameHere>&ORDER%20BY=Created&maxResults=1000&startAt=0"));

            var responseContent = await response.Content.ReadAsStringAsync();
            var jiraTicket = JsonConvert.DeserializeObject<JiraTicket>(responseContent);
            return jiraTicket;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}