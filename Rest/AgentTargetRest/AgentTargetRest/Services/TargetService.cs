using AgentTargetRest.Data;
using AgentTargetRest.Dto;
using AgentTargetRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgentTargetRest.Services
{
    public class TargetService(ApplicationDbContext context) : ITargetService
    {
        private readonly Dictionary<string, (int, int)> Direction = new()
        {
            {"n", (0, 1)},
            {"s", (0, -1)},
            {"e", (-1, 0)},
            {"w", (1, 0)},
            {"ne", (-1, 1)},
            {"nw", (1, 1)},
            {"se", (-1, -1)},
            {"sw", (1, -1)}
        };
        public async Task<ActionResult<TargetModel>> DeleteTargetModelAsync(long id)
        {
            var target = await context.Targets.FindAsync(id);
            if (target == null)
            {
                return null;
            }

            context.Targets.Remove(target);
            await context.SaveChangesAsync();

            return target;
        }

        public async Task<ActionResult<TargetModel>> GetTargetModelAsync(long id)
        {
            var target = await context.Targets.FindAsync(id);

            if (target == null)
            {
                return null;
            }

            return target;
        }

        public async Task<List<TargetModel>> GetTargetsAsync()
        {
            return await context.Targets.ToListAsync();
        }

        public async Task<IdDto> CreateTargetModel(TargetDto targetDto)
        {
            try
            {
                TargetModel target = new()
                {
                    Image = targetDto.Photo_Url,
                    Name = targetDto.Name,
                    Role = targetDto.Position
                };
                await context.Targets.AddAsync(target);
                await context.SaveChangesAsync();
                var a = await context.Targets.FirstOrDefaultAsync(A => A.Image == target.Image && A.Name == target.Name && A.Role == target.Role);
                IdDto idDto = new() { Id = a.Id };
                return idDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult<TargetModel>> UpdateTargetAsync(long id, TargetModel targetModel)
        {

            if (!context.Targets.Any(a => a.Id == id))
            {
                return null;
            }
            try
            {
                var target = await context.Targets.FirstOrDefaultAsync(a => a.Id == id);
                target.Name = targetModel.Name;
                target.Role = targetModel.Role;
                target.Image = targetModel.Image;
                await context.SaveChangesAsync();
                return target;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TargetModel> MoveTarget(long id, DirectionsDto directionDto)
        {
            TargetModel? target = await context.Targets.FirstOrDefaultAsync(t => t.Id == id);
            if (target == null)
            {
                throw new Exception("Target not found");
            }
            var (x, y) = Direction[directionDto.Direction];
            target.X += x;
            target.Y += y;
            if(target.X < 0 || target.X > 1000 || target.Y < 0 || target.Y > 1000)
            {
                throw new Exception($"Range over, the target is in: ({target.X},{target.Y})");
            }
            await context.SaveChangesAsync();
            return target;
        }

        public async Task<TargetModel> Pin(PinDto pin, long id)
        {
            TargetModel? target = await context.Targets.FirstOrDefaultAsync(t => t.Id == id);
            if (target == null)
            {
                throw new Exception("Target not found");
            }
            target.X = pin.X;
            target.Y = pin.Y;
            await context.SaveChangesAsync();
            return target;
        }

        public async bool IsTargetValid(long id)
        {
            if (context.Targets.Any(t => t.Id == id))
            {
                var a = await context.Missions.Where(m => m.MissionStatus == 0).ToListAsync();
                var b = a.Select(a => a.TargetId).ToList();
                if (b.Contains(id))
                { return true; }
            }
            return false;
        }
    }
}
