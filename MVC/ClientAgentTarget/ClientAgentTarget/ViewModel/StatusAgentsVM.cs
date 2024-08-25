using ClientAgentTarget.Models;

namespace ClientAgentTarget.ViewModel
{
    public class StatusAgentsVM
    {
        public string AgentName { get; set; }
        public int X {  get; set; }
        public int Y {  get; set; }
        public string StatusAgents {  get; set; }
        public MissionModel Mission { get; set; }
        public double TimeLeft { get; set; }
        public int EliminatedAmount { get; set; }
    }
}
