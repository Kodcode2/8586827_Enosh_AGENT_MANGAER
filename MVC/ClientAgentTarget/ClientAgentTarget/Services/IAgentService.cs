using ClientAgentTarget.Models;
using ClientAgentTarget.ViewModel;

namespace ClientAgentTarget.Services
{
    public interface IAgentService
    {
        Task<List<AgentModel>> GetAllAgents();
        Task<List<StatusAgentsVM>> CreateAgentsVM();
        Task<StatusAgentsVM> Details(long agentId);
    }
}
