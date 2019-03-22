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
        int CurrentPlayerNum { get; set; }

        public Node GetNextMove(Node root)
        {
            CurrentPlayerNum = root.BoardState.player.playerNumber;
            var i = 0;
           //czas sie skonczy
            while (i < 20)
            {
                root = MakeSelection(root);
                i++;
            }
            return root.Children.OrderByDescending(x => x.WinsNum).FirstOrDefault();
        }

        Node MakeSelection(Node root)
        {
            var node = UCT.SelectNodeBasedOnUcb(root);

            if (node == null)
                return root;
            else if (node.VisitsNum == 0)
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
            return root;
        }

        void MakeExpantion(Node node)
        {
            node = tree.ExtendChildrenNode(node.BoardState, node);
        }

        void MakeBackPropagation(Node node)
        {

        }
     
        int MakeRollout(Node node)
        {
            //var winner =  node.BoardState.GetWinnerOfRandomGame();
            //return winner.playerNumber == CurrentPlayerNum ? 1 : 0;

            var  r = new Random().Next(0, 2);
            return r;
        }
    }
}
