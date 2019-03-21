using SettlersOfCatan.MCTS.Interfaces;
using SettlersOfCatan.MCTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.MCTS.Algorithm
{
    public class MonteCarloTreeSearch : IMonteCarloTreeSearch
    {

        readonly int MAX_TIME = 2000;
        readonly int INFINITY = int.MaxValue;

        public void GetNextMove(Node root)
        {
            // while() dopoki czas sie nie skonczy
            while (true)
            {
                root = MakeSelection(root);
            }
           
        }

        Node MakeSelection(Node root)
        {
            var node = UCT.SelectNodeBasedOnUcb(root);

            if (node.Children.Count() == 0)
            {
                if (node.State.VisitsNum == 0)
                {
                    var score = MakeRollout(node);
                    node.State.WinsNum += score;
                    node.State.VisitsNum += 1;
                }
                else
                    MakeExpansion(node);
            }
            root.State.VisitsNum += 1;
            root.State.WinsNum += node.State.WinsNum;
            return root;
        }

        Node FindTheBestMove(Node node)
        {
            //if (node.Children.Count == 0)
            //    return node;
            //else
            //{
            //    var calculationResult = MakeSelection(node);
            //    calculationResult.Children.OrderByDescending(x => (x.State.WinsNum / x.State.SimulationNum));
            //    return calculationResult.Children.FirstOrDefault();
            //}

            MakeSelection(node);
            return null;
        }

     
        void MakeExpansion(Node node)
        {

        }

        void MakeBackPropagation()
        {

        }

        int MakeRollout(Node node)
        {
            //tutaj random leci
            return 0;
        }


    }
}
