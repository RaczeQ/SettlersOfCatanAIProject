using SettlersOfCatan.SimplifiedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.AI.Agents
{
   
    public class ControllingAgent : IAgent
    {
        private Random _r = new Random();
        private const int minResourceAmount = 4;

        public object makeMove(BoardState state)
        {
            if (state.canBuildNewSettlements.Count() > 0)
            {
                return state.canBuildNewSettlements.ElementAt(_r.Next(0, state.canBuildNewSettlements.Count()));
            }
            else if (state.canUpgradeSettlement.Count() > 0)
            {
                return state.canUpgradeSettlement.ElementAt(_r.Next(0, state.canUpgradeSettlement.Count()));
            }
            else if (state.canBuildRoad.Count() > 0)
            {
                var roadId =  getTheBestRoadId(state);
                if(roadId == null)
                    return state.canBuildRoad.ElementAt(_r.Next(0, state.canBuildRoad.Count()));
                else
                    return state.canBuildRoad.ElementAt(state.canBuildRoad.ToList().IndexOf(state.canBuildRoad.Where(x => x.id == roadId).FirstOrDefault()));

            }
            else if (state.resourcesAvailableToSell.Values.Any(x => x))
            {
                // Buy resource with lowest amount for resource with highest amount left after buy
                var amountLeft = state.playerResourcesAmounts
                    .Where(x => state.resourcesAvailableToSell[x.Key])
                    .ToDictionary(k => k.Key, v => v.Value - state.bankTradePrices[v.Key]);
                //var boughtResource = state.playerResourcesAmounts.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList()[0];
                var boughtResource = state.playerResourcesAcquiredPerResource.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList()[0];
                var selledResource = amountLeft.OrderBy(kv => -kv.Value).Select(kv => kv.Key).ToList()[0];
                if (boughtResource != selledResource)
                {
                    TradeProposition proposition = new TradeProposition()
                    {
                        boughtResource = boughtResource,
                        selledResource = selledResource,
                        boughtResourceAmount = 1
                    };
                    return proposition;
                }
            }
            return state.endTurnButton;
        }

        public Road placeFreeRoad(BoardState state)
        {
            return state.availableRoads.ElementAt(_r.Next(0, state.availableRoads.Count()));
            //var roadId = getTheBestRoadId(state);
            //return state.canBuildRoad.ElementAt(state.canBuildRoad.ToList().IndexOf(state.canBuildRoad.Where(x => x.id == roadId).FirstOrDefault()));

        }

        public Settlement placeFreeSettlement(BoardState state)
        {
            int indexId = 0;
            var possible_settlements = state.availableSettlements.
                Select(x => new SimplifiedSettlement
                {
                    Id = x.id,
                    TitleWeight = x.adjacentTiles.Select(y => y.tileType).ToList()
                }).ToList();
            var rarestTitle = getOponentTheRarestResources(state);
            if(rarestTitle!=null && rarestTitle?.FirstOrDefault().Key != Board.ResourceType.Desert)
                indexId = state.availableSettlements.ToList().IndexOf( state.availableSettlements.Where(x => x.adjacentTiles.Any(y => y.tileType == rarestTitle.FirstOrDefault().Key)).FirstOrDefault());   
            else
                indexId = _r.Next(0, state.availableSettlements.Count());
            return state.availableSettlements.ElementAt(indexId);
        }

        private List<KeyValuePair<Board.ResourceType, int>> getOponentTheRarestResources(BoardState state)
        {
            var oponentResources = state.settlements
                .Where(x => x.owningPlayer != null
                && x.owningPlayer != state.player)
                .Select(y => new
                {
                    Title = y.adjacentTiles.Select(z => z.tileType).ToList(),
                    Wood = y.adjacentTiles.Where(z => z.tileType == Board.ResourceType.Wood).Count(),
                    Ore = y.adjacentTiles.Where(z => z.tileType == Board.ResourceType.Ore).Count(),
                    Brick = y.adjacentTiles.Where(z => z.tileType == Board.ResourceType.Brick).Count(),
                    Sheep = y.adjacentTiles.Where(z => z.tileType == Board.ResourceType.Sheep).Count(),
                }).ToList();

            if (oponentResources.Count > 0)
            {
                Dictionary<Board.ResourceType, int> dictionary = new Dictionary<Board.ResourceType, int>();
                dictionary.Add(Board.ResourceType.Wood, oponentResources.Sum(x => x.Wood));
                dictionary.Add(Board.ResourceType.Ore, oponentResources.Sum(x => x.Ore));
                dictionary.Add(Board.ResourceType.Brick, oponentResources.Sum(x => x.Brick));
                dictionary.Add(Board.ResourceType.Sheep, oponentResources.Sum(x => x.Sheep));

                return dictionary.OrderBy(x => x.Value).ToList();
            }
            return null;
        }

        private int? getTheBestRoadId(BoardState state){
            var rarestResources = getOponentTheRarestResources(state).FirstOrDefault().Key;
            var availableRoads = state.canBuildRoad;


            Dictionary<int, int> roadCosts = new Dictionary<int, int>();

            
            foreach(var item in availableRoads)
            {
                var score = getSettlementsCosts(item.id, item.connectedSettlements.Where(x => x.owningPlayer == state.player)?.FirstOrDefault()?.id, 0, item, rarestResources, 0);
                roadCosts.Add(item.id, score);
            }
            if (roadCosts.Count > 0)
                return roadCosts.OrderBy(x => x.Value).FirstOrDefault().Key;
            else
                return null;
        }


        private int getSettlementsCosts(int startRoadId, int? startSettlementId,  int i, Road road, Board.ResourceType type, int costs)
        {
            i++;
            if (i < 15)
            {
                foreach (var item in road.connectedSettlements.Where( x=> x.id!=startSettlementId))
                {
                    if (item.adjacentTiles.Any(x => x.tileType == type) && !item.connectedRoads.Any(x=> x.connectedSettlements.Any(y=> y.id==startSettlementId)))
                        return costs;
                    else
                    {
                        costs++;
                        foreach (var r in item.connectedRoads.Where(x=> x.id!=startRoadId))
                        {
                            return getSettlementsCosts(r.id, startSettlementId, i, r, type, costs);
                        }
                    }
                }
            }
            return costs;
        }
    }
}
