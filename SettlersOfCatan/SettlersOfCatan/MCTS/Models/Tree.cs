using SettlersOfCatan.AI;
using SettlersOfCatan.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.MCTS.Models
{
    public class Tree
    {
        public Node Root { get; set; }
        private Random _r = new Random();

        public Node CreateRoot(BoardState state, int depth = 0)
        {
            Root = new Node()
            {
                BoardState = state,
                Children = new List<Node>(),
                Depth = depth
            };
            return ExtendChildrenNode(state, Root);
        }

        public Node ExtendChildrenNode(BoardState state, Node node)
        {
            node.Children = new List<Node>();

            if (state.CanBuildNewSettlements.Any())
            {
                var sortedSettlements = state.CanBuildNewSettlements
                .OrderByDescending(
                    s => s.adjacentTiles
                        .Sum(t => t.getResourceType() != Board.ResourceType.Desert
                            ? BoardState.CHIP_MULTIPLIERS[t.numberChip.numberValue]
                            : 0)
                );
                var move = new BuildSettlementMove(sortedSettlements.First());
                node.Children.Add(new Node()
                {
                    Move = move,
                    BoardState = state.MakeMove(move),
                    Depth = node.Depth + 1
                });
            }

            if (state.CanUpgradeSettlement.Any())
            {
                var sortedCities = state.CanUpgradeSettlement
                .OrderByDescending(
                    s => s.adjacentTiles
                        .Sum(t => t.getResourceType() != Board.ResourceType.Desert
                            ? BoardState.CHIP_MULTIPLIERS[t.numberChip.numberValue]
                            : 0)
                );
                var move = new BuildCityMove(sortedCities.First());
                node.Children.Add(new Node()
                {
                    Move = move,
                    BoardState = state.MakeMove(move),
                    Depth = node.Depth + 1
                });
            }

//            var endSettlements = state.Settlements.Where(s => s.owningPlayer == state.player || s.checkForOtherSettlement())
//                .Where(s =>
//                    s.connectedRoads.Count(
//                        r => r.owningPlayer == state.player) == 1).ToList();
            var endRoads = state.CanBuildRoad
                .Where(r => r.connectedSettlements
                    .Any(s => s.connectedRoads.Count(cr => cr.owningPlayer != null) == 1))
                .ToList();
            if (endRoads.Any())
            {
                
                var move = new BuildRoadMove(endRoads.ElementAt(_r.Next(0, endRoads.Count())));
                node.Children.Add(new Node()
                {
                    Move = move,
                    BoardState = state.MakeMove(move),
                    Depth = node.Depth + 1
                });
            }
            else if (state.CanBuildRoad.Any())
            {
                var move = new BuildRoadMove(state.CanBuildRoad.ElementAt(_r.Next(0, state.CanBuildRoad.Count())));
                node.Children.Add(new Node()
                {
                    Move = move,
                    BoardState = state.MakeMove(move),
                    Depth = node.Depth + 1
                });
            }

            if (state.ResourcesAvailableToSell.Values.Any(x => x) &&
                state.ResourcesAvailableToBuy.Values.Any(x => x))
            {
                var copy = state;
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
                    if (move.CanMakeMove(copy))
                    {
                        node.Children.Add(new Node()
                        {
                            Move = move,
                            BoardState = copy.MakeMove(move),
                            Depth = node.Depth + 1
                        });
                    }
                }
            }

            var copyState = state;
            node.Children.Add(new Node()
            {
                Move = new EndMove(),
                Children = new List<Node>(),
                BoardState = copyState.MakeMove(new EndMove()),
                Depth = node.Depth + 1
            });
            return node;
        }
    }
}

