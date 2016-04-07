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

        public void loadBankTrade()
        {
        }

        public void loadHarborTrade()
        {

        }

        public void loadPlayerTrade()
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
