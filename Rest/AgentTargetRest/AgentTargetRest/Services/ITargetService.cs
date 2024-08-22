using AgentTargetRest.Dto;
using AgentTargetRest.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgentTargetRest.Services
{
    public interface ITargetService
    {
        Task<List<TargetModel>> GetTargetsAsync();
        Task<ActionResult<TargetModel>> GetTargetModelAsync(long id);
        Task<ActionResult<TargetModel>> PostTargetModel(TargetDto targetDto);
        Task<ActionResult<TargetModel>> UpdateTargetAsync(long id, TargetModel targetModel);
        Task<ActionResult<TargetModel>> DeleteTargetModelAsync(long id);
    }
}
