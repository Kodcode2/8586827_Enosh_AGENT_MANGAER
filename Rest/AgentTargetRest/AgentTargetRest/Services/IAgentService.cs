using AgentTargetRest.Dto;
using AgentTargetRest.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgentTargetRest.Services
{
    public interface IAgentService
    {
        Task<List<AgentModel>> GetAgentsAsync();
        Task<ActionResult<AgentModel>> GetAgentModelAsync(long id);
        Task<IdDto> CreateAgentModelAsync(AgentDto agentDto);
        Task<ActionResult<AgentModel>> UpdateAgentAsync(long id, AgentModel agentModel);
        Task<ActionResult<AgentModel>> DeleteAgentModelAsync(long id);
        Task<AgentModel> MoveAgentAsync(long id, DirectionsDto directionDto);
        Task<AgentModel> PinAsync(PinDto pin, long id);
        Task<bool> IsAgentValidAsync(long id);
        Task<AgentModel> FindAgentByIdAsync(long id);
        bool IsAgentValid(AgentModel agent);
    }
}
