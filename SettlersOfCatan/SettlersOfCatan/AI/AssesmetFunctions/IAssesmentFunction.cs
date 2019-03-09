using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.AI.AssesmetFunctions
{
    public interface IAssesmentFunction
    {
        int getNewSettlementIndex(BoardState state);

        int getNewRoadIndex(BoardState state);
    }
}
