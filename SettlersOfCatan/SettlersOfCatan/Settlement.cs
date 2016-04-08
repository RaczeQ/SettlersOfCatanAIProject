using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

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

        public Settlement(Point position, int index)
        {
            this.position = position;
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.Stretch;
            Location = new Point(position.X - 6, position.Y - 6);
            Size = new Size(12, 12);
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

        /*
            Conditions must apply: 
            no other player's settlement must be present at this location or, within 1 road's distance.
                if a settlement exists player must have required resources: 
            must have required resources: Brick, Wood, Wheat, Sheep

         */
        public void buildSettlement(Player currentPlayer, bool takeResources)
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

                this.owningPlayer = currentPlayer;
                this.BackColor = owningPlayer.getPlayerColor();
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
            } else
            {
                throw new BuildError(BuildError.LocationOwnedBy(owningPlayer));
            }
        }
    }
}
