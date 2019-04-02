using SettlersOfCatan.GameObjects;
using SettlersOfCatan.SimplifiedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.AI.AssesmetFunctions
{
    public class SimplifiedSettlementBuildAssesmentFunction : IAssesmentFunction
    {
        public int? getNewRoadIndex(BoardState state)
        {
            return findTheBestRoadIndex(state);
        }


        public int? getNewSettlementIndex(BoardState state)
        {
            return findTheBestSettlementIndex(state);
        }

        int? findTheBestSettlementIndex(BoardState state)
        {
            var player = state.player;
            var potentialSettlements = state.CanBuildNewSettlements
                       .Where(x => x.owningPlayer == null
                       && x.connectedRoads.Any(y => y.owningPlayer != null && y.owningPlayer != player))
                       .Select(x => new SimplifiedSettlement
                       {
                           Id = x.id,
                           OwningPlayer = x.owningPlayer,
                           ConnectedRoads = x.connectedRoads
                          .Select(
                              y => new SimplifiedRoad
                              {
                                  Id = y.id,
                                  OwningPlayer = y.owningPlayer
                              }
                              ).ToList(),
                           OccupiedRoads = x.connectedRoads.Where(z => z.owningPlayer != null && z.owningPlayer != player).Count()
                       }).ToList();
            if (potentialSettlements.Count > 0)
                return potentialSettlements.OrderByDescending(x => x.OccupiedRoads).ToList().FirstOrDefault().Id;
            else
                return null;
        }

        public int? findTheBestRoadIndex(BoardState state)
        {
            var availableRoads = state.CanBuildRoad;
            Dictionary<int, int> roadCosts = new Dictionary<int, int>();

            foreach (var item in availableRoads)
            {
                var score = getSettlementsCosts(item.id, item.connectedSettlements.Where(x => x.owningPlayer == state.player)?.FirstOrDefault()?.id, 0, item, 0, state);
                roadCosts.Add(item.id, score);
            }
            if (roadCosts.Count > 0)
                return roadCosts.OrderBy(x => x.Value).FirstOrDefault().Key;
            else
                return null;
        }

        int getSettlementsCosts(int startRoadId, int? startSettlementId, int i, Road road, int costs, BoardState state)
        {
            i++;
            if (i < 15)
            {
                foreach (var item in road.connectedSettlements.Where(x => x.id != startSettlementId))
                {
                    if (item.owningPlayer==null && item.connectedRoads.Any(x=> x.owningPlayer!= state.player) )
                        return costs;
                    else
                    {
                        costs++;
                        foreach (var r in item.connectedRoads.Where(x => x.id != startRoadId))
                        {
                            return getSettlementsCosts(r.id, startSettlementId, i, r,  costs, state);
                        }
                    }
                }
            }
            return costs;
        }

    }
}
