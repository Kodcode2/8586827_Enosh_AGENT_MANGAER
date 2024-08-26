using ClientAgentTarget.ViewModel;

namespace ClientAgentTarget.Services
{
    public interface IMatrixService
    {
        Task<MatrixVM> InitMatrix();
    }
}
