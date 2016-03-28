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
    public partial class PlayerInfoPanel : UserControl
    {

        private String wheatCount = "0";
        private String sheepCount = "0";
        private String woodCount = "0";
        private String brickCount = "0";
        private String oreCount = "0";

        bool resourcesHidden = false;

        public PlayerInfoPanel()
        {
            InitializeComponent();
        }

        //Replaces the value with an asterisk
        public void HideResources()
        {
            this.lblBrick.Text = "*";
            this.lblOre.Text = "*";
            this.lblSheep.Text = "*";
            this.lblWheat.Text = "*";
            this.lblWood.Text = "*";
            resourcesHidden = true;
        }

        public void ShowResources()
        {
            this.lblBrick.Text = brickCount;
            this.lblOre.Text = oreCount;
            this.lblSheep.Text = sheepCount;
            this.lblWheat.Text = wheatCount;
            this.lblWood.Text = woodCount;
            resourcesHidden = false;
        }

        //Each updates the required values
        public void setWood(int count)
        {
            lblWood.Text = count + "";
            woodCount = count + "";
            if (resourcesHidden)
            {
                lblWood.Text = "*";
            }
        }

        public void setWheat(int count)
        {
            lblWheat.Text = count + "";
            wheatCount = count + "";
            if (resourcesHidden)
            {
                lblWheat.Text = "*";
            }
        }

        public void setSheep(int count)
        {
            lblSheep.Text = count + "";
            sheepCount = count + "";
            if (resourcesHidden)
            {
                lblSheep.Text = "*";
            }
        }

        public void setBrick(int count)
        {
            lblBrick.Text = count + "";
            brickCount = count + "";
            if (resourcesHidden)
            {
                lblBrick.Text = "*";
            }
        }

        public void setOre(int count)
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
