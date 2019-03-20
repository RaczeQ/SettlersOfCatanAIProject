using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettlersOfCatan.GameObjects;

namespace SettlersOfCatan.SimplifiedModels
{
    public class SimplifiedTile
    {
        public int Index { get; set; }
        public NumberChip NumberChip { get; set; }
        public int GatherChance { get; set; }
        public Board.ResourceType TileType { get; set; }
        public List<SimplifiedSettlement> AdjacentSettlements { get; set; } = new List<SimplifiedSettlement>();
        public List<SimplifiedRoad> AdjacentRoads { get; set; } = new List<SimplifiedRoad>();
    }
}
