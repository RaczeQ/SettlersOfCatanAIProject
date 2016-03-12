using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SettlersOfCatan
{
    public class Tile
    {
        private Point position; //Expressed in pixels
        private Point size; //Expressed in pixels. This value will NEVER change. It is the base size that is used for resizing calculations.
        private double imageAspectRatio = 0;
        public int index = 0;

        private Bitmap image;
        private Bitmap original;
        private Panel canvas;

        private int requiredRoll;
        private int gatherChance = 0;

        private double imageWidthPercentage = 1;
        private double imageHeightPercentage = 1;

        private double imageXPercentage = 1;
        private double imageYPercentage = 1;

        public Tile(Panel p, int collectNumber)
        {
            p.Paint += new System.Windows.Forms.PaintEventHandler(this.draw);
            requiredRoll = collectNumber;
            this.canvas = p;
            this.position = new Point(0, 0);
            this.size = new Point(0, 0);
        }

        //Sets this objects drawing image.
        public void setImage(String imagePath)
        {
            image = new Bitmap(imagePath);
            original = new Bitmap(imagePath);
            imageAspectRatio = image.Width / image.Height;
            size = new Point(image.Width, image.Height);
            imageWidthPercentage = (double)image.Width / (double)canvas.Width;
            imageHeightPercentage = (double)image.Height / (double)canvas.Height;
        }

        //Sets the center* of the object. *Sets the bottom left hand corner actually
        public void setCenter(Point p)
        {
            position.X = p.X;
            position.Y = p.Y;
            imageXPercentage = (double)p.X / (double)canvas.Width;
            imageYPercentage = (double)p.Y / (double)canvas.Height;
        }

        public void setGatherChance(int num)
        {
            gatherChance = num;
        }

        public int getGatherChance()
        {
            return gatherChance;
        }

        public void resizeToFit(int newScreenWidth, int newScreenHeight)
        {
            //Change the size to fit correctly within the available space.
            //The size of a tile is based on the size of the drawable area
            //The horizontal size is simply (tileWidth/oldScreenWidth)*newScreenWidth
            //The vertical size is a bit more complicated: (tileHeight/oldScreenHeight)*newScreenHeight-(tileTriHeight/oldScreenHeight)*newScreenHeight

            //Then we need to convert the unit into dpi as this is UNFORTUNEATLY what VS wants to use.
            //Then we apply these numbers to the images and *cross fingers*.

            double newWidth = imageWidthPercentage * (double)newScreenWidth;
            double newHeight = imageHeightPercentage * (double)newScreenHeight; //Apply the "Meshing" to this as well!!!!

            double newPositionX = imageXPercentage * (double)newScreenWidth;
            double newPositionY = imageYPercentage * (double)newScreenHeight;

            image = ResizeBitmap(original, (int)Math.Round(newWidth), (int)Math.Round(newHeight));
            this.position.X = (int)newPositionX;
            this.position.Y = (int)newPositionY;
        }

        //Draws this object to it's assigned canvas
        public void draw(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            resizeToFit(canvas.Size.Width, canvas.Size.Height);

            e.Graphics.DrawImage(image, position.X, position.Y);
            if (gatherChance != -1)
            {
                e.Graphics.DrawString(index + "", new Font(new FontFamily("Microsoft Sans Serif"), 12), Brushes.Black, position.X + (size.X / 2), position.Y + (size.Y / 2));
            }
        }

        /** This resizes bitmap images and forces a better looking smoothing **/
        private Bitmap ResizeBitmap(Bitmap b, int nWidth, int nHeight)
        {
            Bitmap result = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage((Image)result))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(b, 0, 0, nWidth, nHeight);
            }
            return result;
        }
    }
}
