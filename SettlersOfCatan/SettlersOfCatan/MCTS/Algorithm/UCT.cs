using SettlersOfCatan.MCTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.MCTS.Algorithm
{
    //(Upper Confidence Bound applied to trees) 
    public static class UCT
    {
        readonly static double EXPLORATION = Math.Sqrt(2.0);
        public static double UtcValue(int winsNum, int visitsNum, int totalVisitsNum)
        {
            if (visitsNum == 0)
                return int.MaxValue;
            else
                return (winsNum / visitsNum) + EXPLORATION * Math.Sqrt(Math.Log(totalVisitsNum) / visitsNum);
        }

        public static Node SelectNodeBasedOnUcb(Node parent)
        {
            return parent.Children.OrderByDescending(x => UCT.UtcValue(x.WinsNum, x.VisitsNum, parent.VisitsNum)).FirstOrDefault();
        }
    }
}
