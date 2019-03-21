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


        MonteCarloTreeSearch mcts = new MonteCarloTreeSearch();
        public Move makeMove(BoardState state)
        {
            var newState = Mapper.Map<BoardState>(state);

            Tree tree = new Tree();
            var root = tree.CreateRoot(newState);
            if (root.Children.Count == 0)
                return new EndMove();

            var result = mcts.GetNextMove(root);
            return new EndMove();


        }

        public Road placeFreeRoad(BoardState state)
        {
            return state.availableRoads.ElementAt(_r.Next(0, state.availableRoads.Count()));
        }

        public Settlement placeFreeSettlement(BoardState state)
        {
            return state.availableSettlements.ElementAt(_r.Next(0, state.availableSettlements.Count()));
        }
    }
}
