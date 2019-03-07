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
using System.Diagnostics;

namespace SettlersOfCatan
{
    public partial class MainMenu : Form
    {

        Board theGame;

        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //this.Hide();
            theGame = new Board(false);
            theGame.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRules_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Resources/SoC_rv_Rules_091907.pdf");
            } catch (Exception ex)
            {
                MessageBox.Show("You must have a pdf reader installed to view the rules document.");
            }
        }
    }
}
