using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SettlersOfCatan
{
    public class DevelopmentCard : PictureBox, Card
    {

        public enum DevCardType { Back, Knight, Victory, Road, Plenty, Monopoly };
        public static Size devCardSize = new Size(64, 92);


        private DevCardType type = DevCardType.Back;
        private bool hidden = false;
        private bool used = false;

        public DevelopmentCard()
        {
            this.Size = devCardSize;
            
            
            this.BackgroundImage = Bank.devCardImages[(int)DevCardType.Back];

            this.BackgroundImageLayout = ImageLayout.Stretch;

        }

        public void show()
        {
            hidden = false;
            this.BackgroundImage = Bank.devCardImages[(int)type];
        }

        public void hide()
        {
            hidden = true;
            this.BackgroundImage = Bank.devCardImages[(int)DevCardType.Back];
        }

        public void setType(DevCardType type)
        {
            this.type = type;
            if (hidden)
            {
                this.BackgroundImage = Bank.devCardImages[(int)DevCardType.Back];
            } else
            {
                this.BackgroundImage = Bank.devCardImages[(int)type];
            }
        }

        public DevCardType getType()
        {
            return type;
        }

    }
}
