using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SettlersOfCatan.GameObjects;
using SettlersOfCatan.MCTS.Algorithm;
using SettlersOfCatan.MCTS.Models;
using SettlersOfCatan.Moves;

namespace SettlersOfCatan.AI.Agents
{

    public class MctsAgent : IAgent
    {
        private Random _r = new Random();
        private List<Move> nextMoves = new List<Move>();
        
        MonteCarloTreeSearch mcts = new MonteCarloTreeSearch();
        public Move makeMove(BoardState state)
        {
            if (nextMoves.Count > 0)
            {
                var move = nextMoves[0];
                nextMoves.Remove(move);
                return move;
            }

            var newState = Mapper.Map<BoardState>(state);

            Tree tree = new Tree();
            var root = tree.CreateRoot(newState);
            if (root.Children.Count == 0)
            {
                Console.WriteLine("No possible moves. Player ended his turn.");
                return new EndMove();
            }
            var result = mcts.GetNextMove(root);
            nextMoves = ExpandMovesFromWinnerNode(result, new List<Move>() );
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

        public Road placeFreeRoad(BoardState state)
        { 
            return state.availableRoads.ElementAt(_r.Next(0, state.availableRoads.Count()));
        }

        public Settlement placeFreeSettlement(BoardState state)
        {
            
            var sortedSettlements = state.availableSettlements
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
