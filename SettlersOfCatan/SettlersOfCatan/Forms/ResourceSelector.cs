using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SettlersOfCatan.Forms
{
    public partial class ResourceSelector : UserControl
    {
        private int count = 0;
        public int max = 10;
        public int min = 0;
        public Board.ResourceType type;
        private bool selected = false;
        public ResourceSelector(Board.ResourceType type)
        {
            InitializeComponent();
            pictureBox1.BackgroundImage = new Bitmap("Resources/" + Board.iconImageResourceNames[(int)type]);
            lblResName.Text = Board.RESOURCE_NAMES[(int)type];
            this.type = type;
        }

        public void setSelected(bool selected)
        {
            if (selected)
            {
                this.BackColor = Color.Green;
            } else
            {
                this.BackColor = Color.White;
            }
            this.selected = selected;
        }

        public bool getSelected()
        {
            return selected;
        }

        public void hideControls()
        {
            this.btnDecrease.Visible = false;
            this.btnIncrease.Visible = false;
        }

        public void setCount(int number)
        {
            this.count = number;
            lblCount.Text = "" + number;
        }

        public int getCount()
        {
            return this.count;
        }

        private void btnDecrease_Click(object sender, EventArgs e)
        {
            if (count > min)
            {
                count--;
                lblCount.Text = count + "";
            }
        }

        private void btnIncrease_Click(object sender, EventArgs e)
        {
            if (count < max)
            {
                count++;
                lblCount.Text = count + "";
            }
        }

        private void ChildClicked(object sender, EventArgs e)
        {
            this.OnClick(e);
        }
    }
}
