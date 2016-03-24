using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SettlersOfCatan
{
    public class Settlement : PictureBox
    {
        public int id = 0;
        public Point position;

        List<Road> connectedRoads=new List<Road>();
        Control container;

        private double imageWidthPercentage = 1;
        private double imageHeightPercentage = 1;

        private double imageXPercentage = 1;
        private double imageYPercentage = 1;

        int owningPlayer = 0; //0 is no player.

        public Settlement(Point position, int index, Control p)
        {
            this.position = position;
            p.Controls.Add(this);
            Text = index + "";
            Click += click;
            MouseHover += mouseEnter;
            MouseLeave += mouseLeave;
            BringToFront();
            BackColor = Color.Blue;
            BackgroundImageLayout = ImageLayout.Stretch;
            Location = new Point(position.X - 6, position.Y - 6);
            Size = new Size(12, 12);

            this.container = p;
        }

        private void mouseLeave(object sender, EventArgs e)
        {
            Settlement p = (Settlement)sender;
            p.BackgroundImage = null;
        }

        private void mouseEnter(object sender, EventArgs e)
        {
            Settlement p = (Settlement)sender;
        }

        private void click(object sender, EventArgs e)
        {
            //Does nothing yet!
        }

        private void resize()
        {

        }

        /**
            Returns true if the current road is already linked.
          */
        public bool linkRoad(Road rd)
        {
            foreach (Road currentRoad in connectedRoads)
            {
                if (rd.id == currentRoad.id)
                {
                    return true;
                }
            }
            connectedRoads.Add(rd);
            return false;
        }
    }
}
