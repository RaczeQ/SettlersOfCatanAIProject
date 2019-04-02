using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SettlersOfCatan.AI;

namespace SettlersOfCatan.GameObjects
{
    public partial class Player : UserControl
    {

        public static String[] playerColorNames = { "Red", "Blue", "Purple", "Orange", "Green", "Brown" };
        public static Color[] playerColors = { Color.Red, Color.RoyalBlue, Color.Purple, Color.Orange, Color.Green, Color.Brown };

        public int playerNumber = 0;
        public List<ResourceCard> resources { get; private set; }
        public List<DevelopmentCard> onHandDevelopmentCards { get; private set; }
        public List<Settlement> settlements { get; private set; }
        public List<Road> roads { get; private set; }
        private List<ResourceDisplay> resourceDisplays;
        private Random rand;

        private bool _current = false;
        public bool Current
        {
            get { return _current; }
            set
            {
                _current = value;
                if (_current)
                {
                    showInformation();
                    lblTurn.Visible = true;
                    score = Player.calculateVictoryPoints(true, this);
                }
                else
                {
                    showInformation();
                    lblTurn.Visible = false;
                    score = Player.calculateVictoryPoints(false, this);
                }
            }
        }

        public bool isAI
        {
            get { return playerComboBox.SelectedIndex > 0; }
        }

        public IAgent agent
        {
            get {
                return AgentManager.getAgent(playerComboBox.SelectedItem.ToString());
            }
        }


        private int _score = 0;
        public int score
        {
            get { return _score; }
            set
            {
                _score = value;
                this.lblVictoryPoints.Text = "" + _score;
            }
        }

        public Player(bool initialize=true)
        {
            InitializeComponent();
            resources = new List<ResourceCard>();
            onHandDevelopmentCards = new List<DevelopmentCard>();
            settlements = new List<Settlement>();
            roads = new List<Road>();
            rand = new Random();
            resourceDisplays = new List<ResourceDisplay>();
            playerComboBox.SelectedIndex = 0;

            //Prevents designer glitches because Visual Studio is bitches.
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            if (!designMode)
            {
                for (int i = 0; i < 5; i++)
                {
                    ResourceDisplay resDisp = new ResourceDisplay(initialize);
                    resourceDisplays.Add(resDisp);
                    resDisp.setType((Board.ResourceType)i);
                    resDisp.setCount(0);
                    resDisp.show();
                    pnlResources.Controls.Add(resDisp);
                    resDisp.Location = new Point(i * 34 + 32, 2);
                }
            }
        }

        public void SelectAgent(string agent)
        {
            playerComboBox.SelectedItem = agent;
        }

        public void updateScore()
        {
            score = Player.calculateVictoryPoints(!Current, this);
        }

        public void lockPlayerComboBox()
        {
            this.playerComboBox.Enabled = false;
        }

        /*
            Calculates the number of victory points held by this player.
            If includeVPCards is true the victory point cards will be counted
            toward the points, otherwise they will be ignored.
         */
        public static int calculateVictoryPoints(bool includeVPCards, Player player )
        {
            //Count the settlements and cities
            int val = 0;
            foreach (Settlement set in player.settlements)
            {
                val++;
                if (set.city())
                {
                    val++;
                }
            }
            foreach (DevelopmentCard devCard in player.onHandDevelopmentCards)
            {
                //The played knight cards
                //The victory point cards
                if (devCard.getType() == DevelopmentCard.DevCardType.Victory)
                {
                    if (includeVPCards)
                    {
                        val++;
                    }
                }
            }


            //Longest road
            if (player.pbLargestArmy.Visible == true)
            {
                val+= 2;
            }
            //Largest army
            if (player.pbLongestRoad.Visible == true)
            {
                val+=2;
            }
            return val;
        }

        public int getArmySize()
        {
            int vp = 0;
            foreach (DevelopmentCard devCard in onHandDevelopmentCards)
            {
                //The played knight cards
                //The victory point cards
                if (devCard.getType() == DevelopmentCard.DevCardType.Knight)
                {
                    if (devCard.used)
                    {
                        vp++;
                    }
                }
            }
            return vp;
        }

        public int getLongestRoadCount()
        {
            int longest = 0;
            //Checks the longest road for each road the player owns.
            //The length of the road can change depending on where you start from.
            foreach (Road road in roads)
            {
                int len = roadLength(road, new List<Road>(), 0);
                longest = Math.Max(len, longest);
            }

            return longest;
        }

        private int roadLength(Road road, List<Road> countedRoads, int position)
        {
            countedRoads.Add(road);
            int pos = position + 1;
            int maximum = 0;
            foreach (Settlement settlement in road.getConnectedSettlements())
            {
                if (settlement.getOwningPlayer() == this || settlement.getOwningPlayer() == null)
                {
                    List<Road> rds = settlement.getConnectedRoads();
                    foreach (Road rd in rds)
                    {
                        if (rd.getOwningPlayer() == this)
                        {
                            if (!countedRoads.Contains(rd))
                            {
                                maximum = roadLength(rd, countedRoads, pos);
                            }
                        }
                    }
                }
            }
            return Math.Max(pos,maximum);
        }

        public void addSettlement(Settlement s)
        {
            this.settlements.Add(s);
        }

        public void addRoad(Road r)
        {
            this.roads.Add(r);
        }

        public int getSettlementCount()
        {
            return this.settlements.Count;
        }

        public int getRoadCount()
        {
            return this.roads.Count;
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

        public List<DevelopmentCard> getDevelopmentCards()
        {
            return this.onHandDevelopmentCards;
        }

        private void updateDevelopmentCards()
        {

            this.pnlDevCards.Controls.Clear();

            int row = 0;
            int col = 0;
            for (int i = 0; i < onHandDevelopmentCards.Count; i++)
            {
                DevelopmentCard devC = onHandDevelopmentCards[i];
                pnlDevCards.Controls.Add(devC);
                float newX = pnlDevCards.Width / 2 - 12;
                float newY = (DevelopmentCard.devCardSize.Height * newX) / DevelopmentCard.devCardSize.Width;
                devC.Size = new Size((int)newX, (int)newY);
                devC.Location = new Point(col * devC.Width, row * devC.Height);
                col++;
                if (col > 1)
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

        public int getTotalResourceCount()
        {
            return resources.Count();
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
            updateResourceDisplays();
            return rCard;
        }

        public void giveResource(ResourceCard resCard)
        {
            this.resources.Add(resCard);
            updateResourceDisplays();
        }

        public ResourceCard takeRandomResource()
        {
            ResourceCard rCard = null;
            if (resources.Count > 0)
            {
                int num = rand.Next(0, resources.Count);
                rCard = resources[num];
                resources.Remove(rCard);
            } else
            {
                return null;
            }
            updateResourceDisplays();
            return rCard;
        }

        public Color getColor()
        {
            return playerColors[playerNumber];
        }

        public int getNumber()
        {
            return this.playerNumber;
        }

        public String getName()
        {
            return $"{playerColorNames[this.playerNumber]} [{playerComboBox.SelectedItem.ToString()}]";
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

        public void updateResourceDisplays()
        {
            for (int i = 0; i < 5; i ++)
            {
                Board.ResourceType rType = (Board.ResourceType)i;
                resourceDisplays[i].setCount(this.getResourceCount(rType));
            }
        }

        private void hideInformation()
        {
            foreach (ResourceDisplay rd in resourceDisplays)
            {
                rd.hide();
            }
            foreach (DevelopmentCard dc in onHandDevelopmentCards)
            {
                dc.hide();
            }
        }

        private void showInformation()
        {
            foreach (ResourceDisplay rd in resourceDisplays)
            {
                rd.show();
            }
            foreach (DevelopmentCard dc in onHandDevelopmentCards)
            {
                dc.show();
            }
        }

        private void Player_Name_Clicked(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType()) return false;

            Player p = (Player)obj;
            return this.playerNumber == p.playerNumber;
        }
    }
}
