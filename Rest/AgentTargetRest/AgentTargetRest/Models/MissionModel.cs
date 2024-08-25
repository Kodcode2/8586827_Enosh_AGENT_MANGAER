using System.ComponentModel.DataAnnotations;

namespace AgentTargetRest.Models
{
    public enum MissionStatus
    {
        KillPropose,
        OnTask,
        MissionEnded
    }
    public class MissionModel
    {
        public long Id { get; set; }

        [Required]
        public long TargetId { get; set; }
        public TargetModel? Target { get; set; }

        [Required]
        public long AgentId { get; set; }
        public AgentModel? Agent { get; set; }
        public double TimeLeft { get; set; }
        public double ExecutionTime { get; set; }
        public MissionStatus MissionStatus { get; set; } = MissionStatus.KillPropose;
    }
}
