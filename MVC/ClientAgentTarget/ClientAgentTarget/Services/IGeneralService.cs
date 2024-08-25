using ClientAgentTarget.ViewModel;
namespace ClientAgentTarget.Services
{
    public interface IGeneralService
    {
        Task<GeneralVM> CreateGeneralTable();
    }
}
