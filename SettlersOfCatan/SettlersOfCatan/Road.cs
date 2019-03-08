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
    public class Road : PictureBox
    {
        public int id = 0;
        public Point position;

        public List<Settlement> connectedSettlements { get; protected set; } = new List<Settlement>();

        public Player owningPlayer { get; private set; }

        public Road(Point position, int index)
        {
            this.position = position;
            BackColor = Color.Transparent;
            BackgroundImageLayout = ImageLayout.Stretch;
            Location = new Point(position.X - 6, position.Y - 6);
            Size = new Size(12, 12);
            this.Paint += Road_Paint;
            this.Click += Road_Click;

        }

        /*
         * Causes the control to redraw.
         */
        private void Road_Click(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Road_Paint(object sender, PaintEventArgs e)
        {
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = BackgroundImage.Width;
            int height = BackgroundImage.Height;
            Color c = Color.Bisque;
            if (owningPlayer != null)
            {
                c = owningPlayer.getColor();
            }

            float r = ((255.0f - c.R + 0.0f) / 255.0f);
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
               BackgroundImage,
               new Rectangle(0, 0, width, height),  // destination rectangle 
               0, 0,        // upper-left corner of source rectangle 
               width,       // width of source rectangle
               height,      // height of source rectangle
               GraphicsUnit.Pixel,
               imageAttributes);
        }

        public Player getOwningPlayer()
        {
            return this.owningPlayer;
        }

        public String toString()
        {
            String str = "Road ID: " + this.id + " Connected Settlements: (";
            foreach (Settlement r in connectedSettlements)
            {
                str += "Settlement ID: " + r.id + ", ";
            }
            str += " )";
            return str;
        }


        /**
        Returns true if the current road is already linked.
         */
        public bool linkSettlement(Settlement set)
        {
            foreach (Settlement currentSettlement in connectedSettlements)
            {
                if (set.id == currentSettlement.id)
                {
                    return true;
                }
            }
            connectedSettlements.Add(set);
            return false;
        }

        public List<Settlement> getConnectedSettlements()
        {
            return this.connectedSettlements;
        }

        /**
            This algorithm is for checking if the player is able to build a road.
            Conditions for true:
                A- There must be a settlement owned by the player directly adjascent.
                B- There must be a road directly connected to this one owned by the player, but not blocked by another player.
         */
        public bool checkForConnection(Player currentPlayer)
        {

            bool result = false;

            foreach (Settlement set in connectedSettlements)
            {
                Player setPlayer = set.getOwningPlayer();
                if (setPlayer == currentPlayer)
                {
                    return true; //Always return true.
                    //If there is a settlement directly next to a road owned by the same color player we
                    //can just go ahead and place it.
                }
                else
                {
                    List<Road> connectedRoads = set.getConnectedRoads();
                    foreach (Road r in connectedRoads) {
                        if (r != this) //Excluding this road.
                        {
                            if (setPlayer == null)
                            {
                                if (r.getOwningPlayer() == currentPlayer)
                                {
                                    result = true;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        /**
            Add condition:
                Must have either a road or a settlement with matching player.
         */
        public void buildRoad(Player currentPlayer, bool takeResources)
        {

            if (owningPlayer != null)
            {
                throw new BuildError(BuildError.LocationOwnedBy(owningPlayer));
            }

            if (takeResources && !Bank.hasPayment(currentPlayer, Bank.ROAD_COST))
            {
                throw new BuildError(BuildError.NOT_ENOUGH_RESOURCES);
            }

            if (!checkForConnection(currentPlayer))
            {
                throw new BuildError(BuildError.NO_CONNECTED_ROAD);
            }


            this.owningPlayer = currentPlayer;
            //this.BackColor = currentPlayer.getPlayerColor();
            currentPlayer.addRoad(this);
            this.Refresh();
        }
    }
}
