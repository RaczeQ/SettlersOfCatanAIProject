using SettlersOfCatan.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.AI
{
    public interface IAgent
    {
        Settlement placeFreeSettlement(BoardState state);
        Road placeFreeRoad(BoardState state);
        //TerrainTile moveRobber(BoardState state);
        Object makeMove(BoardState state);
    }
}
