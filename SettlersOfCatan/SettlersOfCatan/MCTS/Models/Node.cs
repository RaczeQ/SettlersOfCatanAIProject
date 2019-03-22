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
        public int WinsNum { get; set; }
        public int VisitsNum { get; set; }
        public Move Move { get; set; }
        public BoardState BoardState { get; set; }
        public List<Node> Children { get; set; }
    }
}
