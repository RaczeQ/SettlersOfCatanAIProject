using SettlersOfCatan.AI;
using SettlersOfCatan.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.MCTS.Models
{
    public class Node
    {
        public int RolloutScore { get; set; }
        public int WinsNum { get; set; } = 0;
        public int VisitsNum { get; set; } = 0;
        public double TotalScore
        {
            get
            {
                return VisitsNum != 0 ? (WinsNum / VisitsNum) : 0;
            }
        }

        public Move Move { get; set; }
        public BoardState BoardState { get; set; }
        public List<Node> Children { get; set; }



    }
}
