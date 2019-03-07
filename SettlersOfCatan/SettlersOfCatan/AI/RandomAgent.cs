using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.AI
{
    public class RandomAgent : IAgent
    {
        private Random _r = new Random();

        public object makeMove(BoardState state)
        {
            if (state.canUpgradeSettlement.Count() > 0)
            {
                return state.canUpgradeSettlement.ElementAt(_r.Next(0, state.canUpgradeSettlement.Count()));
            }
            else if (state.canBuildNewSettlements.Count() > 0)
            {
                return state.canBuildNewSettlements.ElementAt(_r.Next(0, state.canBuildNewSettlements.Count()));
            }
            else if (state.canBuildRoad.Count() > 0)
            {
                return state.canBuildRoad.ElementAt(_r.Next(0, state.canBuildRoad.Count()));
            }
            else
            {
                return state.endTurnButton;
            }
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
