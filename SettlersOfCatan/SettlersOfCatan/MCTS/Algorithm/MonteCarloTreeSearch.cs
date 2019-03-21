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

        public Node GetNextMove(Node root)
        {
           //czas sie skonczy
            while (true)
            {
                root = MakeSelection(root);
            }

            return root.Children.OrderByDescending(x => x.State.WinsNum).FirstOrDefault();
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
                    node = MakeSelection(node);
            }
            root.State.VisitsNum += 1;
            root.State.WinsNum += node.State.WinsNum;
            return root;
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
