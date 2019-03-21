using SettlersOfCatan.AI;
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
        Tree tree = new Tree();

        public Node GetNextMove(Node root)
        {
            var i = 0;
           //czas sie skonczy
            while (i < 10)
            {
                MakeSelection(root);
                i++;
            }

            var ttt = root;
            var temp = 5;
            return root.Children.OrderByDescending(x => x.WinsNum).FirstOrDefault();
        }

        void MakeSelection(Node root)
        {
            var node = UCT.SelectNodeBasedOnUcb(root);

            if (node.VisitsNum == 0)
            {
                var score = MakeRollout(node);
                node.WinsNum += score;
                node.VisitsNum += 1;
            }
            else
            {
                MakeExpantion(node);
                MakeSelection(node);
            }
            root.VisitsNum += 1;
            root.WinsNum += node.WinsNum;
            
        }

        void MakeExpantion(Node node)
        {
            node = tree.ExtendChildrenNode(node.BoardState, node);
        }
     
        int MakeRollout(Node node)
        {
            Random r = new Random();
            return r.Next(0, 2);
        }


    }
}
