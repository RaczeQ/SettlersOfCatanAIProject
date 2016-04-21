using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace SettlersOfCatan
{
    [Serializable]
    public class Settlement : PictureBox
    {
        public int id = 0;
        public Point position;

        private List<Road> connectedRoads=new List<Road>();

        private Player owningPlayer;
        private bool isCity = false;
        private Bitmap image = new Bitmap("Resources/settlement.png");


        public Settlement(Point position, int index)
        {
            this.Paint += Settlement_Paint;
            this.position = position;
            BackColor = Color.Transparent;
            BackgroundImageLayout = ImageLayout.Stretch;
            Location = new Point(position.X - 6, position.Y - 6);
            Size = new Size(12, 12);
            Click += forcePaint;
        }

        private void forcePaint(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Settlement_Paint(object sender, PaintEventArgs e)
        {
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = image.Width;
            int height = image.Height;
            Color c = Color.Bisque;
            if (owningPlayer != null)
            {
                c = owningPlayer.getColor();
            }

            float r = ((255.0f - c.R+0.0f) / 255.0f);
            float g = ((255.0f - c.G) / 255.0f);
            float b = ((255.0f - c.B) / 255.0f);


            float[][] colorMatrixElements = {
               new float[] {1, 0, 0, 0, 0},
               new float[] {0, 1, 0, 0, 0},
               new float[] {0, 0, 1, 0, 0},
               new float[] {0, 0, 0, 1, 0},
               new float[] {-r, -g, -b, 0, 1}
            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);

            e.Graphics.DrawImage(
               image,
               new Rectangle(0, 0, width, height),  // destination rectangle 
               0, 0,        // upper-left corner of source rectangle 
               width,       // width of source rectangle
               height,      // height of source rectangle
               GraphicsUnit.Pixel,
               imageAttributes);
        }

        public String toString()
        {
            String str = "Settlement ID: " + this.id + " Connected Roads: (";
            foreach (Road r in connectedRoads)
            {
                str += "Road ID: " + r.id + ", "; 
            }
            str += " )";
            return str;
        }

        public Player getOwningPlayer()
        {
            return this.owningPlayer;
        }


        public List<Road> getConnectedRoads()
        {
            return connectedRoads;
        }

        /**
            Returns true if the current road is already linked.
          */
        public bool linkRoad(Road rd)
        {
            foreach (Road currentRoad in connectedRoads)
            {
                if (rd.id == currentRoad.id)
                {
                    return true;
                }
            }
            connectedRoads.Add(rd);
            return false;
        }

        private bool upgrade(Player currentPlayer)
        {
            return false;
        }

        public bool city()
        {
            return this.isCity;
        }

        public void setOwningPlayer(Player pl)
        {
            //Recolor the image and stuff...
            this.owningPlayer = pl;
            
        }

        /**
            Only returns true if no settlement is within 1 location of this settlement.
         */
        private bool checkForOtherSettlement()
        {
            bool allow = true;
            foreach (Road cRoad in connectedRoads)
            {
                foreach(Settlement cSet in cRoad.getConnectedSettlements())
                {
                    if (cSet != this)
                    {
                        if (cSet.owningPlayer != null)
                        {

                            //MessageBox.Show("There is no player at the location!")
                            allow = false;
                        }
                    }
                }
            }
            return allow;
        }

        private bool checkForConnection(Player currentPlayer)
        {
            bool result = false;
            //We need to check if there is a valid road connection
            foreach (Road r in connectedRoads)
            {
                if (r.getOwningPlayer() == currentPlayer)
                {
                    result = true;
                }
            }
            return result;
        }

        /*
            Conditions must apply: 
            no other player's settlement must be present at this location or, within 1 road's distance.
                if a settlement exists player must have required resources: 
            must have required resources: Brick, Wood, Wheat, Sheep

         */
        public void buildSettlement(Player currentPlayer, bool takeResources, bool connectionCheck)
        {

            if (owningPlayer == null)
            {
                if (!Bank.hasPayment(currentPlayer, Bank.SETTLEMENT_COST) && takeResources)
                {
                    throw new BuildError(BuildError.NOT_ENOUGH_RESOURCES);
                }
                if (!checkForOtherSettlement())
                {
                    throw new BuildError(BuildError.SETTLEMENT_TOO_CLOSE);
                }

                if (connectionCheck && !checkForConnection(currentPlayer))
                {
                    throw new BuildError(BuildError.NO_CONNECTION_SETTLEMENT);
                }

                setOwningPlayer(currentPlayer);
                currentPlayer.addSettlement(this);
            }
            else if (owningPlayer == currentPlayer)
            {
                if (!Bank.hasPayment(currentPlayer, Bank.CITY_COST))
                {
                    throw new BuildError(BuildError.NOT_ENOUGH_RESOURCES);
                }

                if (isCity)
                {
                    throw new BuildError(BuildError.IS_CITY);
                }

                this.isCity = true;
                this.image = new Bitmap("Resources/city.png");
                this.Invalidate();
            } else
            {
                throw new BuildError(BuildError.LocationOwnedBy(owningPlayer));
            }
        }
    }
}
