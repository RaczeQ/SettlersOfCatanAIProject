using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace SettlersOfCatan
{
    public partial class MainMenu : Form
    {

        Board theGame;

        public MainMenu()
        {
            InitializeComponent();
            btnSave.Hide();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //this.Hide();
            theGame = new Board();
            btnSave.Show();
            theGame.Show();
        }



        private void saveGameState()
        {
            MessageBox.Show("Sorry, this feature is not implemented yet.");
        }

        private void loadGameState()
        {
            MessageBox.Show("Sorry, this feature is not implemented yet.");
        }

        private void saveGame(object sender, EventArgs e)
        {
            saveGameState();
        }

        private void loadGame(object sender, EventArgs e)
        {
            loadGameState();
        }
    }
}
