namespace ClientAgentTarget.ViewModel
{
    public class StatusTargetsVM
    {
        public long Id { get; set; }
        public string TargetName { get; set; }
        public string Image {  get; set; }
        public string Role { get; set; }
        public int X {  get; set; }
        public int Y { get; set; }
        public string TargetStatus { get; set; }
        public bool IsHeOnTheWayToHell { get; set; }
        public double TimeToElimanate { get; set; }

    }
}
