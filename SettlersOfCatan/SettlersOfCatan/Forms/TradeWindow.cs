﻿using SettlersOfCatan.Events;
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
        public Player otherPlayer;
        public Board.ResourceType selectedResource = Board.ResourceType.Desert;
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
            lblPlayerTitle.Text = pl.getName();
            lblPlayerTitle.BackColor = pl.getColor();
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
            lblPlayerTitle.Text = pl.getName();
            lblPlayerTitle.BackColor = pl.getColor();
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
        public void loadPlayerTrade(Player currentPlayer, Player otherPlayer)
        {
            //Both players choose their resources.
            initiatingPlayer = currentPlayer;
            lblPlayerTitle.Text = currentPlayer.getName();
            lblPlayerTitle.BackColor = currentPlayer.getColor();
            this.otherPlayer = otherPlayer;
            lblTradeName.Text = otherPlayer.getName();
            lblTradeName.BackColor = otherPlayer.getColor();
            otherPlayer.Current = true;
            btnAccept.Click -= acceptClickToBank;
            btnAccept.Click += acceptClickPlayerToPlayer;
            createResourceSelectors(false, false);
            foreach (ResourceSelector sel in playerResourceSelectors)
            {
                sel.Click += ptpClickResourceSelectorPlayer;
            }

            foreach (ResourceSelector sel in otherResourceSelectors)
            {
                sel.Click += ptpClickResourceSelectorOther;
            }
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
            lblInstructions.Visible = true;
            btnAccept.Enabled = false;
            initiatingPlayer = pl;
            lblPlayerTitle.Text = pl.getName();
            lblPlayerTitle.BackColor = pl.getColor();
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
            lblPlayerTitle.Text = pl.getName();
            lblPlayerTitle.BackColor = pl.getColor();
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

        /*
         * 
         */
         public void loadMonopoly(Player pl)
        {
            pnlOther.Visible = false;
            btnClearSelection.Visible = false;
            lblInstructions.Text += "Please select the resource you wish to take from the other players.";
            lblInstructions.Visible = true;
            lblTradeName.Text = "";
            lblPlayerTitle.Text = pl.getName();
            lblPlayerTitle.BackColor = pl.getColor();
            btnAccept.Click -= acceptClickToBank;
            btnAccept.Click += acceptClickMonopoly;
            btnAccept.Enabled = false;
            //Load all resource selectors.
            for (int i = 0; i < 5; i++)
            {
                ResourceSelector resSelect = new ResourceSelector((Board.ResourceType)i);
                resSelect.hideControls();
                pnlPlayer.Controls.Add(resSelect);
                resSelect.Location = new Point(5, i * resSelect.Height + 1);
                playerResourceSelectors.Add(resSelect);
                resSelect.Click += clickMonopoly;
            }
        }

        /*
         * The following are all the events that drives each of the individual trade window events.
         * 
         */

        private void ptpClickResourceSelectorPlayer(object sender, EventArgs e)
        {
            //Player offer
            if (sender is ResourceSelector)
            {
                ResourceSelector resSelect = (ResourceSelector)sender;
                int count = resSelect.getCount() + 1;
                //Check if the current player has the required number of resources
                if (initiatingPlayer.getResourceCount(resSelect.type) >= count)
                {
                    resSelect.setCount(count);
                    resSelect.setSelected(true);
                }
                else
                {
                    MessageBox.Show(BuildError.NOT_ENOUGH_RESOURCES);
                }
            }
        }

        private void ptpClickResourceSelectorOther(object sender, EventArgs e)
        {
            //Other offer
            if (sender is ResourceSelector)
            {
                ResourceSelector resSelect = (ResourceSelector)sender;
                int count = resSelect.getCount() + 1;
                //Check if the current player has the required number of resources
                if (otherPlayer.getResourceCount(resSelect.type) >= count)
                {
                    resSelect.setCount(count);
                    resSelect.setSelected(true);
                }
                else
                {
                    MessageBox.Show(BuildError.NOT_ENOUGH_RESOURCES);
                }
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

        public void clickMonopoly(object sender, EventArgs e)
        {
            if (sender is ResourceSelector)
            {
                foreach (ResourceSelector resSelect in playerResourceSelectors)
                {
                    resSelect.setSelected(false);
                }
                ((ResourceSelector)sender).setSelected(true);
                btnAccept.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (otherPlayer != null)
            {
                otherPlayer.Current = false;
            }
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
            foreach (Board.ResourceType rt in Enum.GetValues(typeof(Board.ResourceType)))
            {
                //Get the player's current resource counts.
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

        private void acceptClickToBank(object sender, EventArgs e)
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

        private void acceptClickPlayerToPlayer(object sender, EventArgs e)
        {
            int pCount = 0;
            foreach (ResourceSelector select in playerResourceSelectors)
            {
                //Give bank this resource
                pCount += select.getCount();
            }
            int oCount = 0;
            foreach (ResourceSelector select in otherResourceSelectors)
            {
                oCount += select.getCount();
            }

            if (!(pCount > 0 && oCount > 0))
            {
                MessageBox.Show("Both players must select at least one resource to continue.");
                return;
            }

            //Tabulate calculate and complete the transactions!

            foreach (ResourceSelector select in playerResourceSelectors)
            {
                //Give bank this resource
                for (int i = 0; i < select.getCount(); i++)
                {
                    //Give the other player this resource.
                    otherPlayer.giveResource(initiatingPlayer.takeResource(select.type));
                }
            }

            foreach (ResourceSelector select in otherResourceSelectors)
            {
                for (int i = 0; i < select.getCount(); i++)
                {
                    //Give the current player this resource.
                    initiatingPlayer.giveResource(otherPlayer.takeResource(select.type));
                }
            }

            //We are done.
            otherPlayer.Current = false;
            this.Close();
        }

        private void acceptClickMonopoly(object sender, EventArgs e)
        {
            //Somehow transfer all resources to the initiating player from other player's hands.
            //Determine the selected resource
            foreach (ResourceSelector resSelect in playerResourceSelectors)
            {
                if (resSelect.getSelected())
                {
                    this.selectedResource = resSelect.type;
                }
            }
            this.Close();
        }

        /**/
    }
}
