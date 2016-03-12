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

        Image highlightImage;
        List<Road> connectedRoads;

        int owningPlayer = 0; //0 is no player.

        public Settlement(Point position, int index, Panel p, Image highlight)
        {
            p.Controls.Add(this);

            Text = index + "";
            Click += clickSettlement;
            MouseHover += mouseLeaveSettlement;
            MouseLeave += mouseEnterSettlement;
            BringToFront();
            BackColor = Color.AliceBlue;
            BackgroundImageLayout = ImageLayout.Stretch;
            Location = new Point(position.X - 6, position.Y - 6);
            Size = new Size(12, 12);

            highlightImage = highlight;
        }

        private void mouseEnterSettlement(object sender, EventArgs e)
        {
            Settlement p = (Settlement)sender;
            p.BackgroundImage = null;
        }

        private void mouseLeaveSettlement(object sender, EventArgs e)
        {
            Settlement p = (Settlement)sender;
            //p.BackgroundImage = highlightImage;
        }

        private void clickSettlement(object sender, EventArgs e)
        {
            //Does nothing yet!
        }

    }
}
