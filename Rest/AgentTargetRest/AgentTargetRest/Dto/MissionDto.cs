using AgentTargetRest.Models;

namespace AgentTargetRest.Dto
{
    public class MissionDto
    {
        public enum Status
        {
            KillPropose,
            OnTask,
            MissionEnded
        }

        public long TargetId { get; set; }
        public long AgentId { get; set; }
        public double TimeLeft { get; set; }
        public double ExecutionTime { get; set; }
        public Status MissionStatus { get; set; } = Status.KillPropose;
    }
}
