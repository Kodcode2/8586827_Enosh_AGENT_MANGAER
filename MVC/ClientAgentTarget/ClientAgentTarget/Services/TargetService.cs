using ClientAgentTarget.Models;
using ClientAgentTarget.ViewModel;
using System.Data;
using System.Text.Json;

namespace ClientAgentTarget.Services
{
    public class TargetService(IHttpClientFactory clientFactory, IServiceProvider serviceProvider
        ) : ITargetService
    {

        private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();
        private IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();
    
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
                List<TargetModel> targets = JsonSerializer.Deserialize<List<TargetModel>>(
                    content1, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? [];
                return targets;
            }
            return null;
        }

        public async Task<List<StatusTargetsVM>> CreateTargetVM()
        {
            var targets = await GetAllTargets();
            var agents = await agentService.GetAllAgents();
            var missions = await missionService.GetAllMissions();

            long? missionId(TargetModel x) => missions
                .Where(m => m.TargetId == x.Id )
                .Select(m => (long?)m.Id)
                .FirstOrDefault();

            var statusTargets = targets.Select(x =>
            {
                var currentMissionId = missionId(x);
                var timeToEliminate = missions
                    .Where(m => m.Id == currentMissionId)
                    .Select(m => m.TimeLeft)
                    .FirstOrDefault();

                var eliminatedAmount = missions
                    .Where(m => m.AgentId == x.Id && m.MissionStatus == MissionStatus.MissionEnded)
                    .Count();

                bool IsHeOnTheWayToHell1 = missions.Any(m => m.TargetId == x.Id && m.MissionStatus != MissionStatus.KillPropose);
                return new StatusTargetsVM
                {
                    Id = x.Id,
                    TargetName = x.Name,
                    Image = x.Image,
                    Role = x.Role,
                    X = x.X,
                    Y = x.Y,
                    TargetStatus = x.TargetStatus.ToString(),
                    IsHeOnTheWayToHell = IsHeOnTheWayToHell1,
                    TimeToElimanate = timeToEliminate,
                 };
            }).ToList();

            return statusTargets;
        }

        public async Task<StatusTargetsVM> Details(long targetId)
        {
            var listTargetsVM = await CreateTargetVM();
            return listTargetsVM.Where(a => a.Id == targetId).FirstOrDefault() ?? new StatusTargetsVM();
        }

    }
}
