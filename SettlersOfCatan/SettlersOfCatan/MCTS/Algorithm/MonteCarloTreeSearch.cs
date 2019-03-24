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
            root.Depth = 0;
            Console.WriteLine(String.Format("Start MCTS. Current player number: {0}", CurrentPlayerNum));
            var i = 0;
           //czas sie skonczy
            while (i < 10)
            {
                root = MakeSelection(root);
                i++;
            }

            var nextMove = root.Children.OrderByDescending(x => (x.TotalScore)).FirstOrDefault();
            Console.WriteLine("MCTS: visits number: {0}, wins number {1}, next move: {2}", root.VisitsNum, root.WinsNum, nextMove.Move == null ? "no available moves" : nextMove.Move.ToString());

            return nextMove;
        }

        Node MakeSelection(Node root)
        {
            Console.WriteLine("Start execute node. Current player: {0}. Available children: {1}. Depth: {2}. Wins: {3}. Visits {4}",
                root.BoardState.player.playerNumber, root.Children.Count(), root.Depth, root.WinsNum, root.VisitsNum);

            if (root.Children.Count == 0)
            {
                root = ChangeToNextPlayer(root.BoardState);
                Console.WriteLine(String.Format("Player switched to {0}", root.BoardState.player.playerNumber));
                root = MakeSelection(root);
            }

            //

            var node = UCT.SelectNodeBasedOnUcb(root);

            if (node.VisitsNum == 0)
            {

                node.Depth = root.Depth + 1;
                Console.WriteLine("Current node depth {0}. Children index {1} ", node.Depth, root.Children.IndexOf(node));


                var score = MakeRollout(node);
                node.RolloutScore = score;
                node.WinsNum += score;
                node.VisitsNum += 1;
            }
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
            Console.WriteLine("Return one node above. Player {0}", root.BoardState.player.playerNumber);
            return root;
        }

        Node ChangeToNextPlayer(BoardState state)
        {
            var changedState = state.ChangeToNextPlayer();
            BoardState.RollDice(ref changedState);
            return tree.CreateRoot(changedState);
        }

        void MakeExpantion(Node node)
        {
            node = tree.ExtendChildrenNode(node.BoardState, node);
        }

     
        int MakeRollout(Node node)
        {
            var winer = node.BoardState.GetWinnerOfRandomGame();
            Console.WriteLine("Roolout winner is {0}", winer.playerNumber);
            return (winer.playerNumber == CurrentPlayerNum) ? 1 : 0;
        }
    }
}
