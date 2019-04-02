using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.AI.AssesmetFunctions
{
    public class SettlementAssesmentFunction : IAssesmentFunction
    {
       
        public int? getNewRoadIndex(BoardState state)
        {
            return 0;
        }

        public int? getNewSettlementIndex(BoardState state)
        {
            var availableSettlements = state.AvailableSettlements;
            return 0;
        }

    }
}
