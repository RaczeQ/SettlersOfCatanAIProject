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
        Image highlightImage;
        List<Settlement> connectedSettlements;

        private double imageWidthPercentage = 1;
        private double imageHeightPercentage = 1;

        private double imageXPercentage = 1;
        private double imageYPercentage = 1;

        int owningPlayer = 0; //0 is no player.

        Control container;

        public Road(Point position, int index, Control p, Image highlight)
        {
            p.Controls.Add(this);

            Text = index + "";
            Click += click;
            MouseHover += mouseEnter;
            MouseLeave += mouseLeave;
            BringToFront();
            BackColor = Color.Red;
            BackgroundImageLayout = ImageLayout.Stretch;
            Location = new Point(position.X - 6, position.Y - 6);
            Size = new Size(12, 12);

            highlightImage = highlight;
            this.container = p;
            container.SizeChanged += new EventHandler(ResizeRoad);

            this.LocationChanged += new EventHandler(locationChanged);
            this.SizeChanged += new EventHandler(sizeChanged);

            imageWidthPercentage = (double)this.Width / (double)container.Width;
            imageHeightPercentage = (double)this.Height / (double)container.Height;
            imageXPercentage = (double)this.Location.X / (double)container.Width;
            imageYPercentage = (double)this.Location.Y / (double)container.Height;

        }

        public void sizeChanged(Object sender, EventArgs args)
        {
            imageWidthPercentage = (double)this.Width / (double)container.Width;
            imageHeightPercentage = (double)this.Height / (double)container.Height;
        }

        public void locationChanged(Object sender, EventArgs args)
        {
            imageXPercentage = (double)this.Location.X / (double)container.Width;
            imageYPercentage = (double)this.Location.Y / (double)container.Height;
        }

        public void ResizeRoad(Object sender, EventArgs args)
        {
            resizeToFit(container.Size.Width, container.Size.Height);
        }

        public void resizeToFit(int newScreenWidth, int newScreenHeight)
        {

            double newWidth = imageWidthPercentage * (double)newScreenWidth;
            double newHeight = imageHeightPercentage * (double)newScreenHeight;

            double newPositionX = imageXPercentage * (double)newScreenWidth;
            double newPositionY = imageYPercentage * (double)newScreenHeight;

            this.Size = new Size((int)Math.Round(newWidth), (int)Math.Round(newHeight));
            this.Location = new Point((int)newPositionX, (int)newPositionY);
        }

        private void mouseEnter(object sender, EventArgs e)
        {
            Road p = (Road)sender;
            p.BackgroundImage = null;
        }

        private void mouseLeave(object sender, EventArgs e)
        {
            Road p = (Road)sender;
            //p.BackgroundImage = highlightImage;
        }

        private void click(object sender, EventArgs e)
        {
            //Does nothing yet!
        }
    }
}
