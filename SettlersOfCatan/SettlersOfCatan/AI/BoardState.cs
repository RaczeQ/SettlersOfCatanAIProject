using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SettlersOfCatan.AI
{
    public class BoardState
    {
        public BoardState(Board b)
        {
            player = b.currentPlayer;
            tiles = b.boardTiles;
            settlements = b.settlementLocations;
            roads = b.roadLocations;
            endTurnButton = b.btnEndTurn;
            bankTradePrices = b.getPlayerBankCosts(player);
            playerResourcesAmounts = Enum.GetValues(typeof(Board.ResourceType))
                .Cast<Board.ResourceType>()
                .ToDictionary(k => k, v => player.getResourceCount(v));
        }
        public Player player { get; private set; }
        public IEnumerable<Tile> tiles { get; private set; }
        public IEnumerable<TerrainTile> terrainTiles { get { return tiles.OfType<TerrainTile>(); } }
        public IEnumerable<Settlement> settlements { get; private set; }
        public IEnumerable<Settlement> availableSettlements
        {
            get
            {
                return settlements
                    .Where(s => s.owningPlayer == null && s.checkForOtherSettlement());
            }
        }
        public IEnumerable<Settlement> canBuildNewSettlements
        {
            get
            {
                return availableSettlements
                    .Where(s => s.checkForConnection(player) && Bank.hasPayment(player, Bank.SETTLEMENT_COST));
            }
        }
        public IEnumerable<Settlement> canUpgradeSettlement
        {
            get
            {
                return player.settlements
                    .Where(s => !s.isCity && Bank.hasPayment(player, Bank.CITY_COST));
            }
        }
        public IEnumerable<Road> roads { get; private set; }
        public IEnumerable<Road> availableRoads
        {
            get
            {
                return roads
                    .Where(r => r.owningPlayer == null && r.checkForConnection(player));
            }
        }
        public IEnumerable<Road> canBuildRoad
        {
            get
            {
                return availableRoads
                    .Where(r => Bank.hasPayment(player, Bank.ROAD_COST));
            }
        }
        public Button endTurnButton { get; private set; }
        public IDictionary<Board.ResourceType, int> playerResourcesAmounts { get; private set; }
        public IDictionary<Board.ResourceType, int> bankTradePrices { get; private set; }
        public IDictionary<Board.ResourceType, bool> resourcesAvailableToBuy
        {
            get
            {
                return playerResourcesAmounts
                    .ToDictionary(k => k.Key, v => v.Value >= bankTradePrices[v.Key]);
            }
        }
    }
}
