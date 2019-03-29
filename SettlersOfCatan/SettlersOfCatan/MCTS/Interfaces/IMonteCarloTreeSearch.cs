using SettlersOfCatan.MCTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.MCTS.Interfaces
{
    public interface IMonteCarloTreeSearch
    {
        Node GetNextMove(ref Node root);
     
    }
}
