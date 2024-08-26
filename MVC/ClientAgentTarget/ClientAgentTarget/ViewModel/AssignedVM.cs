namespace ClientAgentTarget.ViewModel
{
    public class AssignedVM
    {
        public long MissionId { get; set; }
        public string AgentName { get; set; }
        public int AgentLocationX  { get; set; }
        public int AgentLocationY  { get; set; }
        
        public string TargetName { get; set; }
        public int TargetLocationX { get; set; }
        public int TargetLocationY { get; set; }
        public string TargetImage {  get; set; }
        public string Role {  get; set; }
        public double Distance { get; set; }
        public double TimeUntillEliminated { get; set; }
        public string MissionSituation { get; set; }
    }
}
