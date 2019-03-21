using SettlersOfCatan.GameObjects;
using SettlersOfCatan.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AutoMapper;
using SettlersOfCatan.Moves;
using SettlersOfCatan.SimplifiedModels;

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

        private List<Road> _roads = new List<Road>();
        private List<Settlement> _settlements = new List<Settlement>();
        private List<Harbor> _harbors = new List<Harbor>();
        private List<TerrainTile> _terrainTiles = new List<TerrainTile>();
        private List<Player> _players = new List<Player>();

        public Bank bank { get; set; }
        public Player player { get; set; }
        public Button endTurnButton { get; set; }

        private void CopyGameState(List<Road> roads, List<Settlement> settlements, List<Harbor> harbors,
            List<TerrainTile> terrainTiles, List<Player> players, Player currentPlayer)
        {
            // Copy every object
            _roads = Mapper.Map<List<Road>>(roads);
            _settlements = Mapper.Map<List<Settlement>>(settlements);
            _harbors = Mapper.Map<List<Harbor>>(harbors);
            _terrainTiles = Mapper.Map<List<TerrainTile>>(terrainTiles);
            _players = Mapper.Map<List<Player>>(players);

            // Find current player
            player = _players[players.IndexOf(currentPlayer)];

            // Repair road references
            for (int idxR = 0; idxR < roads.Count; idxR++)
            {
                // Set owning player
                if (roads[idxR].owningPlayer != null)
                {
                    _roads[idxR].owningPlayer = _players[players.IndexOf(roads[idxR].owningPlayer)];
                }
                // Set connected settlements
                foreach (var settlement in roads[idxR].connectedSettlements)
                {
                    _roads[idxR].connectedSettlements.Add(_settlements[settlements.IndexOf(settlement)]);
                }
            }

            // Repair settlement references
            for (int idxS = 0; idxS < settlements.Count; idxS++)
            {
                // Set owning player
                if (settlements[idxS].owningPlayer != null)
                {
                    _settlements[idxS].owningPlayer = _players[players.IndexOf(settlements[idxS].owningPlayer)];
                }
                // Set connected roads
                foreach (var road in settlements[idxS].connectedRoads)
                {
                    _settlements[idxS].connectedRoads.Add(_roads[roads.IndexOf(road)]);
                }
                // Set adjacent tiles
                foreach (var terrainTile in settlements[idxS].adjacentTiles)
                {
                    _settlements[idxS].adjacentTiles.Add(_terrainTiles[terrainTiles.IndexOf(terrainTile)]);
                }
            }

            // Repair harbor references
            for (int idxH = 0; idxH < harbors.Count; idxH++)
            {
                // Set valid trade locations
                foreach (var settlement in harbors[idxH].validTradeLocations)
                {
                    _harbors[idxH].validTradeLocations.Add(_settlements[settlements.IndexOf(settlement)]);
                }
            }

            // Repair terrain tiles references
            for (int idxT = 0; idxT < terrainTiles.Count; idxT++)
            {
                // Set adjacent roads
                foreach (var road in terrainTiles[idxT].adjascentRoads)
                {
                    _terrainTiles[idxT].adjascentRoads.Add(_roads[roads.IndexOf(road)]);
                }

                // Set adjacent settlements
                foreach (var settlement in terrainTiles[idxT].adjascentSettlements)
                {
                    _terrainTiles[idxT].adjascentSettlements.Add(_settlements[settlements.IndexOf(settlement)]);
                }
            }

            // Repair player references
            for (int idxP = 0; idxP < players.Count; idxP++)
            {
                // Set adjacent roads
                foreach (var road in players[idxP].roads)
                {
                    _players[idxP].roads.Add(_roads[roads.IndexOf(road)]);
                }

                // Set adjacent settlements
                foreach (var settlement in players[idxP].settlements)
                {
                    _players[idxP].settlements.Add(_settlements[settlements.IndexOf(settlement)]);
                }
            }
        }

        public BoardState(Board b)
        {
            endTurnButton = b.btnEndTurn;
            bank = Mapper.Map<Bank>(Board.TheBank);
            CopyGameState(b.roadLocations, b.settlementLocations, b.harbors,
                b.boardTiles.OfType<TerrainTile>().ToList(), b.playerOrder.ToList(), b.currentPlayer);
        }

        public BoardState(BoardState b)
        {
            endTurnButton = b.endTurnButton;
            bank = Mapper.Map<Bank>(b.bank);
            CopyGameState(b._roads, b._settlements, b._harbors, b._terrainTiles, b._players, b.player);
        }

        public BoardState MakeMove(Move m)
        {
            var b = new BoardState(this);
            m.MakeMove(ref b);
            return b;
        }

        public double GetScore { get
            { 
                double victory_points_score = Player.calculateVictoryPoints(true, player) * VICTORY_POINT_MULTIPLIER;
                double roads_score = 
                    player.roads.Count * ROAD_VALUE + 
                    player.getLongestRoadCount() * (player == BoardFunctions.GetPlayerWithLongestRoad(_players) ? LONGEST_ROAD_MULTIPLIER : 0);
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
        public IEnumerable<TerrainTile> terrainTiles { get { return _terrainTiles; } }
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
        public IEnumerable<Settlement> settlements { get { return _settlements; } }
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
        public IEnumerable<Road> roads { get { return _roads; } }
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
        public IDictionary<Board.ResourceType, int> playerResourcesAmounts { 
            get
            { 
                return Enum.GetValues(typeof(Board.ResourceType))
                    .Cast<Board.ResourceType>()
                    .ToDictionary(k => k, v => player.getResourceCount(v));
            }
        }
        public IDictionary<Board.ResourceType, int> bankTradePrices => BoardFunctions.GetPlayerBankCosts(player, _harbors);

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
