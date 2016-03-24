using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SettlersOfCatan
{
    public class Road : PictureBox
    {
        public int id;
        public Point position;

        List<Settlement> connectedSettlements=new List<Settlement>();

        private double imageWidthPercentage = 1;
        private double imageHeightPercentage = 1;

        private double imageXPercentage = 1;
        private double imageYPercentage = 1;

        int owningPlayer = 0; //0 is no player.

        Control container;

        public Road(Point position, int index, Control p)
        {
            p.Controls.Add(this);
            this.position = position;
            Text = index + "";
            Click += click;
            MouseHover += mouseEnter;
            MouseLeave += mouseLeave;
            BringToFront();
            BackColor = Color.Red;
            BackgroundImageLayout = ImageLayout.Stretch;
            Location = new Point(position.X - 6, position.Y - 6);
            Size = new Size(12, 12);

            this.container = p;

        }

        private void mouseEnter(object sender, EventArgs e)
        {
            Road p = (Road)sender;
            p.BackgroundImage = null;
        }

        private void mouseLeave(object sender, EventArgs e)
        {
            Road p = (Road)sender;
        }

        private void click(object sender, EventArgs e)
        {
            //Does nothing yet!
        }


        /**
        Returns true if the current road is already linked.
         */
        public bool linkSettlement(Settlement set)
        {
            foreach (Settlement currentSettlement in connectedSettlements)
            {
                if (set.id == currentSettlement.id)
                {
                    return true;
                }
            }
            connectedSettlements.Add(set);
            return false;
        }
    }
}
