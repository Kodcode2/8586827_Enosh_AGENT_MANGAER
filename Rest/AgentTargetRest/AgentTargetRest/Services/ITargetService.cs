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
        Task<TargetModel> MoveTarget(long id, DirectionsDto directionDto);
        Task<TargetModel> Pin(PinDto pin, long id);
        bool IsTargetValid(long id);
    }
}
