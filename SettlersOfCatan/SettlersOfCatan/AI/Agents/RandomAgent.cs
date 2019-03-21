using SettlersOfCatan.GameObjects;
using SettlersOfCatan.SimplifiedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettlersOfCatan.Moves;

namespace SettlersOfCatan.AI.Agents
{
    public class RandomAgent : IAgent
    {
        private Random _r = new Random();
        private const int minResourceAmount = 4;

        public Move makeMove(BoardState state)
        {
            if (state.canBuildNewSettlements.Any())
            {
                return new BuildSettlementMove(
                    state.canBuildNewSettlements.ElementAt(_r.Next(0, state.canBuildNewSettlements.Count())));
            }
            else if (state.canUpgradeSettlement.Any())
            {
                return new BuildCityMove(
                    state.canUpgradeSettlement.ElementAt(_r.Next(0, state.canUpgradeSettlement.Count())));
            }
            else if (state.canBuildRoad.Any())
            {
                return new BuildRoadMove(state.canBuildRoad.ElementAt(_r.Next(0, state.canBuildRoad.Count())));
            }
            else if (state.resourcesAvailableToSell.Values.Any(x => x) &&
                     state.resourcesAvailableToBuy.Values.Any(x => x))
            {
                var amountLeft = state.playerResourcesAmounts
                    .Where(x => state.resourcesAvailableToSell[x.Key])
                    .ToDictionary(k => k.Key, v => v.Value - state.bankTradePrices[v.Key]);
                var boughtResource = state.playerResourcesAcquiredPerResource
                    .Where(x => state.resourcesAvailableToBuy[x.Key])
                    .OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList()[0];
                var selledResource = amountLeft.OrderBy(kv => -kv.Value).Select(kv => kv.Key).ToList()[0];
                if (boughtResource != selledResource)
                {
                    return new BankTradeMove(boughtResource, selledResource, 1);
                }
            }

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