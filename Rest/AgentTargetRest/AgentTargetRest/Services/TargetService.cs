using AgentTargetRest.Data;
using AgentTargetRest.Dto;
using AgentTargetRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgentTargetRest.Services
{
    public class TargetService : ITargetService
    {
        private readonly ApplicationDbContext _context;

        public async Task<ActionResult<TargetModel >> DeleteTargetModelAsync(long id)
        {
            var target = await _context.Targets.FindAsync(id);
            if (target == null)
            {
                return null;
            }

            _context.Targets.Remove(target);
            await _context.SaveChangesAsync();

            return target;
        }

        public async Task<ActionResult<TargetModel>> GetTargetModelAsync(long id)
        {
            var target = await _context.Targets.FindAsync(id);

            if (target == null)
            {
                return null;
            }

            return target;
        }

        public async Task<List<TargetModel>> GetTargetsAsync()
        {
            return await _context.Targets.ToListAsync();
        }

        public async Task<ActionResult<TargetModel>> PostTargetModel(TargetDto targetDto)
        {
            try
            {
                TargetModel target = new()
                {
                    Image = targetDto.Photo_Url,
                    Name = targetDto.Name,
                    Role = targetDto.Position
                };
                await _context.Targets.AddAsync(target);
                await _context.SaveChangesAsync();
                return target;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult<TargetModel>> UpdateTargetAsync(long id, TargetModel targetModel)
        {

            if (!_context.Targets.Any(a => a.Id == id))
            {
                return null;
            }
            try
            {
                var target = await _context.Targets.FirstOrDefaultAsync(a => a.Id == id);
                target.Name = targetModel.Name;
                target.Role = targetModel.Role;
                target.Image = targetModel.Image;
                await _context.SaveChangesAsync();
                return target;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
