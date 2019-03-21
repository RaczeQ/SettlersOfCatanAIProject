using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.MCTS.Models
{
    public class Node
    {
        public State State { get; set; }
        public List<Node> Children { get; set; }
    }
}
