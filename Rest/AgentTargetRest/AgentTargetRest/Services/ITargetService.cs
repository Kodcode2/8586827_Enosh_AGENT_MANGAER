using AgentTargetRest.Dto;
using AgentTargetRest.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgentTargetRest.Services
{
    public interface ITargetService
    {
        Task<List<TargetModel>> GetTargetsAsync();
        Task<ActionResult<TargetModel>> GetTargetModelAsync(long id);
        Task<IdDto> CreateTargetModel(TargetDto targetDto);
        Task<ActionResult<TargetModel>> UpdateTargetAsync(long id, TargetModel targetModel);
        Task<ActionResult<TargetModel>> DeleteTargetModelAsync(long id);
        Task<TargetModel> MoveTargetAsync(long id, DirectionsDto directionDto);
        Task<TargetModel> PinAsync(PinDto pin, long id);
        Task<bool> IsTargetValidAsync(TargetModel target);
        Task<TargetModel> FindTargetByIdAsync(long id);
    }
}
