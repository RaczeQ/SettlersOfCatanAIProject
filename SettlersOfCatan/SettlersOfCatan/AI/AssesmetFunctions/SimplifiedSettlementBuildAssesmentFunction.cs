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
        public int getNewRoadIndex(BoardState state)
        {
            throw new NotImplementedException();
        }

        public int getNewSettlementIndex(BoardState state)
        {
            return findTheBestSettlementIndex(state);
        }
    
        //Na razie uwzglednia grę tylko dla dwóch graczy
        //zwraca indeks nowej osady która łączy najwięcej dróg przeciwnika
        int findTheBestSettlementIndex(BoardState state)
        {
            var player = state.player;
            var potentialSettlements = state.availableSettlements
                       .Where(x => x.owningPlayer == null
                       && x.connectedRoads.Any(y => y.owningPlayer != null && y.owningPlayer != player))
                       .Select( x=> new SimplifiedSettlement
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
            var res = potentialSettlements.OrderByDescending(x => x.OccupiedRoads).ToList();
            var resultId = res.FirstOrDefault().Id;
            return resultId;
        }
    }

   
  
}
