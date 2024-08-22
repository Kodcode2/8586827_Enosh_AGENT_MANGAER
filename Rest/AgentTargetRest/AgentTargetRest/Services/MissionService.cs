using AgentTargetRest.Data;
using AgentTargetRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentTargetRest.Services
{
    public class MissionService(ApplicationDbContext context, IAgentService agentService, ITargetService targetService) : IMissionService
    {
        public async Task<List<MissionModel>> IsAgentHasMission(long id)
        {
            if (await agentService.IsAgentFree(id))
            {
                var agent = await agentService.FindAgentById(id);

                var mission1 = await context.Targets.Where(t => Distance(agent, t) < 200).ToListAsync();
                var mission2 =  mission1.Where(t => targetService.IsTargetValid(t.Id)).ToList();
                List<MissionModel> missions = [mission2.Select(t => new MissionModel
                 {
                     AgentId = agent.Id,
                     TargetId = t.Id,

                 }
                     ];
            }
            return new List<MissionModel>();
        }

        public double Distance(AgentModel agent, TargetModel target) =>
            Math.Sqrt(Math.Pow(target.X - agent.X, 2)
                + Math.Pow(target.Y - agent.Y, 2));


        public Task<List<MissionModel>> IsAgentHasMission(long id)
        {
            throw new NotImplementedException();
        }
    }
}
