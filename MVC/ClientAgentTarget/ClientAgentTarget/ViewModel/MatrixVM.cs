using ClientAgentTarget.Models;

namespace ClientAgentTarget.ViewModel
{
    public class MatrixVM
    {
        public string[,] Matrix { get; set; }
        public List<AgentModel> Agents { get; set; }
        public List<TargetModel> Targets { get; set; }
        public MatrixVM(int rows, int cols)
        {
            Matrix = new string[rows, cols];
            Agents = new List<AgentModel>();
            Targets = new List<TargetModel>();
        }
    }
}
