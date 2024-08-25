using ClientAgentTarget.Models;

namespace ClientAgentTarget.Services
{
    public interface ITargetService
    {
        Task<List<TargetModel>> GetAllTargets();
    }
}
