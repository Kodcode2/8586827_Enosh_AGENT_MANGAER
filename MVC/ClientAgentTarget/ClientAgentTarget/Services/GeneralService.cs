using ClientAgentTarget.ViewModel;

namespace ClientAgentTarget.Services
{
    public class GeneralService(IAgentService agentService, ITargetService targetService, IMissionService missionService) :IGeneralService
    {
        public async Task<GeneralVM> CreateGeneralTable()
        {
            var agents = await agentService.GetAllAgents();
            var Missions = await missionService.GetAllMissions();
            var targets = await targetService.GetAllTargets();
            var a = Missions.Select(m => m.AgentId).ToHashSet();
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
    }
}
