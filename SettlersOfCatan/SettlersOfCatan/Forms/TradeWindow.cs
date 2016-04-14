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

        public List<ResourceSelector> playerResourceIcons = new List<ResourceSelector>();
        public List<ResourceSelector> otherResourceIcons = new List<ResourceSelector>();
        public Player initiatingPlayer;
        private bool canClearOther = true;

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
            foreach (ResourceSelector sel in playerResourceIcons)
            {
                sel.Click += lockedTradePlayerClickPlayerResource;
            }

            foreach (ResourceSelector select in otherResourceIcons)
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
            foreach (ResourceSelector sel in playerResourceIcons)
            {
                sel.Click += lockedTradePlayerClickPlayerResource;
            }

            if (hb.getTradeOutputResource() == Board.ResourceType.Desert)
            {
                //The player may choose any resource to trade for.
                foreach (ResourceSelector select in otherResourceIcons)
                {
                    select.Click += playerClickOtherResource;
                }
            } else
            {
                //The player is only allowed to choose the one resource.
                canClearOther = false;
                foreach (ResourceSelector select in otherResourceIcons)
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
            Allow the player to pick any two available resources from the bank for no cost.
         */
        public void loadYearOfPlenty(Player pl)
        {
            initiatingPlayer = pl;
        }


        public void lockedTradePlayerClickPlayerResource(object sender, EventArgs e)
        {
            if (sender is ResourceSelector)
            {
                bool otherResourceSelected = false;
                ResourceSelector selectedItem=null;
                foreach (ResourceSelector select in otherResourceIcons)
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
                foreach (ResourceSelector select in otherResourceIcons)
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
                playerResourceIcons.Add(playerSelector);
                pnlPlayer.Controls.Add(playerSelector);
                playerSelector.Location = new Point(5, i * playerSelector.Height + 1);
                if (!playerControlsEnabled)
                {
                    playerSelector.hideControls();
                }

                ResourceSelector otherSelector = new ResourceSelector(resType);
                otherResourceIcons.Add(otherSelector);
                pnlOther.Controls.Add(otherSelector);
                otherSelector.Location = new Point(5, i * playerSelector.Height + 1);
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
            foreach (ResourceSelector sel in playerResourceIcons)
            {
                sel.setSelected(false);
                sel.setCount(0);
            }
            if (canClearOther)
            {
                foreach (ResourceSelector sel in otherResourceIcons)
                {
                    sel.setSelected(false);
                    sel.setCount(0);
                }
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            //Tabulate calculate and complete the transactions!

            foreach (ResourceSelector select in playerResourceIcons)
            {
                //Give bank this resource
                for (int i = 0; i < select.getCount(); i++)
                {
                    ResourceCard rc = initiatingPlayer.takeResource(select.type);
                    Board.TheBank.putResourceCard(rc);
                }
            }

            foreach (ResourceSelector select in otherResourceIcons)
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
