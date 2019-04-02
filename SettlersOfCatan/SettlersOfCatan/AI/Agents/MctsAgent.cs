using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SettlersOfCatan.GameObjects;
using SettlersOfCatan.MCTS.Algorithm;
using SettlersOfCatan.MCTS.Models;
using SettlersOfCatan.Moves;
using SettlersOfCatan.Results;

namespace SettlersOfCatan.AI.Agents
{

    public class MctsAgent : IAgent
    {
        private Random _r = new Random();
        [ThreadStatic]
        private static List<Move> nextMoves = new List<Move>();
        
        MonteCarloTreeSearch mcts = new MonteCarloTreeSearch();
        public Move makeMove(BoardState state)
        {
            if (nextMoves != null && nextMoves.Count > 0)
            {
                var move = nextMoves[0];
                nextMoves.Remove(move);
                return move;
            }

            var newState = Mapper.Map<BoardState>(state);

            Tree tree = new Tree();
            var root = tree.CreateRoot(newState);
            if (root.Children.Count == 0 || root.Children.Count == 1 && root.Children.FirstOrDefault().Move.GetType() == typeof(EndMove) )
            {
                Console.WriteLine("No possible moves. Player ended his turn.");
                return new EndMove();
            }
            var result = mcts.GetNextMove(root);
            root.PrintPretty("", true);
            Console.WriteLine();
            result.PrintPretty("", true);
            nextMoves = ExpandMovesFromWinnerNode(result, new List<Move>());

            SaveResult(root, result);
            return result.Move;
        }

        private List<Move> ExpandMovesFromWinnerNode(Node node, List<Move> moves)
        {
            if (node==null || node.Children==null || node.Children.Count == 0 || node.Children.Any(x => x.VisitsNum == 0))
                return moves;
            
            var nextNode = node.Children.OrderByDescending(x => (x.TotalScore)).FirstOrDefault();
            moves.Add(nextNode.Move);
            return ExpandMovesFromWinnerNode(nextNode, moves);       
        }

        private void SaveResult(Node root, Node nextMove)
        {
            var depthMeasures = Node.GetDepthMeasures(root);
            var meanDepthValue = depthMeasures.Item1;
            var medianDepthValue = depthMeasures.Item2;
            var deepestNodeValue = depthMeasures.Item3;
            var exploreFactor = Node.ChildrenExploredFactor(root);
            var nfi = new NumberFormatInfo() {NumberDecimalSeparator = "."};
            var result = String.Format("{0},{1},{2},{3},{4},{5}", 
                root.WinsNum.ToString(), // Number of wins
                root.VisitsNum.ToString(), // Number of visits / playouts
                meanDepthValue.ToString(nfi), // Mean depth
                medianDepthValue.ToString(nfi), // Median depth
                deepestNodeValue.ToString(), // Max depth
                exploreFactor.ToString(nfi) // Factor of exploration
            );
            FileWriter.SaveResultToFile(result);
        }

    
        public Road placeFreeRoad(BoardState state)
        { 
            return state.AvailableRoads.ElementAt(_r.Next(0, state.AvailableRoads.Count()));
        }

        public Settlement placeFreeSettlement(BoardState state)
        {
            
            var sortedSettlements = state.AvailableSettlements
               .OrderByDescending(
                   s => s.adjacentTiles
                       .Sum(t => t.getResourceType() != Board.ResourceType.Desert
                           ? BoardState.CHIP_MULTIPLIERS[t.numberChip.numberValue]
                           : 0)
               );
            return sortedSettlements.First();
        }
    }
}
