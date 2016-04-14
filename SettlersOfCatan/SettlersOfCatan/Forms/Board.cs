using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SettlersOfCatan.Events;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace SettlersOfCatan
{
    public partial class Board : Form, EvtOwnr
    {
        public static Bank TheBank = new Bank();

        public enum ResourceType { Wood=0, Brick, Ore, Wheat, Sheep, Desert};
        public static String[] RESOURCE_NAMES = { "Wood", "Brick", "Ore", "Wheat", "Sheep", "NoResource" };
        public static String[] TILE_NAMES = { "Forest", "Hills", "Mountains", "Farms", "Fields", "Desert" };
        public Bitmap[] tileImages = new Bitmap[6];
        public String[] tileImageResourceNames = { "Forest_Tile.png", "Hills_Tile.png", "Mountain_Tile.png", "Wheat_Fields_Tile.png", "Pasture_Tile.png", "Desert_Tile.png" };
        public static String[] iconImageResourceNames = { "Wood_Icon.png", "Brick_Icon.png", "Ore_Icon.png", "Wheat_Icon.png", "Sheep_Icon.png", "No_Resource_Icon.png" };
        //Keeps track of what tile indexes are ocean borders for later use.
        private Random rand = new Random();
        public Tile[] boardTiles = new Tile[BOARD_TILE_COUNT];
        public List<Road> roadLocations = new List<Road>();
        public List<Settlement> settlementLocations = new List<Settlement>();
        public List<Harbor> harbors = new List<Harbor>();

        //This is the distribution of terrain resources for a four player game.
        public static Board.ResourceType[] fourPlayerTiles = 
            {
                Board.ResourceType.Ore, Board.ResourceType.Ore, Board.ResourceType.Ore,
                Board.ResourceType.Sheep, Board.ResourceType.Sheep, Board.ResourceType.Sheep, Board.ResourceType.Sheep,
                Board.ResourceType.Wood, Board.ResourceType.Wood, Board.ResourceType.Wood, Board.ResourceType.Wood,
                Board.ResourceType.Wheat, Board.ResourceType.Wheat, Board.ResourceType.Wheat, Board.ResourceType.Wheat,
                Board.ResourceType.Brick, Board.ResourceType.Brick, Board.ResourceType.Brick, Board.ResourceType.Desert
            };


        private int[] oceanBorderIndsFourPlayer = { 0, 1, 2, 3, 4, 8, 9, 14, 15, 21, 22, 27, 28, 32, 33, 34, 35, 36 };

        public static int[] FourPlayerHarborLocations =         { 2, 0, 8, 32, 33, 35, 22, 21, 9 };
        public static int[] FourPlayerHarborRequiredResources = { 2, 3, 3, 2, 3 , 2 , 2 , 3 , 2 };
        public static String[] FourPlayerHarborResourceNames = { "Harbor_2-1-Wool.png", "Harbor_3-1-X.png", "Harbor_3-1-X.png", "Harbor_2-1-Brick.png",
            "Harbor_3-1-X.png", "Harbor_2-1-Wood.png", "Harbor_2-1-Wheat.png", "Harbor_3-1-X.png", "Harbor_2-1-Ore.png" };
        public static Board.ResourceType[] FourPlayerHarborOutputResources =
            {
                Board.ResourceType.Sheep, Board.ResourceType.Desert, Board.ResourceType.Desert,
                Board.ResourceType.Brick, Board.ResourceType.Desert, Board.ResourceType.Wood, Board.ResourceType.Wheat,
                Board.ResourceType.Desert, Board.ResourceType.Ore
            };

        //This is the correctly ordered number chip distribution for a four player game.
        public int[] fourPlayerNumberChips = { 9,12,11,10,6,11,4,8,5,3,8,3,4,9,10,6,2,5};
        public Deck terrainTiles;
        public Deck numberChips;
        public static int BOARD_TILE_COUNT = 37;
        public static int SPACING = 128; //I require a 1:1 aspect ratio in order for FAR more simple positioning.
        //This variable is used to determine how many pixels tall the triangle of a terrain tile is.
        public static int TILE_TRIANGLE_HEIGHT = 35;

        public Player[] playerPanels; //A list of the player panels in the original order.
        public Player[] playerOrder; //The players in the order of first player.
        public int turnCounter = 0;
        public Player firstPlayer;
        public Player currentPlayer;
        public enum GameState { Setup, FirstDiceRoll, FirstSettlement, FirstResources, DiceRoll, PlayerTurn };
        public static Event currentGameEvent;
        public GameState currentGameState;


        /*
            Thief movement options.
         */
        public static bool THIEF_MUST_MOVE = true;
        public static bool THIEF_CANNOT_GO_HOME = true; //(back to the desert)

        /*
            Board setup options.
         */
        public static bool RANDOM_NUMBER_CHITS = false;

        public Board()
        {
            InitializeComponent();

            this.pnlBoardArea.BackColor = Color.Transparent; //Set the color to transparent. The color is white by default so it is visible in the editor.

            //Load up the tile image resources .
            for (int i = 0; i < 6; i++)
            {
                tileImages[i] = new Bitmap("Resources/" + tileImageResourceNames[i]);
            }
            //Set up player objects and initial player order
            playerOrder = new Player[4];
            playerPanels = new Player[4];
            playerPanels[0] = playerInfoPanel1;
            playerPanels[1] = playerInfoPanel2;
            playerPanels[2] = playerInfoPanel3;
            playerPanels[3] = playerInfoPanel4;
            for (int i = 0; i < playerPanels.Count(); i ++)
            {
                playerPanels[i].setPlayerNumber(i);
            }
            currentPlayer = playerPanels[0];

            addEventText("Welcome to catan. When you are ready, click on Set Up Board to begin.");

        }

        //Runs when an event has sucessfully resolved.
        public void subeventEnded()
        {
            switch (this.currentGameState)
            {
                case GameState.FirstDiceRoll:
                    //Move to the next stage

                    //Re order the players so the firstPlayer is the first item in the playerOrder list.
                    int fp = firstPlayer.getPlayerNumber();
                    for (int i = 0; i < playerOrder.Count(); i ++ )
                    {
                        playerOrder[i] = playerPanels[fp];
                        fp++;
                        if (fp == playerPanels.Count())
                        {
                            fp = 0;
                        }
                    }
                    currentGameState = GameState.FirstSettlement;
                    currentGameEvent = new FirstSettlementEvt();
                    currentGameEvent.beginExecution(this ,this);
                    break;
                case GameState.FirstSettlement:
                    //We are finished placing settlements
                    currentGameState = GameState.FirstResources;
                    currentGameEvent = new FirstResourcesEvt();
                    currentGameEvent.beginExecution(this, this);
                    break;
                case GameState.FirstResources:
                    //We can finally move to the gameplay loop.
                    currentPlayer = firstPlayer;
                    addEventText(UserMessages.PlayerDiceRollPrompt(currentPlayer));
                    currentGameState = GameState.DiceRoll;
                    currentGameEvent = new DiceRollEvt();
                    currentGameEvent.beginExecution(this, this);
                    break;
                case GameState.DiceRoll:
                    //Run the player turn event.
                    currentGameState = GameState.PlayerTurn;
                    currentGameEvent = new PlayerTurnEvt();
                    currentGameEvent.beginExecution(this, this);
                    break;
                case GameState.PlayerTurn:
                    //Increment to the next player and start the dice roll event.
                    turnCounter = (turnCounter == playerPanels.Count() - 1) ? 0 : turnCounter + 1;
                    currentPlayer = playerOrder[turnCounter];
                    addEventText(UserMessages.PlayerDiceRollPrompt(currentPlayer));
                    currentGameState = GameState.DiceRoll;
                    currentGameEvent = new DiceRollEvt();
                    currentGameEvent.beginExecution(this, this);
                    break;
            }
        }

        /*
            Creates the data structures used for the board's tiles.
         */
        public void distributeTiles()
        {

            //Sets up the terrain tiles deck
            terrainTiles = new Deck(19);
            foreach (Board.ResourceType r in fourPlayerTiles)
            {
                terrainTiles.putCard(new TerrainTile(pnlBoardArea, r, this.tileImages[(int)r]));

            }

            terrainTiles.shuffleDeck();

            //Set up the number chip deck. (can be randomized, however, the default is to keep the correct order).
            numberChips = new Deck(18);
            foreach (int num in fourPlayerNumberChips)
            {
                numberChips.putCard(new NumberChip(num));
            }

            /*
                If random number chip order is marked, randomize the number chips deck.
                    * Replace x with proper condition *
                if (x) {
                    numberChips.shuffleDeck();
                }
            */

            int row = 0;
            int column = 0;

            //This creates and positions each tile in the world.
            for (int i = 0; i < BOARD_TILE_COUNT; i++)
            {

                int numSettlementSpots = 2; //By default each tile creates 2 settlement spots

                //This checks for the end of a row on specific indices for correct row breaks.
                if (i == 4 || i == 9 || i == 15 || i == 22 || i == 28 || i == 33)
                {
                    row += 1;
                    column = 0;
                    //We need to create 3 settlement spots
                }
                //Shift is used to keep track of the row offsets for the correct "meshing" of the tiles.
                int shift = 0;
                //This checks for every other row and applies a half tile shift if it is an odd numbered row.
                if (row % 2 != 0)
                {
                    shift += SPACING / 2;
                }
                //This shifts the specified row indices by whole tiles for the correct look.
                switch (row)
                {
                    case 0:
                        shift += SPACING * 2;
                        break;
                    case 1:
                        shift += SPACING * 1;
                        break;
                    case 2:
                        shift += SPACING * 1;
                        break;
                    case 4:
                        shift += SPACING * 1;
                        break;
                    case 5:
                        shift += SPACING * 1;
                        //We need to create even more settlement spots (+4)
                        numSettlementSpots++;
                        //if (i==)
                        break;
                    case 6:
                        shift += SPACING * 2;
                        break;
                }

                //Determine if this tile is an ocean border.
                bool ocean = false;
                for (int f = 0; f < oceanBorderIndsFourPlayer.Length; f++)
                {
                    if (i == oceanBorderIndsFourPlayer[f])
                    {
                        ocean = true;
                    }
                }

                Point position = new Point(column * SPACING + shift, row * (SPACING - TILE_TRIANGLE_HEIGHT));

                if (!ocean)
                {
                    
                    TerrainTile tile = (TerrainTile)terrainTiles.drawTopCard();
                    tile.setPosition(position);
                    tile.index = i;
                    boardTiles[i] = tile;
                    if (tile.getResourceType() == ResourceType.Desert)
                    {
                        //Create a number chip with a value of 0
                        tile.setNumberChip(new NumberChip(0));
                        tile.placeThief();
                    } else
                    {
                        //Get the next number chip in line.
                        tile.setNumberChip((NumberChip)numberChips.drawTopCard());
                    }
                    
                } else
                {
                    OceanBorderTile tile = new OceanBorderTile(pnlBoardArea);
                    tile.setPosition(position);
                    tile.index = i;
                    boardTiles[i] = tile;
                }

                column++;

                /*
                    The following code block takes care of creating, linking, and positioning each settlement and road for the game.
                    This ensures there are no duplicate locations.

                */
                if (!ocean)
                {
                    TerrainTile t = (TerrainTile)boardTiles[i];
                    //Default placement is the top left and top center locations of the tile.
                    //This covers almost all settlement locations.

                    Point[] settlementPoints = new Point[6];                 
                    //A list of positions for each possible settlement location on this tile. The order matters greatly. (In a clockwise direction)
                    settlementPoints[0] = new Point(position.X, position.Y + TILE_TRIANGLE_HEIGHT);
                    settlementPoints[1] = new Point(position.X + (SPACING / 2), position.Y);
                    settlementPoints[2] = new Point(position.X + SPACING, position.Y + TILE_TRIANGLE_HEIGHT);
                    settlementPoints[3] = new Point(position.X + SPACING, position.Y + SPACING - TILE_TRIANGLE_HEIGHT);
                    settlementPoints[4] = new Point(position.X + (SPACING / 2), position.Y + SPACING);
                    settlementPoints[5] = new Point(position.X, position.Y + SPACING - TILE_TRIANGLE_HEIGHT);

                    foreach (Point setPoint in settlementPoints)
                    {
                        //We check if this settlement location has already been created.
                        Settlement settlementLocation = findSettlementWithPosition(setPoint);
                        //If not already created we need to make a new location
                        if (settlementLocation == null)
                        {
                            settlementLocation = new Settlement(setPoint, 0);
                            settlementLocation.id = settlementLocations.Count;
                            settlementLocations.Add(settlementLocation);
                            pnlBoardArea.Controls.Add(settlementLocation);
                            settlementLocation.BringToFront();
                        }
                        t.adjascentSettlements.Add(settlementLocation);
                    }


                    //This is almost an exact duplicate of the above process. The only difference being the positions.
                    Point[] roadPoints = new Point[6];
                    roadPoints[0] = new Point(position.X, position.Y + (SPACING / 2));
                    roadPoints[1] = new Point(position.X + (SPACING / 4), position.Y + (TILE_TRIANGLE_HEIGHT / 2));
                    roadPoints[2] = new Point(position.X + (SPACING / 4) * 3, position.Y + (TILE_TRIANGLE_HEIGHT / 2));
                    roadPoints[3] = new Point(position.X + SPACING, position.Y + (SPACING / 2));
                    roadPoints[4] = new Point(position.X + (SPACING / 4) * 3, position.Y + (SPACING - (TILE_TRIANGLE_HEIGHT / 2)));
                    roadPoints[5] = new Point(position.X + (SPACING / 4), position.Y + (SPACING - (TILE_TRIANGLE_HEIGHT / 2)));
                    foreach (Point roadPoint in roadPoints)
                    {
                        //We check if this settlement location has already been created.
                        Road roadLocation = findRoadWithPosition(roadPoint);
                        if (roadLocation == null)
                        {
                            roadLocation = new Road(roadPoint, 0);
                            pnlBoardArea.Controls.Add(roadLocation);
                            roadLocation.id = roadLocations.Count;
                            roadLocations.Add(roadLocation);
                            roadLocation.BringToFront();
                        }
                        t.adjascentRoads.Add(roadLocation);
                    }

                    //Next we link the roads and settlements together.
                    //We know the order in which the roads and settlements were created in, so all we need to do is link them in that order.
                    for (int index = 0; index < 6; index++)
                    {
                        t.adjascentRoads[index].linkSettlement(t.adjascentSettlements[index]);
                        //Link settlement to road
                        if (index==0)
                        {   
                            t.adjascentRoads[index].linkSettlement(t.adjascentSettlements[5]);
                        } else
                        {
                            t.adjascentRoads[index].linkSettlement(t.adjascentSettlements[index - 1]);
                        }
                        //Link road to settlement
                        t.adjascentSettlements[index].linkRoad(t.adjascentRoads[index]);
                        if (index==5)
                        {
                            t.adjascentSettlements[index].linkRoad(t.adjascentRoads[0]);
                        }
                        else
                        {
                            t.adjascentSettlements[index].linkRoad(t.adjascentRoads[index + 1]);
                        }
                    }
                }
            }

            //Position each of the harbors
            for (int i = 0; i < FourPlayerHarborLocations.Count(); i++)
            {
                
                        //Create a new harbor using the information from the trade at harborCount location
                 
                OceanBorderTile bt = (OceanBorderTile)this.boardTiles[FourPlayerHarborLocations[i]];
                Harbor h = new Harbor(FourPlayerHarborRequiredResources[i], FourPlayerHarborOutputResources[i]);
                //MessageBox.Show("Resources/" + FourPlayerHarborResourceNames[harborCount]);
                h.BackgroundImage = new Bitmap("Resources/" + FourPlayerHarborResourceNames[i]);
                h.Location = new Point(0, TILE_TRIANGLE_HEIGHT);
                harbors.Add(h);
                bt.setHarbor(h);
                bt.Controls.Add(h);

                //Set the settlements that will be able to use this harbor
                Point position = bt.getPosition();

                Point[] settlementPoints = new Point[6];
                //A list of positions for each possible settlement location on this ocean border
                settlementPoints[0] = new Point(position.X, position.Y + TILE_TRIANGLE_HEIGHT);
                settlementPoints[1] = new Point(position.X + (SPACING / 2), position.Y);
                settlementPoints[2] = new Point(position.X + SPACING, position.Y + TILE_TRIANGLE_HEIGHT);
                settlementPoints[3] = new Point(position.X + SPACING, position.Y + SPACING - TILE_TRIANGLE_HEIGHT);
                settlementPoints[4] = new Point(position.X + (SPACING / 2), position.Y + SPACING);
                settlementPoints[5] = new Point(position.X, position.Y + SPACING - TILE_TRIANGLE_HEIGHT);
                //Look through the list of points to see if any of these positions match exisiting settlement locations
                foreach (Point p in settlementPoints)
                {
                    Settlement set = findSettlementWithPosition(p);
                    if (set != null)
                    {
                        //We have found a valid trade location
                        bt.getHarbor().addTradeLocation(set);
                    }
                }
            }

            debugSaveBoardData();
        }

        public void debugSaveBoardData()
        {
            try
            {
                StreamWriter write = new StreamWriter("datastructure.txt");
                write.WriteLine("Tiles: ");
                foreach (Tile t in boardTiles) 
                {
                    if (t is TerrainTile)
                    {
                        write.WriteLine(t.toString());
                    }
                }
                write.WriteLine("Settlements: ");
                foreach (Settlement s in settlementLocations)
                {
                    write.WriteLine(s.toString());
                }
                write.WriteLine("Roads: ");
                foreach (Road s in roadLocations)
                {
                    write.WriteLine(s.toString());
                }
                write.Close();
            }
            catch (IOException e)
            {
                MessageBox.Show("There was an I/O issue. " + e.Message);
            }
        }

        /**
            Checks if the points are equal.
         */
        public bool LocationsEqual(Point p1, Point p2)
        {
            return (p1.X == p2.X) && (p1.Y == p2.Y);
        }

        /**
            Used to determine if the distance between two points is less than the provided value.
            Uses the distance formula A*A - B*B = C*C (Pythagorean theorem)
         */
        public bool distanceLessThan(Point p1, Point p2, int distance)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y)) < distance;
        }

        /**
            This will look in the list of road locations for a road with a *roughly* matching location and return it.
            We use a distance formula to determine if the position is within our margin of error due to small
            rounding differences between the different road locations. (4 is a bit much for rounding errors)
         */
        public Road findRoadWithPosition(Point pt)
        {
            Road r = null;

            foreach (Road road in roadLocations)
            {
                if (distanceLessThan(pt, road.position, 4))
                {
                    r = road;
                }
            }
            return r;
        }

        /**
           This will look in the list of settlement locations for a settlement with a matching location then return it. 
         */
        public Settlement findSettlementWithPosition(Point pt)
        {
            Settlement s = null;
            foreach (Settlement settlement in settlementLocations)
            {
                //MessageBox.Show(pt.X + " " + pt.Y + settlement.Location.X + " " + settlement.Location.Y);
                if (LocationsEqual(pt, settlement.position))
                {
                    s = settlement;
                }
            }
            return s;
        }

        //Tool tip events.

        public void showRoadBuildToolTip(object sender, EventArgs e)
        {
            Point loc = ((PictureBox)sender).Location;
            loc.X += 32;
            loc.Y += 32;
            this.pnlRoadToolTip.Location = loc;
            this.pnlRoadToolTip.Visible = true;
        }

        public void hideRoadBuildToolTip(object sender, EventArgs e)
        {
            this.pnlRoadToolTip.Visible = false;
        }

        public void showSettlementBuildToolTip(object sender, EventArgs s)
        {
            if (sender is Settlement)
            {
                Settlement set = (Settlement)sender;
                Point loc = ((PictureBox)sender).Location;
                loc.X += 32;
                loc.Y += 32;
                if (set.getOwningPlayer() == null)
                {
                    this.pnlSettlementToolTip.Location = loc;
                    this.pnlSettlementToolTip.Visible = true;
                } else
                {
                    this.pnlCityToolTip.Location = loc;
                    this.pnlCityToolTip.Visible = true;
                }
            }
        }

        public void hideSettlementBuildToolTip(object sender, EventArgs s)
        {
            this.pnlSettlementToolTip.Visible = false;
            this.pnlCityToolTip.Visible = false;
        }


        public void showDevelopmentCardToolTip(object sender, EventArgs s)
        {
            Point loc = ((PictureBox)sender).Parent.Location;
            loc.X += ((PictureBox)sender).Width;
            loc.Y += ((PictureBox)sender).Location.Y;
            this.pnlDevelopmentCardToolTip.Location = loc;
            this.pnlDevelopmentCardToolTip.Visible = true;
        }

        public void hideDevelopmentCardToolTip(object sender, EventArgs s)
        {
            this.pnlDevelopmentCardToolTip.Visible = false;
        }

        public void enableToolTips()
        {
            foreach (Settlement set in this.settlementLocations)
            {
                set.MouseEnter += showSettlementBuildToolTip;
                set.MouseLeave += hideSettlementBuildToolTip;
            }
            foreach (Road road in this.roadLocations)
            {
                road.MouseEnter += showRoadBuildToolTip;
                road.MouseLeave += hideRoadBuildToolTip;
            }
            pbBuildDevelopmentCard.MouseEnter += showDevelopmentCardToolTip;
            pbBuildDevelopmentCard.MouseLeave += hideDevelopmentCardToolTip;
        }

        public void disableToolTips()
        {
            foreach (Settlement set in this.settlementLocations)
            {
                set.MouseEnter -= showSettlementBuildToolTip;
                set.MouseLeave -= hideSettlementBuildToolTip;
            }
            foreach (Road road in this.roadLocations)
            {
                road.MouseEnter -= showRoadBuildToolTip;
                road.MouseLeave -= hideRoadBuildToolTip;
            }
            pbBuildDevelopmentCard.MouseEnter -= showDevelopmentCardToolTip;
            pbBuildDevelopmentCard.MouseLeave -= hideDevelopmentCardToolTip;
        }

        public void checkForWinner()
        {
            //Yay!

            //Update the largest army and longest road stuff
            Player lapl = null;
            int largestArmy = 0;
            Player llpl = null;
            int longestRoad = 0;
            foreach (Player pl in playerOrder)
            {
                int arm = pl.getArmySize();
                if (arm > largestArmy)
                {
                    lapl = pl;
                    largestArmy = arm;
                }

                arm = pl.getLongestRoadCount();
                if (arm > longestRoad)
                {
                    llpl = pl;
                    longestRoad = arm;
                }
                pl.setLongestRoad(false);
                pl.setLargestArmy(false);
                
            }
            if (largestArmy > 3)
            {
                lapl.setLargestArmy(true);
            }
            if (longestRoad > 5)
            {
                llpl.setLongestRoad(true);
            }

            bool winner = false;
            foreach (Player pl in playerOrder)
            {
                int vps = pl.calculateVictoryPoints(true);
                if (vps >= 10)
                {
                    winner = true;
                } else
                {
                    pl.setVictoryPoints(pl.calculateVictoryPoints(false));
                }
            }
            if (winner)
            {
                MessageBox.Show("There is a winner.");
            }
        }

        /**
            Adds the text to the events list box and scrolls it to the bottom, then hides the blue highlight bar.
         */
        public void addEventText(String text)
        {
            this.lstGameEvents.Items.Add(text);
            this.lstGameEvents.SelectedIndex = this.lstGameEvents.Items.Count - 1;
            this.lstGameEvents.SelectedIndex =  -1;
        }

        private void btnSetupBoard_Click(object sender, EventArgs e)
        {
            distributeTiles();
            //debugSaveBoardData();
            this.btnSetupBoard.Hide();
            this.currentGameState = GameState.FirstDiceRoll;
            currentGameEvent = new FirstPlayerEvt();
            currentGameEvent.beginExecution(this, this);
        }

        private void btnCheat_Click(object sender, EventArgs e)
        {
            foreach (Player p in this.playerOrder)
            {
                p.giveResource(new ResourceCard(ResourceType.Brick));
                p.giveResource(new ResourceCard(ResourceType.Brick));
                p.giveResource(new ResourceCard(ResourceType.Brick));
                p.giveResource(new ResourceCard(ResourceType.Sheep));
                p.giveResource(new ResourceCard(ResourceType.Sheep));
                p.giveResource(new ResourceCard(ResourceType.Sheep));
                p.giveResource(new ResourceCard(ResourceType.Wheat));
                p.giveResource(new ResourceCard(ResourceType.Wheat));
                p.giveResource(new ResourceCard(ResourceType.Wheat));
                p.giveResource(new ResourceCard(ResourceType.Wood));
                p.giveResource(new ResourceCard(ResourceType.Wood));
                p.giveResource(new ResourceCard(ResourceType.Wood));
                p.giveResource(new ResourceCard(ResourceType.Ore));
                p.giveResource(new ResourceCard(ResourceType.Ore));
                p.giveResource(new ResourceCard(ResourceType.Ore));
            }
        }
    }
}
