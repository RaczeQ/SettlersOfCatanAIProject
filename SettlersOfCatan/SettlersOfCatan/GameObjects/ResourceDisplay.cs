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
        private static IDictionary<Board.ResourceType, Bitmap> bitmaps;
        public Board.ResourceType type = Board.ResourceType.Desert;
        public int count = 0;
        public bool hidden = false;

        public ResourceDisplay(bool initialize = true)
        {
            if (initialize)
            {
                bitmaps = new Dictionary<Board.ResourceType, Bitmap>();
                foreach (Board.ResourceType rt in Enum.GetValues(typeof(Board.ResourceType)))
                {
                    bitmaps.Add(rt, new Bitmap("Resources/" + Board.iconImageResourceNames[(int)rt]));
                }
            }
            InitializeComponent();
        }

        public void setType(Board.ResourceType rType)
        {
            type = rType;
            //Set the icon.
            pbIcon.BackgroundImage = bitmaps[rType];
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
