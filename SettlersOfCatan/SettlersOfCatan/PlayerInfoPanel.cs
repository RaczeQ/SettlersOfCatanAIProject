using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SettlersOfCatan
{
    public partial class Player : UserControl
    {

        public static String[] playerColorNames = { "Red", "Blue", "White", "Orange", "Green", "Brown" };
        public static Color[] playerColors = { Color.Red, Color.Blue, Color.White, Color.Orange, Color.Green, Color.Brown };

        private int playerNumber = 0;
        private List<ResourceCard> resources;
        private bool activePlayer = false;
        private List<DevelopmentCard> onHandDevelopmentCards;


        public Player()
        {
            InitializeComponent();
            resources = new List<ResourceCard>();
            onHandDevelopmentCards = new List<DevelopmentCard>();
        }

        public void setPlayerNumber(int number)
        {
            this.setPlayerColor(playerColors[number]);
            this.setPlayerName(playerColorNames[number]);
            this.playerNumber = number;
        }

        public void giveDevelopmentCard(DevelopmentCard card)
        {
            onHandDevelopmentCards.Add(card);
            updateDevelopmentCards();
        }

        public DevelopmentCard takeDevelopmentCard(DevelopmentCard.DevCardType cardType)
        {
            DevelopmentCard theCard = null;
            foreach (DevelopmentCard c in onHandDevelopmentCards) 
            {
                if (c.getType() == cardType)
                {
                    theCard = c;
                }
            }
            onHandDevelopmentCards.Remove(theCard);
            updateDevelopmentCards();
            return theCard;
        }

        public void updateDevelopmentCards()
        {

            this.pnlDevCards.Controls.Clear();

            int row = 0;
            int col = 0;
            for (int i = 0; i < onHandDevelopmentCards.Count; i++)
            {
                DevelopmentCard devC = onHandDevelopmentCards[i];
                pnlDevCards.Controls.Add(devC);
                devC.Location = new Point(col * devC.Width, row * devC.Height);
                col++;
                if (col > 2)
                {
                    col = 0;
                    row++;
                }
            }
        }

        /**
            Gets the number of the specified resource type.
         */
        public int getResourceCount(Board.ResourceType type)
        {
            int count = 0;
            foreach (ResourceCard res in resources)
            {
                if (res.getResourceType() == type)
                {
                    count++;
                }
            }
            return count;
        }

        /**
            Gets a resource card witht the matching type from the player's
            deck. If no available card exists null is returned.
         */
        public ResourceCard takeResource(Board.ResourceType type)
        {
            ResourceCard rCard = null;
            foreach (ResourceCard res in resources)
            {
                if (res.getResourceType() == type)
                {
                    rCard = res;
                }
            }
            if (rCard != null)
            {
                resources.Remove(rCard);
            }
            updateResourceGUI();
            return rCard;
        }

        public void giveResource(ResourceCard resCard)
        {
            this.resources.Add(resCard);
            updateResourceGUI();
        }

        public Color getPlayerColor()
        {
            return playerColors[playerNumber];
        }

        public int getPlayerNumber()
        {
            return this.playerNumber;
        }

        public String getPlayerName()
        {
            return playerColorNames[this.playerNumber];
        }
        private String wheatCount = "0";
        private String sheepCount = "0";
        private String woodCount = "0";
        private String brickCount = "0";
        private String oreCount = "0";

        bool resourcesHidden = false;

        //Replaces the value with an asterisk
        private void HideResources()
        {
            this.lblBrick.Text = "*";
            this.lblOre.Text = "*";
            this.lblSheep.Text = "*";
            this.lblWheat.Text = "*";
            this.lblWood.Text = "*";
            resourcesHidden = true;
        }

        private void ShowResources()
        {
            this.lblBrick.Text = brickCount;
            this.lblOre.Text = oreCount;
            this.lblSheep.Text = sheepCount;
            this.lblWheat.Text = wheatCount;
            this.lblWood.Text = woodCount;
            resourcesHidden = false;
        }

        public void setPlayerColor(Color color)
        {
            this.pnlPlayerColor.BackColor = color;
        }

        public void setPlayerName(String name)
        {
            this.lblPlayerColorName.Text = name;
        }

        public void setLongestRoad(bool value)
        {
            this.pbLongestRoad.Visible = value;
        }

        public void setLargestArmy(bool value)
        {
            this.pbLargestArmy.Visible = value;
        }

        public void setTurn(bool value)
        {
            this.lblTurn.Visible = value;
            activePlayer = value;
            if (value)
            {
                ShowResources();
            } else
            {
                HideResources();
            }
        }

        private void updateResourceGUI()
        {
            this.setBrick(getResourceCount(Board.ResourceType.Brick));
            this.setOre(getResourceCount(Board.ResourceType.Ore));
            this.setWood(getResourceCount(Board.ResourceType.Wood));
            this.setWheat(getResourceCount(Board.ResourceType.Wheat));
            this.setSheep(getResourceCount(Board.ResourceType.Sheep));
        }

        //Each updates the required values
        private void setWood(int count)
        {
            lblWood.Text = count + "";
            woodCount = count + "";
            if (resourcesHidden)
            {
                lblWood.Text = "*";
            }
        }

        private void setWheat(int count)
        {
            lblWheat.Text = count + "";
            wheatCount = count + "";
            if (resourcesHidden)
            {
                lblWheat.Text = "*";
            }
        }

        private void setSheep(int count)
        {
            lblSheep.Text = count + "";
            sheepCount = count + "";
            if (resourcesHidden)
            {
                lblSheep.Text = "*";
            }
        }

        private void setBrick(int count)
        {
            lblBrick.Text = count + "";
            brickCount = count + "";
            if (resourcesHidden)
            {
                lblBrick.Text = "*";
            }
        }

        private void setOre(int count)
        {
            lblOre.Text = count + "";
            oreCount = count + "";
            if (resourcesHidden)
            {
                lblOre.Text = "*";
            }
        }
    }
}
