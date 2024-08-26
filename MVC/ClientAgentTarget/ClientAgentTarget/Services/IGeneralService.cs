using ClientAgentTarget.ViewModel;
namespace ClientAgentTarget.Services
{
    public interface IGeneralService
    {
        Task<GeneralVM> CreateGeneralTable();
        Task<List<AssignedVM>> CreateAssigned();
        Task<bool> ConfirmedAssinged(long MissionId);
        Task<AssignedVM> Details(long missionId);
    }
}
