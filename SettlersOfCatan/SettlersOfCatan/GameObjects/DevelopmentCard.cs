﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SettlersOfCatan.GameObjects
{
    [Serializable]
    public class DevelopmentCard : PictureBox, Card
    {

        public enum DevCardType { Back, Knight, Victory, Road, Plenty, Monopoly };
        public static Size devCardSize = new Size(128, 184);

        private bool canUse = false;
        private DevCardType type = DevCardType.Back;
        private bool hidden = false;
        public bool used = false;

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
            if (!used)
            {
                hidden = true;
                this.BackgroundImage = Bank.devCardImages[(int)DevCardType.Back];
            }
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

        public bool isPlayable()
        {
            return canUse;
        }

        public void setPlayable(bool use)
        {
            this.canUse = use;
        }

    }
}
