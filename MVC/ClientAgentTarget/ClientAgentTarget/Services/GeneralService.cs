using ClientAgentTarget.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;


namespace ClientAgentTarget.Services
{
    public class GeneralService(IAgentService agentService, ITargetService targetService, IMissionService missionService, IHttpClientFactory clientFactory) :IGeneralService
    {
        private readonly string baseUrl = "https://localhost:7201/Missions/";
        public async Task<GeneralVM> CreateGeneralTable()
        {
            var agents = await agentService.GetAllAgents();
            var Missions = await missionService.GetAllMissions();
            var targets = await targetService.GetAllTargets();
            GeneralVM general = new()
            {
                Agents = agents.Count(),
                ActiveAgents = agents.Where(a => a.AgentStatus == Models.AgentStatus.Active).Count(),
                Targets = targets.Count(),
                EliminatedTargets = targets.Where(t => t.TargetStatus == Models.TargetStatus.Dead).Count(),
                Missions = Missions.Count(),
                ActiveMissions = Missions.Where(m => m.MissionStatus == Models.MissionStatus.OnTask).Count(),
                AgentsVsTargets = agents.Count() / targets.Count(),
                AgentsOnPropose = Missions.Select(m => m.AgentId).ToHashSet().Count(),
            };
            return general;
        }

        public async Task<List<AssignedVM>> CreateAssigned()
        {
            var agents = await agentService.GetAllAgents();
            var missions = await missionService.GetAllMissions();
            var targets = await targetService.GetAllTargets();
            List<string> status = ["Propose", "On task", "Killed"];

            return (from mission in missions
                       join agent in agents on mission.AgentId equals agent.Id
                       join target in targets on mission.TargetId equals target.Id
                       select (mission, agent, target))
                       .Select(all =>
                       {
                           var (mission, agent, target) = all;
                           return new AssignedVM()
                           {
                               MissionId = mission.Id,
                               AgentName = agent.NickName,
                               AgentLocationX = agent.X,
                               AgentLocationY = agent.Y,
                               TargetName = target.Name,
                               TargetLocationX = target.X,
                               TargetLocationY = target.Y,
                               TargetImage = target.Image,
                               Role = target.Role,
                               Distance = Math.Sqrt(
                                   Math.Pow(target.X - agent.X, 2) +
                                   Math.Pow(target.Y - agent.Y, 2)
                               ),
                               TimeUntillEliminated = mission.TimeLeft,
                               MissionSituation = mission.MissionStatus.ToString(),
                           };
                       }).ToList();
        }

        public async Task<bool> ConfirmedAssinged(long MissionId)
        {
            var missions = await missionService.GetAllMissions();
            var mission = missions.FirstOrDefault(m => m.Id == MissionId);
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}{MissionId}");
            // request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authentication.Token);
            /*var httpContent = new StringContent(
                JsonSerializer.Serialize(mission, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }),
                Encoding.UTF8,
                "application/json"
            );*/
           // request.Content = httpContent;
            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<AssignedVM> Details(long missionId)
        {
            var missions = await CreateAssigned();
            return missions.Where(m => m.MissionId == missionId).First();
        }
    }
}
