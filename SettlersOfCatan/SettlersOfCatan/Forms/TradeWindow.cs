using SettlersOfCatan.Events;
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
        public TradeWindow()
        {
            InitializeComponent();
            MessageBox.Show("This is not implemented yet. Close the window to continue.");
        }

        /*
            Allow the player to trade any 4 of the same resource for any one other available resource from the bank.
         */
        public void loadBankTrade()
        {

        }

        /*
            Allow the player to trade with the chosen harbor.
         */
        public void loadHarborTrade()
        {

        }

        /*
            Allow the player to trade with another player.
         */
        public void loadPlayerTrade()
        {

        }

        /*
            Allow the player to pick any two available resources from the bank for no cost.
         */
        public void loadYearOfPlenty()
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
