using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SettlersOfCatan.TransparencyFix
{
    class BoardArea : TPanel
    {

        protected override void OnDraw()
        {
            Rectangle rct2 = new Rectangle(0, 0, this.Width, this.Height);
            this.graphics.DrawRectangle(new Pen(Color.Black), rct2);
            foreach (Control c in Controls)
            {
                int width = c.Size.Width;
                int height = c.Size.Height;
                int x = c.Location.X;
                int y = c.Location.Y;
                Rectangle rct = new Rectangle(x, y, width, height);
                if (c.BackgroundImage != null)
                {
                    this.graphics.DrawImage(c.BackgroundImage, rct);
                } else
                {
                    Pen p = new Pen(c.BackColor);
                    //this.graphics.DrawRectangle(p, rct);
                }
            }
        }
    }
}
