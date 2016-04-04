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
    public partial class Dice : UserControl
    {
        private String resourcePath = "Resources/";
        private String redDiceFaceFileName = "_Die_Face_Red.png";
        private String yellowDiceFaceFileName = "_Die_Face_Yellow.png";

        private Bitmap[] yellowDiceFaces = new Bitmap[6];
        private Bitmap[] redDiceFaces = new Bitmap[6];

        public Dice()
        {
            InitializeComponent();

        }

        private int diceValue1 = 0;
        private int diceValue2 = 0;
        private Random diceRandomizer;
        private int lastRollValue = 0;

        /*
            Gives a realistic dice value by generating two random numbers and adding them.
            The values of the individual dice can be accessed later with the above global variables.
         */
        public int roll()
        {
            lastRollValue = getRollValue();
            diceValue1 = diceRandomizer.Next(1, 7);
            diceValue2 = diceRandomizer.Next(1, 7);
            return diceValue1 + diceValue2;
        }

        public int getRollValue()
        {
            return diceValue1 + diceValue2;
        }

        public int getLastRollValue()
        {
            return lastRollValue;
        }

        private void Dice_Enable(object sender, EventArgs e)
        {
            this.lblInstructions.Visible = this.Enabled;
            this.pbRedDie.Visible = this.Enabled;
            this.pbYellowDie.Visible = this.Enabled;
        }

        private void childClicked(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void Dice_Click(object sender, EventArgs e)
        {
            roll();
            this.pbYellowDie.Image = yellowDiceFaces[diceValue2 - 1];
            this.pbRedDie.Image = redDiceFaces[diceValue1 - 1];
            this.lblRollValue.Text = getRollValue() + "";
        }

        private void Dice_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                for (int i = 1; i < 7; i++)
                {
                    yellowDiceFaces[i-1] = new Bitmap(resourcePath + i + yellowDiceFaceFileName);
                    redDiceFaces[i-1] = new Bitmap(resourcePath + i + redDiceFaceFileName);
                }
                pbYellowDie.Image = yellowDiceFaces[0];
                pbRedDie.Image = redDiceFaces[0];
                diceRandomizer = new Random();
            }
        }
    }
}
