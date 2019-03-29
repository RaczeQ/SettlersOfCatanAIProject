using SettlersOfCatan.AI;
using SettlersOfCatan.MCTS.Interfaces;
using SettlersOfCatan.MCTS.Models;
using SettlersOfCatan.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SettlersOfCatan.MCTS.Algorithm
{
    public class MonteCarloTreeSearch : IMonteCarloTreeSearch
    {
        readonly int MAX_TIME = 20;
        Tree tree = new Tree();
        int CurrentPlayerNum { get; set; }


        public Node GetNextMove(Node root)
        {
            CurrentPlayerNum = root.BoardState.player.playerNumber;
 
            Console.WriteLine(String.Format("Start MCTS. Current player number: {0}", CurrentPlayerNum));

            root = MakeSelectionStarter(root).Result;

            var nextMove = root.Children.OrderByDescending(x => (x.TotalScore)).FirstOrDefault();
            Console.WriteLine("MCTS: visits number: {0}, wins number {1}, next move: {2}", root.VisitsNum, root.WinsNum, nextMove.Move == null ? "no available moves" : nextMove.Move.ToString());

            return nextMove;
        }

        private async Task<Node> MakeSelectionStarter(Node root)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(MAX_TIME));
            while (true)
            {
                try
                {
                    root = await MakeSelection(root, tokenSource.Token, 0);
                } catch (OperationCanceledException)
                {
                    return root;
                }
            }
        }


        async Task<Node> MakeSelection(Node root, CancellationToken cancellationToken, int iter)
        {
            iter++;
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            try
            {
                Console.WriteLine("Start execute node. Current player: {0}. Available children: {1}. Depth: {2}. Wins: {3}. Visits {4}",
                    root.BoardState.player.playerNumber, root.Children.Count(), root.Depth, root.WinsNum, root.VisitsNum);
            }
            catch (Exception) { }

            if ((root.Children == null || root.Children.Count == 0) || (root.Move != null && root.Move.GetType() == typeof(EndMove)))
            {
                if(root.Move != null && root.Move.GetType() == typeof(EndMove))
                    Console.WriteLine(String.Format("End move"));

                var rootVisits = root.VisitsNum; 
                var rootWins = root.WinsNum;

                var rootWithChangesPlayer = ChangeToNextPlayer(root.BoardState);
                rootWithChangesPlayer = await MakeSelection(rootWithChangesPlayer, cancellationToken, iter);

                root.VisitsNum = rootVisits + 1;
                root.WinsNum = rootWins + rootWithChangesPlayer.RolloutScore;
                Console.WriteLine("Ended switch. Wins {0}", (root!=null ? root.WinsNum.ToString() : "not known"));
            }
            else
            {
                var node = UCT.SelectNodeBasedOnUcb(root);

                if (node.VisitsNum == 0)
                {  
                    var score = MakeRollout(node);
                    node.RolloutScore = score;
                    node.WinsNum += score;
                    node.VisitsNum += 1;
                }
                else
                {
                    if (node.Move.GetType() != typeof(EndMove))
                    {
                        MakeExpansion(node);
                        node = await MakeSelection(node, cancellationToken, iter);
                    }
                }
                if (node.VisitsNum != 0)
                {
                    root.VisitsNum += 1;
                    root.WinsNum += node.RolloutScore;
                    root.RolloutScore = node.RolloutScore;
                }
            }
            Console.WriteLine("Return from node. Player {0}. Wins: {1}. Visits {2}", root.BoardState.player.playerNumber, root.WinsNum, root.VisitsNum);
            Console.WriteLine(" ======================> END ON TREE DEPTH => " + iter);

            return root;
        }

        Node ChangeToNextPlayer(BoardState state)
        {
            var changedState = state.ChangeToNextPlayer();
            BoardState.RollDice(ref changedState);
            return tree.CreateRoot(changedState);
        }

        void MakeExpansion(Node node)
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
