using AgentTargetRest.Dto;
using AgentTargetRest.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgentTargetRest.Services
{
    public interface IAgentService
    {
        Task<List<AgentModel>> GetAgentsAsync();
        Task<ActionResult<AgentModel>> GetAgentModelAsync(long id);
        Task<IdDto> CreateAgentModel(AgentDto agentDto);
        Task<ActionResult<AgentModel>> UpdateAgentAsync(long id, AgentModel agentModel);
        Task<ActionResult<AgentModel>> DeleteAgentModelAsync(long id);
        Task<AgentModel> MoveAgent(long id, DirectionsDto directionDto);
        Task<AgentModel> Pin(PinDto pin, long id);
        Task<bool> IsAgentFree(long id);
        Task<AgentModel> FindAgentById(long id);
    }
}
