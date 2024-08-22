using AgentTargetRest.Models;

namespace AgentTargetRest.Services
{
    public interface IMissionService
    {
        Task<List<MissionModel>> CreateListMissionsFromAgent(long id);
        Task<MissionModel> CreateMission(MissionModel mission);
    }
}
