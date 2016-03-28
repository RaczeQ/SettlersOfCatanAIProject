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

using System.Reflection;
using System.IO;

namespace SettlersOfCatan
{
    public partial class Board : Form
    {
        public enum ResourceType { Wood=0, Brick, Ore, Wheat, Sheep, Desert};
        public String[] RESOURCE_NAMES = { "Wood", "Brick", "Ore", "Wheat", "Sheep", "NoResource" };
        public static String[] TILE_NAMES = { "Forest", "Hills", "Mountains", "Farms", "Fields", "Desert" };
        public Bitmap[] tileImages = new Bitmap[6];
        public String[] tileImageResourceNames = { "Forest_Tile.png", "Hills_Tile.png", "Mountain_Tile.png", "Wheat_Fields_Tile.png", "Pasture_Tile.png", "Desert_Tile.png" };

        //Keeps track of what tile indexes are ocean borders for later use.
        int[] oceanBorderInds = { 0, 1, 2, 3, 4, 8, 9, 14, 15, 21, 22, 27, 28, 32, 33, 34, 35, 36 };

        String[] tileFileNames = { "Rock.png", "Wood.png" };
        String oceanBorderFileName = "Ocean.png";

        Random rand = new Random();
        Tile[] boardTiles = new Tile[BOARD_TILE_COUNT];
        List<Road> roadLocations = new List<Road>();
        List<Settlement> settlementLocations = new List<Settlement>();

        public Board.ResourceType[] fourPlayerTiles = {
            Board.ResourceType.Ore, Board.ResourceType.Ore, Board.ResourceType.Ore,
            Board.ResourceType.Sheep, Board.ResourceType.Sheep, Board.ResourceType.Sheep, Board.ResourceType.Sheep,
            Board.ResourceType.Wood, Board.ResourceType.Wood, Board.ResourceType.Wood, Board.ResourceType.Wood,
            Board.ResourceType.Wheat, Board.ResourceType.Wheat, Board.ResourceType.Wheat, Board.ResourceType.Wheat,
            Board.ResourceType.Brick, Board.ResourceType.Brick, Board.ResourceType.Brick, Board.ResourceType.Desert };

        public Deck terrainTiles;



        public static int BOARD_TILE_COUNT = 37;
        public static int SPACING = 128; //I require a 1:1 aspect ratio in order for FAR more simple positioning.
        //This variable is used to determine how many pixels tall the triangle of a terrain tile is.
        public static int TILE_TRIANGLE_HEIGHT = 35;

        public Board()
        {
            InitializeComponent();

            this.pnlBoardArea.BackColor = Color.Transparent; //Set the color to transparent. The color is white by default so it is visible in the editor.
            this.pbBuildDevelopmentCard.MouseEnter += showDevelopmentCardToolTip;
            this.pbBuildDevelopmentCard.MouseLeave += hideDevelopmentCardToolTip;

            //Load up the tile image resources .
            for (int i = 0; i < 6; i++)
            {
                tileImages[i] = new Bitmap("Resources/" + tileImageResourceNames[i]);
            }

            BoardSetup();
        }
        /**
            Checks if the points are equal.
         */
        public bool LocationsEqual(Point p1, Point p2)
        {
            return (p1.X==p2.X)&&(p1.Y==p2.Y);
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

        public void BoardSetup()
        {

            //Sets up the terrain tiles deck
            terrainTiles = new Deck(19);
            foreach (Board.ResourceType r in fourPlayerTiles)
            {
                terrainTiles.putCard(new TerrainTile(pnlBoardArea, r, this.tileImages[(int)r]));

            }

            terrainTiles.shuffleDeck();
            //

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
                int gatherChance = rand.Next(1, 12);
                for (int f = 0; f < oceanBorderInds.Length; f++)
                {
                    if (i == oceanBorderInds[f])
                    {
                        ocean = true;
                        gatherChance = 0;
                    }
                }

                Point position = new Point(column * SPACING + shift, row * (SPACING - TILE_TRIANGLE_HEIGHT));

                if (!ocean)
                {
                    
                    TerrainTile tile = (TerrainTile)terrainTiles.drawTopCard();
                    tile.Location = position;
                    tile.setGatherChance(gatherChance);
                    tile.index = i;
                    boardTiles[i] = tile;
                    
                } else
                {
                    OceanBorderTile tile = new OceanBorderTile(pnlBoardArea);
                    tile.Location = position;
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

                    for (int ind= 0;ind < 6;ind++)
                    {
                        //We check if this settlement location has already been created.
                        Settlement settlementLocation = findSettlementWithPosition(settlementPoints[ind]);
                        if (settlementLocation == null)
                        {
                            settlementLocation = new Settlement(settlementPoints[ind], i, this.pnlBoardArea);
                            settlementLocations.Add(settlementLocation);
                            settlementLocation.MouseEnter += showSettlementBuildToolTip;
                            settlementLocation.MouseLeave += hideSettlementBuildToolTip;
                            settlementLocation.id = settlementLocations.Count;
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
                            roadLocation = new Road(roadPoint, i, this.pnlBoardArea);
                            roadLocations.Add(roadLocation);
                            roadLocation.MouseEnter += showRoadBuildToolTip;
                            roadLocation.MouseLeave += hideRoadBuildToolTip;
                            roadLocation.id = roadLocations.Count;
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
            Point loc = ((PictureBox)sender).Location;
            loc.X += 32;
            loc.Y += 32;
            this.pnlSettlementToolTip.Location = loc;
            this.pnlSettlementToolTip.Visible = true;
        }

        public void hideSettlementBuildToolTip(object sender, EventArgs s)
        {
            this.pnlSettlementToolTip.Visible = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            Dice d = new Dice();
            ((Button)sender).Text = "" + d.roll();
        }


        public void drawStuff()
        {
            //Testing out getting transparency working?
        }

        private void Board_Load(object sender, EventArgs e)
        {

        }
    }
}
