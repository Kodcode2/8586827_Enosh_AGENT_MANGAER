using AgentTargetRest.Models;

namespace AgentTargetRest.Services
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMisionsAsync();
        Task<List<MissionModel>> GetProposeMisionsAsync();
        Task<List<MissionModel>> GetOnTaskMisionsAsync();
        Task<List<MissionModel>> GetEndedMisionsAsync();

        Task<List<MissionModel>> CreateListMissionsFromAgentPinMoveAsync(long agentId);
        Task<List<MissionModel>> CreateListMissionsFromTargetPinMoveAsync(long targetId);
        Task MainMissionFuncAsync(long missionId);
        Task MainUpdate();
        Task Delete();
    }
}
