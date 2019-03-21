using SettlersOfCatan.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.MCTS.Models
{
    public class State
    {
        public int WinsNum { get; set; }
        public int VisitsNum { get; set; }
        public BoardState BoardState { get; set; }
    }
}
