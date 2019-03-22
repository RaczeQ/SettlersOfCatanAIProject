using SettlersOfCatan.AI.AssesmetFunctions;
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
    public class AggressiveAgent : IAgent
    {
        SimplifiedSettlementBuildAssesmentFunction settlementBuildAssesmentFunction =
            new SimplifiedSettlementBuildAssesmentFunction();

        private Random _r = new Random();
        private const int minResourceAmount = 4;

        public Move makeMove(BoardState state)
        {
            var moves = new List<Tuple<double, Move>>();
            var currentScore = state.GetScore;

            if (state.canBuildNewSettlements.Any())
            {
                var newState = new BoardState(state);
                var index = settlementBuildAssesmentFunction.getNewSettlementIndex(state);
                var settlement = state.canBuildNewSettlements.ElementAt(index == null
                    ? _r.Next(0, state.canBuildNewSettlements.Count())
                    : state.canBuildNewSettlements.ToList()
                        .IndexOf(state.canBuildNewSettlements.FirstOrDefault(x => x.id == index)));
                var move = new BuildSettlementMove(settlement);
                move.MakeMove(ref newState);
                var score = newState.GetScore;
                moves.Add(new Tuple<double, Move>(score - currentScore, move));
            }

            if (state.canUpgradeSettlement.Any())
            {
                var newState = new BoardState(state);
                var settlement = state.canUpgradeSettlement
                    .OrderByDescending(
                        s => s.adjacentTiles
                            .Sum(t => t.getResourceType() != Board.ResourceType.Desert
                                ? BoardState.CHIP_MULTIPLIERS[t.numberChip.numberValue]
                                : 0)
                    );
                var move = new BuildCityMove(settlement.First());
                move.MakeMove(ref newState);
                var score = newState.GetScore;
                moves.Add(new Tuple<double, Move>(score - currentScore, move));
            }

            if (state.canBuildRoad.Any())
            {
                var newState = new BoardState(state);
                var index = settlementBuildAssesmentFunction.getNewRoadIndex(state);
                var road = state.canBuildRoad.ElementAt(index == null
                    ? _r.Next(0, state.canBuildRoad.Count())
                    : state.canBuildRoad.ToList().IndexOf(state.canBuildRoad.FirstOrDefault(x => x.id == index)));
                var move = new BuildRoadMove(road);
                move.MakeMove(ref newState);
                var score = newState.GetScore;
                moves.Add(new Tuple<double, Move>(score - currentScore, move));
            }

            if (state.resourcesAvailableToSell.Values.Any(x => x) &&
                     state.resourcesAvailableToBuy.Values.Any(x => x))
            {
                var newState = new BoardState(state);
                var amountLeft = state.playerResourcesAmounts
                    .Where(x => state.resourcesAvailableToSell[x.Key])
                    .ToDictionary(k => k.Key, v => v.Value - state.bankTradePrices[v.Key]);
                var boughtResource = state.playerResourcesAcquiredPerResource
                    .Where(x => state.resourcesAvailableToBuy[x.Key])
                    .OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList()[0];
                var selledResource = amountLeft.OrderBy(kv => -kv.Value).Select(kv => kv.Key).ToList()[0];
                if (boughtResource != selledResource)
                {
                    var move = new BankTradeMove(boughtResource, selledResource, 1);
                    if (move.CanMakeMove(state))
                    {
                        move.MakeMove(ref newState);
                        var score = newState.GetScore;
                        moves.Add(new Tuple<double, Move>(score - currentScore, move));
                    }
                }
            }

            moves.Add(new Tuple<double, Move>(0, new EndMove()));

            return moves.OrderByDescending(t => t.Item1).First().Item2;
        }

        public Road placeFreeRoad(BoardState state)
        {
            //TODO
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

            // List<SimplifiedSettlement> selected = null;
            // var currentSettlements = state.player.settlements
            //     .Select(x => new SimplifiedSettlement
            //     {
            //         Id = x.id,
            //         Weight = x.adjacentTiles.Sum(y => getTileWeightBasedOnCreatingNewSettlementsStrategy(y.tileType)),
            //         TileWeights = x.adjacentTiles.Select(y => y.tileType).ToList()
            //     }).ToList();

            // var possible_settlements = state.availableSettlements.
            //     Select(x => new SimplifiedSettlement
            //     {
            //         Id = x.id,
            //         Weight = x.adjacentTiles.Sum(y => getTileWeightBasedOnCreatingNewSettlementsStrategy(y.tileType)),
            //         TileWeights = x.adjacentTiles.Select(y=> y.tileType).ToList()
            //     }).ToList();

            // if (currentSettlements.Count()>0)
            // {
            //     selected = possible_settlements.OrderByDescending(x => x.TileWeights.Distinct().Count()).ThenByDescending(z => z.Weight).ToList();
            //     var result = selected.Where(x => !currentSettlements.Any(y => y.TileWeights.OrderBy(z => z.ToString()) == x.TileWeights.OrderBy(z => z.ToString()))).FirstOrDefault();
            //     return state.availableSettlements.ElementAt(state.availableSettlements.ToList().IndexOf(state.availableSettlements.Where(x => x.id == result.Id).FirstOrDefault()));
            // }
            // else
            // {
            //     selected = possible_settlements.OrderByDescending(x => x.TileWeights.Distinct().Count()).ThenByDescending(z => z.Weight).ToList();
            //     var result = selected.FirstOrDefault();
            //     return state.availableSettlements.ElementAt(state.availableSettlements.ToList().IndexOf(state.availableSettlements.Where(x => x.id == result.Id).FirstOrDefault()));
            // }
        }

        private int getTileWeightBasedOnCreatingNewSettlementsStrategy(Board.ResourceType type)
        {
            if (type == Board.ResourceType.Brick || type == Board.ResourceType.Wood)
                return 2;
            else if (type == Board.ResourceType.Sheep || type == Board.ResourceType.Ore)
                return 1;
            else return 0;
        }


        private bool agent_can_build_road()
        {
            return false;
        }
    }
}