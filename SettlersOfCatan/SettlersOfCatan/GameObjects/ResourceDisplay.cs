using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SettlersOfCatan.GameObjects
{
    public partial class ResourceDisplay : UserControl
    {

        public Board.ResourceType type = Board.ResourceType.Desert;
        public int count = 0;
        public bool hidden = false;

        public ResourceDisplay(bool loadBitmaps = true)
        {
            if (loadBitmaps)
            {
                InitializeComponent();
            }
        }

        public void setType(Board.ResourceType rType, bool loadBitmaps)
        {
            type = rType;
            //Set the icon.
            if (loadBitmaps)
            {
                pbIcon.BackgroundImage = new Bitmap("Resources/" + Board.iconImageResourceNames[(int) type]);
            }
        }

        public void setCount(int rCount)
        {
            count = rCount;
            if (!hidden)
            {
                lblCount.Text = "" + count;
            }
        }

        public void show()
        {
            lblCount.Text = "" + count;
            hidden = false;
        }

        public void hide()
        {
            lblCount.Text = "*";
            hidden = true;
        }
    }
}
