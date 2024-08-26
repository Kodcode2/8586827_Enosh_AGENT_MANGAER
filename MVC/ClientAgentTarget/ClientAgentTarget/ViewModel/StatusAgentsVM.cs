using ClientAgentTarget.Models;

namespace ClientAgentTarget.ViewModel
{
    public class StatusAgentsVM
    {
        public long Id { get; set; }
        public string AgentName { get; set; }
        public int X {  get; set; }
        public int Y {  get; set; }
        public string StatusAgents {  get; set; }
        public long MissionId { get; set; }
        public double TimeToElimanate { get; set; }
        public int EliminatedAmount { get; set; }
    }
}
