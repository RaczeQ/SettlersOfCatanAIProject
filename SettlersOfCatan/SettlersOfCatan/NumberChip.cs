using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SettlersOfCatan
{
    public class NumberChip : PictureBox, Card
    {
        private int numberValue = 0;

        private bool blocked = false;

        public NumberChip(int numberValue)
        {
            this.numberValue = numberValue;
            if (numberValue != 0)
            {
                this.BackgroundImage = new Bitmap("Resources/" + numberValue + "_Chip.png");
            }
        }

        public void placeThief()
        {
            this.blocked = true;
            this.BackgroundImage = new Bitmap("Resources/thief.png");
        }

        public void removeThief()
        {
            this.blocked = false;
            this.BackgroundImage = new Bitmap("Resources/" + numberValue + "_Chip.png");
        }

        public bool isBlocked()
        {
            return blocked;
        }

        public int getNumber()
        {
            return numberValue;
        }
    }
}
