using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;


namespace SettlersOfCatan
{
    [Serializable]
    public abstract class Tile : PictureBox, Card
    {
        public int index = 0;

        private Panel container;

        public Tile(Panel p)
        {
            container = p;
            container.Controls.Add(this);
            this.BackColor = Color.Transparent;
            this.Visible = true;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
//            base.OnPaint(pe);
        }

        public abstract string toString();
    }

    /**
     */
    [Serializable]
    public class TerrainTile : Tile
    {

        private NumberChip numberChip;
        private int gatherChance = 0;
        private Board.ResourceType tileType;

        public List<Settlement> adjascentSettlements;
        public List<Road> adjascentRoads;

        public TerrainTile(Panel p, Board.ResourceType resourceType, Bitmap image ) : base(p)
        {
            this.tileType = resourceType;
            adjascentSettlements = new List<Settlement>();
            adjascentRoads = new List<Road>();

            //Atuomatically determine what image we need to display depending on the resource type
            this.BackgroundImage = image;
            this.Size = BackgroundImage.Size;
            this.BackColor = Color.Transparent;

        }

        public void setNumberChip(NumberChip chip)
        {
            chip.Size = new Size(32, 32);
            chip.Location = new Point(Board.SPACING / 2-16, Board.SPACING/2-16);
            this.numberChip = chip;
            this.gatherChance = chip.getNumber();
            this.Controls.Add(chip);
        }

        public void distributeResource()
        {
            if (tileType != Board.ResourceType.Desert)
            {
                foreach (Settlement set in adjascentSettlements)
                {
                    if (set.getOwningPlayer() != null)
                    {
                        ResourceCard rc = Board.TheBank.giveResource(tileType);
                        if (rc != null)
                        {
                            set.getOwningPlayer().giveResource(rc);
                        }
                        else
                        {
                            MessageBox.Show("No more resources to give!");
                        }
                    }
                }
            }
        }

        public bool hasSettlement(Settlement findSettlement)
        {
            foreach (Settlement lookSettlement in adjascentSettlements)
            {
                if (lookSettlement == findSettlement)
                {
                    return true;
                }
            }
            return false;
        }

        public int getGatherChance()
        {
            return gatherChance;
        }

        public Board.ResourceType getResourceType()
        {
            return this.tileType;
        }

        public void placeThief()
        {
            this.numberChip.placeThief();
        }

        public void removeThief()
        {
            this.numberChip.removeThief();
        }

        public bool isGatherBlocked()
        {
            return this.numberChip.isBlocked();
        }

        public NumberChip getNumberChip()
        {
            return this.numberChip;
        }

        public override String toString()
        {
            String str = "Terrain Tile ID: " + index + " Settlements: ( ";
            foreach (Settlement s in adjascentSettlements)
            {
                str += s.id + ", ";
            }
            str += ") Roads: ( ";
            foreach (Road r in adjascentRoads)
            {
                str += r.id + ", ";
            }
            str += " )";
            return str;
        }
    }

    /**

     */
    [Serializable]
    public class OceanBorderTile : Tile
    {

        public OceanBorderTile(Panel p) : base(p)
        {
            this.BackgroundImage = new Bitmap("Resources/Ocean.png");
            this.Size = BackgroundImage.Size;
            this.BackColor = Color.Transparent;
        }

        public override string toString()
        {
            return "";
        }
    }
}
