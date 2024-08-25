using ClientAgentTarget.Models;

namespace ClientAgentTarget.Services
{
    public interface IAgentService
    {
        Task<List<AgentModel>> GetAllAgents();
    }
}
