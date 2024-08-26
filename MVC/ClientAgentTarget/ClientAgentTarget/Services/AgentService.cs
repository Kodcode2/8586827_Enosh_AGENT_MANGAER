using ClientAgentTarget.Models;
using ClientAgentTarget.ViewModel;
using System.Text.Json;

namespace ClientAgentTarget.Services
{
    public class AgentService(
        IHttpClientFactory clientFactory, IServiceProvider serviceProvider
        ) : IAgentService
    {

        private  ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();
        private  IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();
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


        public async Task<List<StatusAgentsVM>> CreateAgentsVM()
        {
            var agents = await GetAllAgents();
            var targets = await targetService.GetAllTargets();
            var missions = await missionService.GetAllMissions();

            long? missionId(AgentModel x) => missions
                .Where(m => m.AgentId == x.Id && m.MissionStatus == MissionStatus.OnTask)
                .Select(m => (long?)m.Id) 
                .FirstOrDefault();    

            var statusAgents = agents.Select(x =>
            {
                var currentMissionId = missionId(x);
                var timeToEliminate = missions
                    .Where(m => m.Id == currentMissionId)
                    .Select(m => m.TimeLeft)
                    .FirstOrDefault();

                var eliminatedAmount = missions
                    .Where(m => m.AgentId == x.Id && m.MissionStatus == MissionStatus.MissionEnded)
                    .Count();

                return new StatusAgentsVM
                {
                    Id = x.Id,
                    AgentName = x.NickName,
                    X = x.X,
                    Y = x.Y,
                    StatusAgents = x.AgentStatus.ToString(),
                    MissionId = currentMissionId.GetValueOrDefault(), 
                    TimeToElimanate = timeToEliminate,
                    EliminatedAmount = eliminatedAmount,
                };
            }).ToList();

            return statusAgents;
        }

        public async Task<StatusAgentsVM> Details(long agentId)
        {
            var listAgebtsVM = await CreateAgentsVM();
            return listAgebtsVM.Where(a => a.Id == agentId).FirstOrDefault() ?? new StatusAgentsVM();
        }

    }
}

