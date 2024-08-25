using ClientAgentTarget.Models;

namespace ClientAgentTarget.Services
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMissions();
    }
}
