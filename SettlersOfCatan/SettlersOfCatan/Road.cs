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

        Player owningPlayer;

        Control container;

        public Road(Point position, int index, Control p)
        {
            p.Controls.Add(this);
            this.position = position;
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
            //MessageBox.Show(sender.GetType().ToString());
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

        public bool buildRoad(Player currentPlayer)
        {
            if (owningPlayer == null)
            {
                if (currentPlayer.getResourceCount(Board.ResourceType.Wood) > 0 && currentPlayer.getResourceCount(Board.ResourceType.Brick) > 0)
                {
                    this.owningPlayer = currentPlayer;
                    this.BackColor = currentPlayer.getPlayerColor();
                    /*
                        Take resources from player!
                     */
                    return true;
                } else
                {
                    MessageBox.Show("Not enough resources!");
                }
            }
            else
            {
                MessageBox.Show("Road is already built there!");
            }
            return false;
        }
    }
}
