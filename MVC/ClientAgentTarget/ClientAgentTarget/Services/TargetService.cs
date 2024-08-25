using ClientAgentTarget.Models;
using System.Text.Json;

namespace ClientAgentTarget.Services
{
    public class TargetService(IHttpClientFactory clientFactory) : ITargetService
    {
        private readonly string baseUrl = "https://localhost:7201/Targets";

        public async Task<List<TargetModel>> GetAllTargets()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authentication.Token);

            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content1 = await result.Content.ReadAsStringAsync();
                List<TargetModel?> targets = JsonSerializer.Deserialize<List<TargetModel>>(
                    content1, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return targets;
            }
            return null;
        }
    }
}
