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
        private static int[] chipNumbers = { 0, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12 };
        [ThreadStatic]
        private static IDictionary<int, Bitmap> bitmaps;
        public int numberValue { get; private set; } = 0;

        public bool blocked { get; private set; } = false;

        public NumberChip(int numberValue, bool initialize = true)
        {
            if (initialize)
            {
                bitmaps = new Dictionary<int, Bitmap>();
                foreach (int i in chipNumbers)
                {
                    bitmaps.Add(i, new Bitmap("Resources/" + i + "_Chip.png"));
                }
            }
            this.numberValue = numberValue;
            if (numberValue != 0)
            {
                this.BackgroundImage = bitmaps[numberValue];
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
