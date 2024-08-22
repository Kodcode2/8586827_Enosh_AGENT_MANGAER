using AgentTargetRest.Data;
using AgentTargetRest.Dto;
using AgentTargetRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgentTargetRest.Services
{
    public class AgentService : IAgentService
    {
        private readonly ApplicationDbContext _context;

        public AgentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AgentModel>> GetAgentsAsync()
        {
            var a = _context.Agents;
            return await _context.Agents.ToListAsync();
        }

        public async Task<ActionResult<AgentModel>> GetAgentModelAsync(long id)
        {
            var agentModel = await _context.Agents.FindAsync(id);

            if (agentModel == null)
            {
                return null;
            }

            return agentModel;
        }

        public async Task<ActionResult<AgentModel>> UpdateAgentAsync(long id, AgentModel agentModel)
        {
            if (!_context.Agents.Any(a => a.Id == id))
            {
                return null;
            }
            try
            {
                var agent = await _context.Agents.FirstOrDefaultAsync(a => a.Id == id);
                agent!.Image = agentModel.Image;
                agent.NickName = agentModel.NickName;
                await _context.SaveChangesAsync();
                return agent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult<AgentModel>> PostAgentModel(AgentDto agentDto)
        {
            try
            {
                AgentModel agent = new()
                {
                    Image = agentDto.Photo_Url,
                    NickName = agentDto.Name,
                };
                await _context.Agents.AddAsync(agent);
                await _context.SaveChangesAsync();
                return agent;
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult<AgentModel>> DeleteAgentModelAsync(long id)
        {
            var agentModel = await _context.Agents.FindAsync(id);
            if (agentModel == null)
            {
                return null;
            }

            _context.Agents.Remove(agentModel);
            await _context.SaveChangesAsync();

            return agentModel;
        }

        private bool AgentModelExists(long id)
        {
            return _context.Agents.Any(e => e.Id == id);
        }
    }
}
