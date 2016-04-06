using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SettlersOfCatan
{
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

        private bool playerHasRequiredSetResources(Player p)
        {
            return (p.getResourceCount(Board.ResourceType.Brick) > 0 &&
                p.getResourceCount(Board.ResourceType.Wood) > 0 &&
                p.getResourceCount(Board.ResourceType.Wheat) > 0 &&
                p.getResourceCount(Board.ResourceType.Sheep) > 0);
        }

        private bool playerHasRequiredCityResources(Player p)
        {
            return (
                p.getResourceCount(Board.ResourceType.Wheat) > 1 &&
                p.getResourceCount(Board.ResourceType.Ore) > 2
                );
        }
        /*
            Conditions must apply: 
            no other player's settlement must be present at this location or, within 1 road's distance.
                if a settlement exists player must have required resources: 
            must have required resources: Brick, Wood, Wheat, Sheep

         */
        public bool buildSettlement(Player currentPlayer, bool takeResources)
        {
            if (owningPlayer == null)
            {
                //Build settlement
                //Check if there is another city within 1 spot!+
                if (playerHasRequiredSetResources(currentPlayer) && takeResources || !takeResources)
                {
                    if (checkForOtherSettlement())
                    {
                        this.owningPlayer = currentPlayer;
                        this.BackColor = owningPlayer.getPlayerColor();
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("You cannot build here. This location is too close to another settlement.");
                        return false;
                    }
                } else
                {
                    MessageBox.Show("You do not have the required resources to build a settlement.");
                    return false;
                }
            } else
            {
                //Upgrade the settlement to city
                if (currentPlayer == owningPlayer)
                {
                    //We upgrade
                    if (takeResources)
                    {
                        //Upgrade the settlement
                        if (!isCity)
                        {
                            if (playerHasRequiredCityResources(currentPlayer))
                            {
                                this.isCity = true;
                                return true;
                            } else
                            {
                                MessageBox.Show("You do not have the required resources to upgrade this settlement.");
                                return false;
                            }
                        } else
                        {
                            MessageBox.Show("You cannot upgrade a city any further.");
                            return false;
                        }
                    } else
                    {
                        //Do not upgrade the settlement
                        MessageBox.Show("You cannot build a settlement on top of another settlement.");
                        return false;
                    }
                    
                } else
                {
                    //We don't upgrade
                    MessageBox.Show("Player " + owningPlayer.getPlayerName() + " already has a settlement here.");
                    return false;
                }
            }
        }
    }
}
