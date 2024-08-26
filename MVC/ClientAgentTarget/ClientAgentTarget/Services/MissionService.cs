using ClientAgentTarget.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ClientAgentTarget.Services
{
    public class MissionService(IHttpClientFactory clientFactory) : IMissionService
    {
        private readonly string baseUrl = "https://localhost:7201/Missions/get-all";
        public async Task<List<MissionModel>> GetAllMissions()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authentication.Token);

            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content1 = await result.Content.ReadAsStringAsync();
                List<MissionModel> missions = JsonSerializer.Deserialize<List<MissionModel>>(
                    content1, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? [];
                return missions;
            }
            return null;
        }

       
    }
}
