using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SettlersOfCatan
{
    public class BoardState
    {
        public BoardState(Player p, IEnumerable<Tile> t, IEnumerable<Settlement> s, IEnumerable<Road> r, Button b)
        {
            player = p;
            tiles = t;
            settlements = s;
            roads = r;
            endTurnButton = b;
        }
        public Player player { get; private set; }
        public IEnumerable<Tile> tiles { get; private set; }
        public IEnumerable<TerrainTile> terrainTiles { get { return tiles.OfType<TerrainTile>(); } }
        public IEnumerable<Settlement> settlements { get; private set; }
        public IEnumerable<Settlement> availableSettlements { get { return settlements
                    .Where(s => s.checkForOtherSettlement()); } }
        public IEnumerable<Settlement> canBuildNewSettlements { get { return availableSettlements
                    .Where(s => s.checkForConnection(player) && Bank.hasPayment(player, Bank.SETTLEMENT_COST)); } }
        public IEnumerable<Settlement> canUpgradeSettlement { get { return player.settlements
                .Where(s => !s.isCity && Bank.hasPayment(player, Bank.CITY_COST)); } }
        public IEnumerable<Road> roads { get; private set; }
        public IEnumerable<Road> availableRoads { get { return roads
                    .Where(r => r.owningPlayer == null && r.checkForConnection(player)); } }
        public IEnumerable<Road> canBuildRoad { get { return availableRoads
                    .Where(r => Bank.hasPayment(player, Bank.ROAD_COST)); } }
        public Button endTurnButton { get; private set; }
    }
}
