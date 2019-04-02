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
            if (state.CanBuildNewSettlements.Any())
            {
                return new BuildSettlementMove(
                    state.CanBuildNewSettlements.ElementAt(_r.Next(0, state.CanBuildNewSettlements.Count())));
            }
            else if (state.CanUpgradeSettlement.Any())
            {
                return new BuildCityMove(
                    state.CanUpgradeSettlement.ElementAt(_r.Next(0, state.CanUpgradeSettlement.Count())));
            }
            else if (state.CanBuildRoad.Any())
            {
                return new BuildRoadMove(state.CanBuildRoad.ElementAt(_r.Next(0, state.CanBuildRoad.Count())));
            }
            else if (state.ResourcesAvailableToSell.Values.Any(x => x) &&
                     state.ResourcesAvailableToBuy.Values.Any(x => x))
            {
                var amountLeft = state.PlayerResourcesAmounts
                    .Where(x => state.ResourcesAvailableToSell[x.Key])
                    .ToDictionary(k => k.Key, v => v.Value - state.BankTradePrices[v.Key]);
                var boughtResource = state.PlayerResourcesAcquiredPerResource
                    .Where(x => state.ResourcesAvailableToBuy[x.Key])
                    .OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList()[0];
                var selledResource = amountLeft.OrderBy(kv => -kv.Value).Select(kv => kv.Key).ToList()[0];
                if (boughtResource != selledResource)
                {
                    var move = new BankTradeMove(boughtResource, selledResource, 1);
                    if (move.CanMakeMove(state))
                    {
                        return new BankTradeMove(boughtResource, selledResource, 1);
                    }
                }
            }

            return new EndMove();
        }

        public Road placeFreeRoad(BoardState state)
        {
            return state.AvailableRoads.ElementAt(_r.Next(0, state.AvailableRoads.Count()));
        }

        public Settlement placeFreeSettlement(BoardState state)
        {
            return state.AvailableSettlements.ElementAt(_r.Next(0, state.AvailableSettlements.Count()));
        }
    }
}