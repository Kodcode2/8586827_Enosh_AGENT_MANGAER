using ClientAgentTarget.Models;
using ClientAgentTarget.ViewModel;

namespace ClientAgentTarget.Services
{
    public interface ITargetService
    {
        Task<List<TargetModel>> GetAllTargets();
        Task<List<StatusTargetsVM>> CreateTargetVM();
        Task<StatusTargetsVM> Details(long targetId);
    }
}
