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
            while (i < 10)
            {
                root = MakeSelection(root);
                i++;
            }

            var nextMove = root.Children.OrderByDescending(x => x.WinsNum).FirstOrDefault();
            Console.WriteLine("MCTS: visits number: {0}, wins number {1}, next move: {2}", root.VisitsNum, root.WinsNum, nextMove.Move == null ? "no available moves" : nextMove.Move.ToString());

            return nextMove;
        }

        Node MakeSelection(Node root)
        {
            // if brak możliwości - przełączyć na drugiego gracza -> oprócz korzenia całego drzewa -> POPRAWIc ten syf
            //if (root.Children.Count == 0 && root.VisitsNum == 0)
            //{
            //    root.VisitsNum += 1;
            //    return root;
            //}
            // TUTAJ PRZELACZAC GRACZA ! -> ten moment nie działa
            if (root.Children.Count == 0)
                return root;

            var node = UCT.SelectNodeBasedOnUcb(root);
            
            if (node.VisitsNum == 0)
            {
                var score = MakeRollout(node);
                node.RolloutScore = score;
                node.WinsNum += score;
                node.VisitsNum += 1;
            }
            // +  Children -> null
            else
            {
                MakeExpantion(node);
                node = MakeSelection(node);
            }
            if (node.VisitsNum != 0)
            {
                root.VisitsNum += 1;
                root.WinsNum += node.RolloutScore;
            }
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
            //var winner = node.BoardState.GetWinnerOfRandomGame();
            //return winner.playerNumber == CurrentPlayerNum ? 1 : 0;

            var r = new Random().Next(0, 2);
            return r;
        }
    }
}
