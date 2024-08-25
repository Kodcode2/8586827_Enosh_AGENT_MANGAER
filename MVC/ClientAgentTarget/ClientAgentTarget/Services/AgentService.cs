using ClientAgentTarget.Models;
using System.Text.Json;

namespace ClientAgentTarget.Services
{
    public class AgentService(IHttpClientFactory clientFactory) : IAgentService
    {
        private readonly string baseUrl = "https://localhost:7201/Agents";
        public async Task<List<AgentModel>> GetAllAgents()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authentication.Token);

            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content1 = await result.Content.ReadAsStringAsync();
                List<AgentModel?> agents = JsonSerializer.Deserialize<List<AgentModel>>(
                    content1, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })
                    ;
                return agents;
            }
            return null;
        }
    }
}
