using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SettlersOfCatan
{
    public partial class Board : Form
    {
        
        public static int BOARD_TILE_COUNT = 37;
        public static int SPACING = 128; //I require a 1:1 aspect ratio in order for FAR more simple positioning.
        //This variable is used to determine how many pixels tall the triangle of a terrain tile is.
        public static int TILE_TRIANGLE_HEIGHT = 35;
        public static Image OUTLINE_IMAGE;

        //Keeps track of what tile indexes are ocean borders for later use.
        int[] oceanBorderInds = { 0, 1, 2, 3, 4, 8, 9, 14, 15, 21, 22, 27, 28, 32, 33, 34, 35, 36 };

        String[] tileFileNames = { "Rock.png", "Wood.png" };
        String oceanBorderFileName = "Ocean.png";

        Random rand = new Random();
        Tile[] boardTiles = new Tile[BOARD_TILE_COUNT];
        

        public Board()
        {
            InitializeComponent();

            OUTLINE_IMAGE = loadImbeddedImage("Resources.Images.outline.png");

            BoardSetup();
        }

        public Image loadImbeddedImage(String pathName)
        {
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream(pathName);
            Image im = new Bitmap(myStream);
            return im;
        }

        public void BoardSetup()
        {
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

                //Check if this tile is an ocean border
                String fName = "";
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
                if (ocean)
                {
                    fName = oceanBorderFileName;
                    numSettlementSpots = 0;
                }
                else {
                    //Pick a random tile from the available tiles list.

                    int ind = rand.Next(0, 2);
                    fName = tileFileNames[ind];
                }
                Point position = new Point(column * SPACING + shift, row * (SPACING - TILE_TRIANGLE_HEIGHT));
                boardTiles[i] = new Tile(pnlBoardArea, 10);
                boardTiles[i].setImage(fName);
                boardTiles[i].setCenter(position);
                boardTiles[i].setGatherChance(gatherChance);
                boardTiles[i].index = i;
                column++;

                //This is for positioning each of the settlement and road location objects. (This should not create any duplicate spots)
                if (!ocean)
                {
                    //Default placement is the top left and top center locations of the tile.
                    //This covers almost all settlement locations.
                    new Settlement(new Point(position.X, position.Y + TILE_TRIANGLE_HEIGHT), i, this.pnlBoardArea, OUTLINE_IMAGE);
                    new Settlement(new Point(position.X + (SPACING / 2), position.Y), i, this.pnlBoardArea, OUTLINE_IMAGE);

                    new Road(new Point(position.X + (SPACING / 4), position.Y + (TILE_TRIANGLE_HEIGHT / 2)), i, this.pnlBoardArea, OUTLINE_IMAGE);
                    new Road(new Point(position.X + (SPACING / 4) * 3, position.Y + (TILE_TRIANGLE_HEIGHT / 2)), i, this.pnlBoardArea, OUTLINE_IMAGE);
                    new Road(new Point(position.X, position.Y + (SPACING / 2)), i, this.pnlBoardArea, OUTLINE_IMAGE);

                    if (i == 7 || i == 13 || i == 20 || i == 26 || i == 31)
                    {
                        new Settlement(new Point(position.X + SPACING, position.Y + TILE_TRIANGLE_HEIGHT), i, this.pnlBoardArea, OUTLINE_IMAGE);
                        new Road(new Point(position.X + SPACING, position.Y + (SPACING / 2)), i, this.pnlBoardArea, OUTLINE_IMAGE);
                        if (i == 20 || i == 26 || i == 31)
                        {
                            new Settlement(new Point(position.X + SPACING, position.Y + SPACING - TILE_TRIANGLE_HEIGHT), i, this.pnlBoardArea, OUTLINE_IMAGE);
                            new Road(new Point(position.X + (SPACING / 4) * 3, position.Y + (SPACING - (TILE_TRIANGLE_HEIGHT / 2))), i, this.pnlBoardArea, OUTLINE_IMAGE);
                        }
                    }
                    if (i == 16 || i == 23 || i == 29 || i == 30 || i == 31)
                    {
                        new Settlement(new Point(position.X, position.Y + SPACING - TILE_TRIANGLE_HEIGHT), i, this.pnlBoardArea, OUTLINE_IMAGE);
                        new Road(new Point(position.X + (SPACING / 4), position.Y + (SPACING - (TILE_TRIANGLE_HEIGHT / 2))), i, this.pnlBoardArea, OUTLINE_IMAGE);
                        if (i == 29 || i == 30 || i == 31)
                        {
                            new Settlement(new Point(position.X + (SPACING / 2), position.Y + SPACING), i, this.pnlBoardArea, OUTLINE_IMAGE);
                        }
                        if (i == 29 || i == 30)
                        {
                            new Road(new Point(position.X + (SPACING / 4) * 3, position.Y + (SPACING - (TILE_TRIANGLE_HEIGHT / 2))), i, this.pnlBoardArea, OUTLINE_IMAGE);
                        }
                    }
                }
            }
        }

    }
}
