using AgentTargetRest.Data;
using AgentTargetRest.Dto;
using AgentTargetRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgentTargetRest.Services
{
    public class AgentService(IServiceProvider serviceProvider, ApplicationDbContext context) : IAgentService
    {
        private IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();
        private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();

        private readonly Dictionary<string, (int, int)> Direction = new()
        {
            {"n", (0, 1)},
            {"s", (0, -1)},
            {"e", (1, 0)},
            {"w", (-1, 0)},
            {"ne", (1, 1)},
            {"nw", (-1, 1)},
            {"se", (1, -1)},
            {"sw", (-1, -1)}
        };
        public async Task<List<AgentModel>> GetAgentsAsync()
        {
            return await context.Agents.ToListAsync();
        }

        public async Task<ActionResult<AgentModel>> GetAgentModelAsync(long id)
        {
            var agentModel = await context.Agents.FindAsync(id);

            if (agentModel == null)
            {
                return null;
            }

            return agentModel;
        }

        public async Task<ActionResult<AgentModel>> UpdateAgentAsync(long id, AgentModel agentModel)
        {
            if (!context.Agents.Any(a => a.Id == id))
            {
                return null;
            }
            try
            {
                var agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == id);
                agent!.Image = agentModel.Image;
                agent.NickName = agentModel.NickName;
                await context.SaveChangesAsync();
                return agent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult<AgentModel>> DeleteAgentModelAsync(long id)
        {
            var agentModel = await context.Agents.FindAsync(id);
            if (agentModel == null)
            {
                return null;
            }

            context.Agents.Remove(agentModel);
            await context.SaveChangesAsync();

            return agentModel;
        }

        private bool AgentModelExists(long id)
        {
            return context.Agents.Any(e => e.Id == id);
        }

        public async Task<AgentModel> PinAsync(PinDto pin, long id)
        {
            AgentModel? agent = await context.Agents.FirstOrDefaultAsync(t => t.Id == id);
            if (agent == null){ throw new Exception("Target not found"); }

            agent.X = pin.X;
            agent.Y = pin.Y;
            await context.SaveChangesAsync();
            await missionService.CreateListMissionsFromAgentPinMoveAsync(id);
            return agent;
        }

        public async Task<AgentModel> MoveAgentAsync(long id, DirectionsDto directionDto)
        {
            AgentModel? agent = await context.Agents.FirstOrDefaultAsync(t => t.Id == id);
            if (agent == null) { throw new Exception("Target not found"); }

            var (x, y) = Direction[directionDto.Direction];
            agent.X += x;
            agent.Y += y;
            if (agent.X < 0 || agent.X > 1000 || agent.Y < 0 || agent.Y > 1000)
            {
                throw new Exception($"Out of range, the agent is in: ({agent.X},{agent.Y})");
            }
            await context.SaveChangesAsync();
            await missionService.CreateListMissionsFromAgentPinMoveAsync(id);
            return agent;
        }
  
        public async Task<IdDto> CreateAgentModelAsync(AgentDto agentDto)
        {
            try
            {
                AgentModel agent = new()
                {
                    Image = agentDto.PhotoUrl,
                    NickName = agentDto.NickName,
                };
                await context.Agents.AddAsync(agent);
                await context.SaveChangesAsync();
                var a = await context.Agents.FirstOrDefaultAsync(A => A.Image == agent.Image && A.NickName == agent.NickName);
                IdDto idDto = new() { Id = a.Id };
                return idDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // function check if the agent are avalible for missions
        public async Task<bool> IsAgentValidAsync(long id)
        {
            var agent = await FindAgentByIdAsync(id);
            if (agent.AgentStatus == AgentStatus.Dormant)
            {
                return true;
            }
            return false;
        }

        public bool IsAgentValid(AgentModel agent)
        {
            if (agent.AgentStatus == AgentStatus.Dormant)
            {
                return true;
            }
            return false;
        }

        // function find the agent by id and return the agent
        public async Task<AgentModel> FindAgentByIdAsync(long id)
        {
            AgentModel? agent = await context.Agents.FirstOrDefaultAsync(t => t.Id == id);
            if (agent == null)
            {
                throw new Exception("Target not found");
            }
            return agent;
        }    
    }
}
