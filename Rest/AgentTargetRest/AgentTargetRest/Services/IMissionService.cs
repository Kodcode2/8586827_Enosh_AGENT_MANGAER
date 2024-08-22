using AgentTargetRest.Models;

namespace AgentTargetRest.Services
{
    public interface IMissionService
    {
        Task<List<MissionModel>> IsAgentHasMission(long id);
    }
}
