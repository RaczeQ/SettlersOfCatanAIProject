using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SettlersOfCatan.AI
{
    public class BoardState
    {
        public static readonly double SETTLEMENT_VALUE = 3;
        public static readonly double CITY_VALUE = 5;
        public static readonly double ROAD_VALUE = 1;
        public static readonly double VICTORY_POINT_MULTIPLIER = 100;
        public static readonly double LONGEST_ROAD_MULTIPLIER = 1.5;
        public static readonly Dictionary<int, double> CHIP_MULTIPLIERS = new Dictionary<int, double>()
        {
            { 6, 5 }, {  8, 5 },
            { 5, 4 }, {  9, 4 },
            { 4, 3 }, { 10, 3 },
            { 3, 2 }, { 11, 2 },
            { 2, 1 }, { 12, 1 },
        };
        public BoardState(Board b)
        {
            _board = b;
        }
        private Board _board { get; set; }
        public Player player { get { return _board.currentPlayer; } }
        public double score { get
            { 
                double victory_points_score = Player.calculateVictoryPoints(true, player) * VICTORY_POINT_MULTIPLIER;
                double roads_score = 
                    player.roads.Count * ROAD_VALUE + 
                    player.getLongestRoadCount() * (player == _board.getPlayerWithLongestRoad() ? LONGEST_ROAD_MULTIPLIER : 0);
                double settlements_score = 0;
                double resources_score = 0;
                foreach (Settlement s in player.settlements) {
                    settlements_score += s.isCity ? CITY_VALUE : SETTLEMENT_VALUE;
                    foreach (TerrainTile tt in s.adjacentTiles) {
                        if (tt.getResourceType() != Board.ResourceType.Desert) {
                            resources_score += CHIP_MULTIPLIERS[tt.numberChip.numberValue] * (s.isCity ? 2 : 1);
                        }
                    }
                }
                return victory_points_score + roads_score + settlements_score + resources_score;
            }
        }
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
        public IDictionary<Board.ResourceType, bool> resourcesAvailableToSell
        {
            get
            {
                return playerResourcesAmounts
                    .ToDictionary(k => k.Key, v => v.Value >= bankTradePrices[v.Key]);
            }
        }
        public IDictionary<Board.ResourceType, bool> resourcesAvailableToBuy
        {
            get
            {
                return playerResourcesAmounts
                    .ToDictionary(k => k.Key, v => Board.TheBank.canGiveOutResource(v.Key, 1));
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
