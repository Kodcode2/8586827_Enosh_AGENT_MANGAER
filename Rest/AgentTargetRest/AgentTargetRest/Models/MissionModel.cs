using System.ComponentModel.DataAnnotations;

namespace AgentTargetRest.Models
{
    public class MissionModel
    {
        public enum Status
        {
            KillPropose,
            OnTask,
            MissionEnded
        }
        public long Id { get; set; }

        [Required]
        public long TargetId { get; set; }
        public TargetModel? Target { get; set; }

        [Required]
        public long AgentId { get; set; }
        public AgentModel? Agent { get; set; }
        public double TimeLeft { get; set; }
        public double ExecutionTime { get; set; }
        public Status MissionStatus { get; set; } = Status.KillPropose;
    }
}
