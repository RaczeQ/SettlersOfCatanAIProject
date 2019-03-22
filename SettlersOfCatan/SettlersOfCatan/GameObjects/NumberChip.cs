using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SettlersOfCatan.GameObjects
{
    [Serializable]
    public class NumberChip : PictureBox, Card
    {
        public int numberValue { get; private set; } = 0;

        public bool blocked { get; private set; } = false;

        public NumberChip(int numberValue, bool loadBitmaps = true)
        {
            this.numberValue = numberValue;
            if (numberValue != 0 && loadBitmaps)
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
            if (numberValue != 0)
            {
                this.BackgroundImage = new Bitmap("Resources/" + numberValue + "_Chip.png");
            }
            else
            {
                this.BackgroundImage = null;
            }
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
