using ClientAgentTarget.ViewModel;
using System.Drawing.Drawing2D;

namespace ClientAgentTarget.Services
{
    public class MatrixService(IAgentService agentService, ITargetService targetService) : IMatrixService
    {
/*        private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();
        private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();
        private readonly IServiceProvider serviceProvider;*/

        public async Task<MatrixVM> InitMatrix()
        {
            try
            {
                var agents = await agentService.GetAllAgents();
                var targets = await targetService.GetAllTargets();
                if (!agents.Any() && !targets.Any())
                {
                    return new MatrixVM(1, 1);
                }
                // Determine the matrix size
                int maxX = Math.Max(agents.Max(a => a.X), targets.Max(t => t.X)) + 1;
                int maxY = Math.Max(agents.Max(a => a.Y), targets.Max(t => t.Y)) + 1;
                var model = new MatrixVM(maxX, maxY);
                // Place agents in the matrix
                foreach (var agent in agents)
                {
                    if (agent.X >= 0 && agent.X < maxX && agent.Y >= 0 && agent.Y < maxY)
                    {
                        model.Matrix[agent.X, agent.Y] += $"{agent.NickName}";
                    }
                }
                // Place targets in the matrix
                foreach (var target in targets)
                {
                    if (target.X >= 0 && target.X < maxX && target.Y >= 0 && target.Y < maxY)
                    {
                        model.Matrix[target.X, target.Y] += $"{target.Name}";
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

