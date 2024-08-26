using AgentTargetRest.Data;
using AgentTargetRest.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AgentTargetRest.Services
{
    public class MissionService(ApplicationDbContext context,
       IServiceProvider serviceProvider
    ) : IMissionService
    {
        private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();
        private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();

        // <GET>
        public async Task<List<MissionModel>> GetAllMisionsAsync() =>
            await context.Missions.ToListAsync();
        // <GET>
        public async Task<List<MissionModel>> GetProposeMisionsAsync() =>
            await context.Missions.Where(m => m.MissionStatus == MissionStatus.KillPropose).ToListAsync();
        // <GET>
        public async Task<List<MissionModel>> GetOnTaskMisionsAsync() =>
            await context.Missions.Where(m => m.MissionStatus == MissionStatus.OnTask).ToListAsync();
        // <GET>
        public async Task<List<MissionModel>> GetEndedMisionsAsync() =>
            await context.Missions.Where(m => m.MissionStatus == MissionStatus.MissionEnded).ToListAsync();


        // Check if this function is right!!!!
        // Get list of targets and filter if valid
        // <FOR PIN/MOVE>
        public async Task<List<TargetModel>> FilterValidTargetsAsync(List<TargetModel> validTargets1)
        {
            var validTargets2 = new List<TargetModel>();

            var tasks = validTargets1.Select(async t =>
            {
                var isValid = await targetService.IsTargetValidAsync(t);
                return new { Target = t, IsValid = isValid };
            });

            var results = await Task.WhenAll(tasks);
            validTargets2 = results.Where(r => r.IsValid).Select(r => r.Target).ToList();

            return validTargets2;
        }


        // If agent move/pin this function will check if thete is any target around
        // <FOR PIN/MOVE>
        public async Task<List<MissionModel>> CreateListMissionsFromAgentPinMoveAsync(long agentId)
        {
            if (await agentService.IsAgentValidAsync(agentId))
            {
                var agent = await agentService.FindAgentByIdAsync(agentId);
                var missions = await context.Missions.ToListAsync();
                var targets = await context.Targets.ToListAsync();
                // filter if the targets are too far
                var validTargetsByDistance = await context.Targets.Where(t => (Math.Sqrt(Math.Pow(t.X - agent.X, 2)
              + Math.Pow(t.Y - agent.Y, 2))) < 200).ToListAsync();
                // filter if there is no other agent on the target
                List<TargetModel> targetsWithoutMissions = targets.Where(t => !missions.Exists(m => m.TargetId == t.Id)).ToList();
                // join missions to targets by distance
                // filter only if is kill propose and agent id is not on mission id
                // and taget by distance (id) not on mission tagertId
                List<TargetModel> validTargets = [
                    ..targetsWithoutMissions,
                    ..(from m in missions
                    join t in validTargetsByDistance on m.TargetId equals t.Id
                    where m.MissionStatus == MissionStatus.KillPropose &&
                    m.AgentId != agentId &&
                    m.TargetId != t.Id
                    select t)
                .ToList()
                ];

                // var validTargets2 = await FilterValidTargetsAsync(validTargetsByDistance);
                // create list of missions after the filters
                List<MissionModel> createMissions = validTargets.Select(t => new MissionModel
                {
                    AgentId = agent.Id,
                    TargetId = t.Id,
                    TimeLeft = Distance(agent, t) / 5,
                })
                    .Where(m => !missions.Exists(em => em.TargetId != m.TargetId && em.AgentId != m.AgentId))
                    .ToList();
            
                await context.Missions.AddRangeAsync(createMissions);
                await context.SaveChangesAsync();
                return missions;
            }
            throw new Exception("The agent on task in another mission");
        }

        // If target move/pin this function will check if thete is any agent around
        // <FOR PIN/MOVE>
        public async Task<List<MissionModel>> CreateListMissionsFromTargetPinMoveAsync(long targetId)
        {
            var target = await targetService.FindTargetByIdAsync(targetId);
            // Check if there is no other agent on the target
            if (context.Missions.Any(m => m.TargetId == targetId && m.MissionStatus == MissionStatus.KillPropose) || !context.Missions.Any(m => m.TargetId == targetId))
            {
                if (target.TargetStatus == TargetStatus.Alive)
                {
                    // Filter the agent if their are too far
                    var validAgents1 = await context.Agents.Where(a => (Math.Sqrt(Math.Pow(target.X - a.X, 2)
                    + Math.Pow(target.Y - a.Y, 2))) < 200).ToListAsync();
                    var validAgents2 = validAgents1.Where(agentService.IsAgentValid);
                    // create list of missions after the filters
                    var missions = await context.Missions.ToListAsync();
                    List<MissionModel> missionsToSave = validAgents2
                        .Select(a => new MissionModel
                        {
                            AgentId = a.Id,
                            TargetId = targetId,
                            TimeLeft = Distance(a, target) / 5,
                        })
                    .Where(m => !missions.Exists(em => em.TargetId != m.TargetId && em.AgentId != m.AgentId))
                    .ToList();
                    context.Missions.AddRange(missions);
                    context.SaveChanges();
                    return missionsToSave;
                }
                throw new Exception("There is already agent on this target");
            }
            throw new Exception("The target is is dead:)");
        }

        // <FOR USER CREATE>
        public bool IsStatusesAvalableToAssinge
            (MissionModel mission, TargetModel target, AgentModel agent)
        {
            if (
                 mission.MissionStatus == MissionStatus.KillPropose ||
                  target.TargetStatus == TargetStatus.Alive ||
                  agent.AgentStatus == AgentStatus.Dormant
              ) return true; return false;
        }

        // <FOR USER CREATE>
        public async Task ChangeStatusesAfterAssingeAsync
            (MissionModel mission, TargetModel target, AgentModel agent)
        {
            await CalculateTimeUntilAssassinAndMoveTheAgentAsync(agent, target!, mission);
            mission.MissionStatus = MissionStatus.OnTask;
            agent.AgentStatus = AgentStatus.Active;
            mission.Target = target;
            mission.Agent = agent;
            var distance = Distance(agent, target);
            mission.TimeLeft = distance / 5;

            await context.SaveChangesAsync();
        }

        // <FOR USER CREATE>
        public void DeleteMission(MissionModel mission)
        {
            context.Missions.Remove(mission);
        }

        private bool RmoveableMissions(MissionModel mission, AgentModel agent, TargetModel target) => (
            mission.MissionStatus == MissionStatus.KillPropose
            && mission.TargetId == target.Id || mission.AgentId == agent.Id
        );


        // <FOR USER CREATE>
        public async Task<MissionModel> FilterMissionsAndDeleteAfterAssigned
        (MissionModel mission, AgentModel agent, TargetModel target)
        {
            try
            {
                if (mission.MissionStatus == MissionStatus.KillPropose
                    && mission.TargetId == target.Id || mission.AgentId == agent.Id)
                {
                    DeleteMission(mission);
                    await context.SaveChangesAsync();
                }
                return mission;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Update mission after the mission assigned  
        // <FOR USER CREATE>
        public async Task UpdateMissionUserAsync(MissionModel mission, AgentModel agent, TargetModel target)
        {
            var distance = Distance(agent, target);

            if (distance > 200)
            {
                context.Missions.Remove(mission);
                throw new Exception("The target went far");
            }
            await ChangeStatusesAfterAssingeAsync(mission, target, agent);
            var missions = await context.Missions.Where(m => m.MissionStatus == 0).ToListAsync();

            var removeableMissions = missions.Where(m => RmoveableMissions(m, agent, target)).ToList();
            context.Missions.RemoveRange(removeableMissions);
            await context.SaveChangesAsync();

            await CalculateTimeUntilAssassinAndMoveTheAgentAsync(agent, target, mission);

            // !!! throw new Exception("One or more of the models status is not avalible");
        }

        // <FOR USER CREATE && SIMULATION UPDATE>
        public double Distance(AgentModel agent, TargetModel target) =>
          Math.Sqrt(Math.Pow(target.X - agent.X, 2)
              + Math.Pow(target.Y - agent.Y, 2));

        // <FOR USER CREATE && SIMULATION UPDATE>
        public (int x, int y) MoveAgentAfterTarget1(AgentModel agent, TargetModel target)
        {
            (int x, int y) agentLocation = (agent.X, agent.Y);

            if (agentLocation.x < target.X) agentLocation.x++;
            else if (agentLocation.x > target.X) agentLocation.x--;

            if (agentLocation.y < target.Y) agentLocation.y++;
            else if (agentLocation.y > target.Y) agentLocation.y--;

            return (agentLocation.x == agent.X && agentLocation.y == agent.Y) ? (-10, -10) : agentLocation;
        }

        // <FOR USER CREATE && SIMULATION UPDATE>
        public async Task CalculateTimeUntilAssassinAndMoveTheAgentAsync
            (AgentModel agent, TargetModel target, MissionModel mission)
        {
            (int x, int y) agentLocation = MoveAgentAfterTarget1(agent!, target!);
            if (agentLocation.x == -10 && agentLocation.y == -10)
            {

                AfterTheMissionSuccess(mission, agent, target);
                return;
            }
            agent!.X = agentLocation.x;
            agent.Y = agentLocation.y;
            var distance = Distance(agent, target);
            mission.TimeLeft = distance / 5;
            await context.SaveChangesAsync();
        }

        // <THE END>
        public void AfterTheMissionSuccess(MissionModel mission, AgentModel agent, TargetModel target)
        {
            mission.MissionStatus = MissionStatus.MissionEnded;
            agent.AgentStatus = AgentStatus.Dormant;
            target.TargetStatus = TargetStatus.Dead;
            // DateTime dateTime = DateTime.Now;

            // mission.ExecutionTime = 14.5;
            context.SaveChanges();
        }

        // <MAIN FOR ALL>
        public async Task MainMissionFuncAsync(long missionId)
        {
            var mission = await context.Missions.FirstOrDefaultAsync(m => m.Id == missionId);
            if (mission != null)
            {
                var target = await context.Targets.FirstOrDefaultAsync(t => t.Id == mission.TargetId);
                var agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == mission.AgentId);
                if (agent == null || target == null) { throw new Exception("Problem with agent or the target"); }
                if (IsStatusesAvalableToAssinge(mission, target, agent))
                {
                    await UpdateMissionUserAsync(mission, agent, target);
                    return;
                }
            }
        }

        public async Task MainUpdate()
        {
            var missions = await context.Missions
                .Include(m => m.Agent)
                .Include(m => m.Target)
                .Where(m => m.MissionStatus == MissionStatus.OnTask)
                .ToListAsync();
            // this is a side effect
            var tasks = missions.Select(async (m) => await CalculateTimeUntilAssassinAndMoveTheAgentAsync(m.Agent!, m.Target!, m)).ToArray();
            Task.WaitAll(tasks);
        }

        public async Task Delete()
        {
            var z = context.Missions.Select(x => x);
            context.Missions.RemoveRange(z);
            await context.SaveChangesAsync();
        }
    }
}

// 1. move/pin => CreateListMissions
// 2. create by user => create mission and delete the rest
// 3. update => move untill the target is eliminated check succssesfully
