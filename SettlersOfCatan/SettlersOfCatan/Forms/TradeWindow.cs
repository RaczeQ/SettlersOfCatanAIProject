using SettlersOfCatan.Events;
using SettlersOfCatan.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SettlersOfCatan
{
    public partial class TradeWindow : Form
    {

        public List<ResourceSelector> playerResourceSelectors = new List<ResourceSelector>();
        public List<ResourceSelector> otherResourceSelectors = new List<ResourceSelector>();
        public Player initiatingPlayer;
        private bool canClearOther = true;

        private int resCount = 0;

        private int minimumPlayerInput = 0;

        public TradeWindow()
        {
            InitializeComponent();
        }

        /*
            Allow the player to trade any 4 of the same resource for any one other available resource from the bank.
         */
        public void loadBankTrade(Player pl)
        {
            //Bank automatically selects the number of resources on the other party
            initiatingPlayer = pl;

            //Auto selects resources from other party (some trades allows the player to choose the resource)
            //Build the window based on the harbor.
            initiatingPlayer = pl;
            createResourceSelectors(false, false);
            minimumPlayerInput = 4;
            lblTradeName.Text = "Bank";
            lblPlayerTitle.Text = pl.getPlayerName();
            lblPlayerTitle.BackColor = pl.getPlayerColor();
            foreach (ResourceSelector sel in playerResourceSelectors)
            {
                sel.Click += lockedTradePlayerClickPlayerResource;
            }

            foreach (ResourceSelector select in otherResourceSelectors)
            {
                select.Click += playerClickOtherResource;
            }
        }

        /*
            Allow the player to trade with the chosen harbor.
         */
        public void loadHarborTrade(Harbor hb, Player pl)
        {
            //Auto selects resources from other party (some trades allows the player to choose the resource)
            //Build the window based on the harbor.
            initiatingPlayer = pl;
            createResourceSelectors(false, false);
            minimumPlayerInput = hb.getRequiredResourceCount();
            lblTradeName.Text = "Harbor";
            lblPlayerTitle.Text = pl.getPlayerName();
            lblPlayerTitle.BackColor = pl.getPlayerColor();
            foreach (ResourceSelector sel in playerResourceSelectors)
            {
                sel.Click += lockedTradePlayerClickPlayerResource;
            }

            if (hb.getTradeOutputResource() == Board.ResourceType.Desert)
            {
                //The player may choose any resource to trade for.
                foreach (ResourceSelector select in otherResourceSelectors)
                {
                    select.Click += playerClickOtherResource;
                }
            } else
            {
                //The player is only allowed to choose the one resource.
                canClearOther = false;
                foreach (ResourceSelector select in otherResourceSelectors)
                {
                    if (select.type == hb.getTradeOutputResource())
                    {
                        select.setSelected(true);
                    } else
                    {
                        select.Hide();
                    }
                }
            }
        }

        /*
            Allow the player to trade with another player.
         */
        public void loadPlayerTrade(Player pl)
        {
            //Both players choose their resources.
            initiatingPlayer = pl;
        }

        /*
         * This will allow the player to choose what resources they loose when
         * the dice land on a "7" while the player has more than 7 resources on hand.
         */
         public void loadPlayerResourceLoss(Player pl)
        {
            //Load the player's resource selectors
            for (int i = 0; i < 5; i++)
            {
                Board.ResourceType resType = (Board.ResourceType)i;
                ResourceSelector playerSelector = new ResourceSelector(resType);
                playerResourceSelectors.Add(playerSelector);
                pnlPlayer.Controls.Add(playerSelector);
                playerSelector.Location = new Point(5, i * playerSelector.Height + 1);
                playerSelector.hideControls();
                playerSelector.Click += playerClickResourceSelectorRobber;
            }
            btnAccept.Enabled = false;
            initiatingPlayer = pl;
            lblPlayerTitle.Text = pl.getPlayerName();
            lblPlayerTitle.BackColor = pl.getPlayerColor();
            //Disable the cancel button
            btnCancel.Visible = false;
            int numToGive = pl.getTotalResourceCount() / 2; //Since this is an integer, VS will automatically drop the decimal which is essentailly the same as floor(count/2)
            minimumPlayerInput = numToGive;
            //Hide the other section
            lblTradeName.Text = "";
            pnlOther.Visible = false;
            //Show the player some instructions.
            lblInstructions.Text = "Please select " + numToGive + " resource to give up.";

            btnClearSelection.Click += playerClickResourceSelectorRobber;
        }

        /*
            Allow the player to pick any two available resources from the bank for no cost.
         */
        public void loadYearOfPlenty(Player pl)
        {
            initiatingPlayer = pl;
            lblTradeName.Text = "Bank";
            lblPlayerTitle.Text = pl.getPlayerName();
            lblPlayerTitle.BackColor = pl.getPlayerColor();
            //Allow the player to choose resources much like they can with the harbor trade
            for (int i = 0; i < 5; i++)
            {
                ResourceSelector otherSelector = new ResourceSelector((Board.ResourceType)i);
                otherResourceSelectors.Add(otherSelector);
                pnlOther.Controls.Add(otherSelector);
                otherSelector.Location = new Point(5, i * otherSelector.Height + 1);
                otherSelector.Click += yearOfPlentyClickResourceSelector;
                otherSelector.hideControls();
            }

        }

        private void yearOfPlentyClickResourceSelector(object sender, EventArgs e)
        {
            //Player can choose any two...
            if (sender is ResourceSelector)
            {
                int count = 0;
                foreach (ResourceSelector select in otherResourceSelectors)
                {
                    count += select.getCount();
                }
                if (count < 2)
                {
                    ((ResourceSelector)sender).setSelected(true);
                    ((ResourceSelector)sender).setCount(((ResourceSelector)sender).getCount() + 1);
                }
            }
        }

        public void lockedTradePlayerClickPlayerResource(object sender, EventArgs e)
        {
            if (sender is ResourceSelector)
            {
                bool otherResourceSelected = false;
                ResourceSelector selectedItem=null;
                foreach (ResourceSelector select in otherResourceSelectors)
                {
                    if (select.getSelected())
                    {
                        otherResourceSelected = true;
                        selectedItem = select;
                    }
                }
                if (otherResourceSelected)
                {
                    ResourceSelector resSelect = (ResourceSelector)sender;
                    //Adds more to this one if there are enough resources owned by the player.
                    int count = resSelect.getCount() + minimumPlayerInput;

                    if (initiatingPlayer.getResourceCount(resSelect.type) >= count)
                    {
                        int outCount = selectedItem.getCount() + 1;
                        if (Board.TheBank.canGiveOutResource(selectedItem.type, outCount))
                        {
                            resSelect.setSelected(true);
                            resSelect.setCount(count);
                            selectedItem.setCount(selectedItem.getCount() + 1);
                        } else
                        {
                            MessageBox.Show(BankError.BankDoesNotHaveEnoughResourceCard());
                        }
                    }
                    else
                    {
                        MessageBox.Show(BuildError.NOT_ENOUGH_RESOURCES);
                    }
                } else
                {
                    MessageBox.Show(TradeError.NO_HARBOR_RESOURCE_SELECTED);
                }
            }
        }

        public void playerClickOtherResource(object sender, EventArgs e)
        {
            if (sender is ResourceSelector)
            {
                int count = 0;
                foreach (ResourceSelector select in otherResourceSelectors)
                {
                    select.setSelected(false);
                    if (select.getCount()>0)
                    {
                        count = select.getCount();
                    }
                    select.setCount(0);
                }
                ((ResourceSelector)sender).setSelected(true);
                ((ResourceSelector)sender).setCount(count);
            }

        }

        public void playerClickResourceSelectorRobber(object sender, EventArgs e)
        {
            //
            if (sender is ResourceSelector)
            {
                ResourceSelector resSelect = (ResourceSelector)sender;
                int count = resSelect.getCount() + 1;
                if (resCount < minimumPlayerInput) {
                    if (initiatingPlayer.getResourceCount(resSelect.type) >= count)
                    {
                        resSelect.setSelected(true);
                        resSelect.setCount(count);
                        resCount++;
                        if (resCount == minimumPlayerInput)
                        {
                            lblInstructions.Text = "You have selected the required number of resources. Click accept to continue or clear selection to start over.";
                            btnAccept.Enabled = true;
                        }
                        else
                        {
                            lblInstructions.Text = "Please select " + (minimumPlayerInput - resCount) + " more resources to give up.";
                        }
                    }
                    else
                    {
                        MessageBox.Show(BuildError.NOT_ENOUGH_RESOURCES);
                    }
                }
                else
                {
                    MessageBox.Show("You do not need to select any more resources.");
                }
            } else if (sender == btnClearSelection)
            {
                //Clear everything.
                resCount = 0;
                lblInstructions.Text = "Please select " + minimumPlayerInput + " resources to give up.";
                btnAccept.Enabled = false;

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void createResourceSelectors(bool playerControlsEnabled, bool otherPartyControlsEnabled)
        {
            //Create the player resource selectors
            for (int i = 0; i < 5; i ++)
            {
                Board.ResourceType resType = (Board.ResourceType)i;
                ResourceSelector playerSelector = new ResourceSelector(resType);
                playerResourceSelectors.Add(playerSelector);
                pnlPlayer.Controls.Add(playerSelector);
                playerSelector.Location = new Point(5, i * playerSelector.Height + 1);
                if (!playerControlsEnabled)
                {
                    playerSelector.hideControls();
                }

                ResourceSelector otherSelector = new ResourceSelector(resType);
                otherResourceSelectors.Add(otherSelector);
                pnlOther.Controls.Add(otherSelector);
                otherSelector.Location = new Point(5, i * otherSelector.Height + 1);
                if (!otherPartyControlsEnabled)
                {
                    otherSelector.hideControls();
                }
            }
        }

        public static bool canPlayerTradeWithHarbor(Harbor hb, Player pl)
        {
            for (int i = 0; i < 5; i++)
            {
                //Get the player's current resource counts.
                
                Board.ResourceType rt = (Board.ResourceType)i;
                int count = pl.getResourceCount(rt);
                if (count >= hb.getRequiredResourceCount())
                {
                    return true;
                }
            }
            return false;
        }

        private void clearSelection(object sender, EventArgs e)
        {
            foreach (ResourceSelector sel in playerResourceSelectors)
            {
                sel.setSelected(false);
                sel.setCount(0);
            }
            if (canClearOther)
            {
                foreach (ResourceSelector sel in otherResourceSelectors)
                {
                    sel.setSelected(false);
                    sel.setCount(0);
                }
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            //Tabulate calculate and complete the transactions!

            foreach (ResourceSelector select in playerResourceSelectors)
            {
                //Give bank this resource
                for (int i = 0; i < select.getCount(); i++)
                {
                    ResourceCard rc = initiatingPlayer.takeResource(select.type);
                    Board.TheBank.putResourceCard(rc);
                }
            }

            foreach (ResourceSelector select in otherResourceSelectors)
            {
                for (int i = 0; i < select.getCount(); i ++)
                {
                    initiatingPlayer.giveResource(Board.TheBank.giveOutResource(select.type));
                }
            }

            //We are done.
            this.Close();
        }
    }
}
