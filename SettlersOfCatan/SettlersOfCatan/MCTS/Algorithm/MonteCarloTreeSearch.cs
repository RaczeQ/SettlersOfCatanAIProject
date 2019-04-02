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
        private readonly int MAX_TIME = 5;
        Tree tree = new Tree();
        int CurrentPlayerNum { get; set; }


        public Node GetNextMove(Node root)
        {
            CurrentPlayerNum = root.BoardState.player.playerNumber;
 
            Console.WriteLine(String.Format("Start MCTS. Current player number: {0}", CurrentPlayerNum));

            root = MakeSelectionStarter(root);

            var nextMove = root.Children.OrderByDescending(x => (x.TotalScore)).FirstOrDefault();
            Console.WriteLine("MCTS: visits number: {0}, wins number {1}, next move: {2}", root.VisitsNum, root.WinsNum, nextMove.Move == null ? "no available moves" : nextMove.Move.ToString());

            return nextMove;
        }

        private Node MakeSelectionStarter(Node root)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(MAX_TIME));
            while (true)
            {
                try
                {
                    root = MakeSelection(root, tokenSource.Token, 0);
                    root.PrintPretty("", true);
                } catch (OperationCanceledException)
                {
                    return root;
                }
            }
        }


        private Node MakeSelection(Node root, CancellationToken cancellationToken, int iter)
        {
            iter++;
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            try
            {
                Console.WriteLine("Start execute node. Current player: {0} Available children: {1} Wins: {2} Visits: {3}",
                    root.BoardState.player.playerNumber, root.Children.Count, root.WinsNum, root.VisitsNum);
            }
            catch(Exception) { }
          
            if (
                (root.Children == null || root.Children.Count == 0) || 
                (root.Move != null && root.Move.GetType() == typeof(EndMove) && 
                 (root.NodesAfterRandomRollDice == null || root.NodesAfterRandomRollDice.Count == 0))
                )
            {
                if(root.Move != null && root.Move.GetType() == typeof(EndMove))
                    Console.WriteLine(String.Format("End move"));

                var rootVisits = root.VisitsNum; 
                var rootWins = root.WinsNum;

                var rootWithChangesPlayer = ChangeToNextPlayer(root);

                if (root.NodesAfterRandomRollDice.Any(x => x.BoardState.CurrentStateHashCode == rootWithChangesPlayer.BoardState.CurrentStateHashCode))
                {
                    var node = root.NodesAfterRandomRollDice.FirstOrDefault(x => x.BoardState.CurrentStateHashCode == rootWithChangesPlayer.BoardState.CurrentStateHashCode);
                    Console.WriteLine("Node was copied from previous round");
                    rootWithChangesPlayer = MakeSelection(node, cancellationToken, iter);
                }
                else
                {
                    root.NodesAfterRandomRollDice.Add(rootWithChangesPlayer);
                    rootWithChangesPlayer = MakeSelection(rootWithChangesPlayer, cancellationToken, iter);
                }

                root.VisitsNum = rootVisits + 1;
                root.WinsNum = rootWins + rootWithChangesPlayer.RolloutScore;
                root.RolloutScore = rootWithChangesPlayer.RolloutScore;
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
                    if(node.Move != null && node.Move.GetType() != typeof(EndMove) && (node.Children == null || node.Children.Count == 0))
                        node = MakeExpansion(node);
                    node = MakeSelection(node, cancellationToken, iter);
                }
                if (node.VisitsNum != 0)
                {
                    root.VisitsNum += 1;
                    root.WinsNum += node.RolloutScore;
                    root.RolloutScore = node.RolloutScore;
                }
            }
            Console.WriteLine(" ======================> END ON TREE DEPTH => {0}, Wins {1}, visits {2}" , iter, root.WinsNum, root.VisitsNum);
            return root;
        }

        private Node ChangeToNextPlayer(Node node)
        {
            var changedState = node.BoardState.ChangeToNextPlayer();
            BoardState.RollDice(ref changedState);
            return tree.CreateRoot(changedState, node.Depth);
        }

        private Node MakeExpansion(Node node)
        {
            return tree.ExtendChildrenNode(node.BoardState, node);
        }

        int MakeRollout(Node node)
        {
            var winner = node.BoardState.GetWinnerOfRandomGame();
            node.Playouts += 1;
            Console.WriteLine("Roolout winner is {0}", winner.playerNumber);
            return (winner.playerNumber == CurrentPlayerNum) ? 1 : 0;
        }
    }
}
