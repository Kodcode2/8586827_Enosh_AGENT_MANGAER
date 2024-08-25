namespace ClientAgentTarget.ViewModel
{
    public class GeneralVM
    {
        public int Agents { get; set; }
        public int ActiveAgents { get; set; }
        public int Targets { get; set; }
        public int EliminatedTargets { get; set;}
        public int Missions { get; set; }
        public int ActiveMissions { get; set; }
        public double AgentsVsTargets { get; set; }
        public int AgentsOnPropose { get; set; }
    }
}
