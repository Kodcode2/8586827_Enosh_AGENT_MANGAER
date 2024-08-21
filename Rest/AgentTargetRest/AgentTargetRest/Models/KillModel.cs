namespace AgentTargetRest.Models
{
    public class KillModel
    {
        public long Id { get; set; }
        public long AgentId { get; set; }
        public AgentModel Agent { get; set; }
        public long TargetId { get; set; }
        public TargetModel Target { get; set; }
        public DateTime KillTime { get; set; }
    }
}
