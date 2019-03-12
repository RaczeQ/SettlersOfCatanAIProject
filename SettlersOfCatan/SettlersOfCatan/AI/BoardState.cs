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
            _board = b;
        }
        private Board _board { get; set; }
        public Player player { get { return _board.currentPlayer; } }
        public IEnumerable<Tile> tiles { get { return _board.boardTiles; } }
        public IEnumerable<TerrainTile> terrainTiles { get { return tiles.OfType<TerrainTile>(); } }
        public IEnumerable<TerrainTile> playerAdjacentTerrainTiles
        {
            get
            {
                return terrainTiles
                    .Where(t => t.adjascentSettlements
                        .Any(s => s.owningPlayer == player)
                    );
            }
        }
        public IDictionary<TerrainTile, int> playerResourceAcqirementPerTile
        {
            get
            {
                var result = new Dictionary<TerrainTile, int>();
                foreach (TerrainTile tt in playerAdjacentTerrainTiles)
                {
                    result.Add(tt, 0);
                    foreach (Settlement s in tt.adjascentSettlements
                        .Where(s => s.owningPlayer == player))
                    {
                        result[tt] += s.isCity ? 2 : 1;
                    }
                }
                return result;
            }
        }
        public IEnumerable<Settlement> settlements { get { return _board.settlementLocations; } }
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
        public IEnumerable<Road> roads { get { return _board.roadLocations; } }
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
        public Button endTurnButton { get { return _board.btnEndTurn; } }
        public IDictionary<Board.ResourceType, int> playerResourcesAmounts { 
            get
            { 
                return Enum.GetValues(typeof(Board.ResourceType))
                    .Cast<Board.ResourceType>()
                    .ToDictionary(k => k, v => player.getResourceCount(v));
            }
        }
        public IDictionary<Board.ResourceType, int> bankTradePrices { get { return _board.getPlayerBankCosts(player); } }
        public IDictionary<Board.ResourceType, bool> resourcesAvailableToBuy
        {
            get
            {
                return playerResourcesAmounts
                    .ToDictionary(k => k.Key, v => v.Value >= bankTradePrices[v.Key]);
            }
        }
        public IDictionary<Board.ResourceType, int> playerResourcesAcquiredPerResource
        {
            get
            {
                var result = Enum.GetValues(typeof(Board.ResourceType))
                    .Cast<Board.ResourceType>()
                    .ToDictionary(k => k, v => 0);
                foreach (Settlement s in player.settlements)
                {
                    foreach (TerrainTile tt in s.adjacentTiles)
                    {
                        result[tt.getResourceType()] += s.isCity ? 2 : 1;
                    }
                }
                return result;
            }
        }

        public IEnumerable<DevelopmentCard> playerCards
        {
            get
            {
                return player.onHandDevelopmentCards;
            }
        }

        public IEnumerable<DevelopmentCard> playerPlayableCards
        {
            get
            {
                return playerCards.Where(c => c.isPlayable());
            }
        }

        public bool canBuyDevelopmentCard
        {
            get
            {
                return Board.TheBank.developmentCards.cardCount() > 0 && Bank.hasPayment(player, Bank.DEV_CARD_COST);
            }
        }
    }
}
