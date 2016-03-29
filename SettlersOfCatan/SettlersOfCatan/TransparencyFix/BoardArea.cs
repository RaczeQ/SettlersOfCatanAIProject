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

            Rectangle bak = new Rectangle(Location.X, Location.Y, Width, Height);
            this.graphics.DrawImage(this.BackgroundImage, bak);
            foreach (Control c in Controls)
            {
                bak.Width = c.Size.Width;
                bak.Height = c.Size.Height;
                bak.X = c.Location.X;
                bak.Y = c.Location.Y;

                if (c.BackgroundImage != null)
                {
                    this.graphics.DrawImage(c.BackgroundImage, bak);
                } else
                {
                    Pen p = new Pen(c.BackColor);
                    this.graphics.DrawRectangle(p, bak);
                }
            }
        }
    }
}
