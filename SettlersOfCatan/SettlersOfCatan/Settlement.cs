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

        Player owningPlayer;
        bool isCity = false;

        public Settlement(Point position, int index, Control p)
        {
            this.position = position;
            p.Controls.Add(this);
            Click += click;
            BringToFront();
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.Stretch;
            Location = new Point(position.X - 6, position.Y - 6);
            Size = new Size(12, 12);

            this.container = p;
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

        /*
            Conditions must apply: 
            no other player's settlement must be present at this location or, within 1 road's distance.
                if a settlement exists player must have required resources: 
            must have required resources: Brick, Wood, Wheat, Sheep

         */
        public bool buildRoad(Player currentPlayer)
        {
            if (owningPlayer == null)
            {
                //Build settlement
            } else
            {
                //Upgrade the settlement to city
            }
            return false;
        }
    }
}
