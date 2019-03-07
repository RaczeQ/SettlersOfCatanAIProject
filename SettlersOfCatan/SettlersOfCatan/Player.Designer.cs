using SettlersOfCatan.AI;
using System.Linq;

namespace SettlersOfCatan
{
    partial class Player
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label6 = new System.Windows.Forms.Label();
            this.lblVictoryPoints = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblPlayerColorName = new System.Windows.Forms.Label();
            this.pnlPlayerColor = new System.Windows.Forms.Panel();
            this.pbLongestRoad = new System.Windows.Forms.PictureBox();
            this.pbLargestArmy = new System.Windows.Forms.PictureBox();
            this.lblTurn = new System.Windows.Forms.Label();
            this.pnlDevCards = new System.Windows.Forms.Panel();
            this.pnlResources = new System.Windows.Forms.Panel();
            this.playerComboBox = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.pnlPlayerColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLongestRoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLargestArmy)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Score";
            // 
            // lblVictoryPoints
            // 
            this.lblVictoryPoints.AutoSize = true;
            this.lblVictoryPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVictoryPoints.Location = new System.Drawing.Point(16, 31);
            this.lblVictoryPoints.Name = "lblVictoryPoints";
            this.lblVictoryPoints.Size = new System.Drawing.Size(25, 25);
            this.lblVictoryPoints.TabIndex = 3;
            this.lblVictoryPoints.Text = "0";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.lblVictoryPoints);
            this.panel1.Location = new System.Drawing.Point(178, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(58, 61);
            this.panel1.TabIndex = 4;
            // 
            // lblPlayerColorName
            // 
            this.lblPlayerColorName.AutoSize = true;
            this.lblPlayerColorName.Location = new System.Drawing.Point(13, 9);
            this.lblPlayerColorName.Name = "lblPlayerColorName";
            this.lblPlayerColorName.Size = new System.Drawing.Size(94, 13);
            this.lblPlayerColorName.TabIndex = 5;
            this.lblPlayerColorName.Text = "Player Color Name";
            this.lblPlayerColorName.Click += new System.EventHandler(this.Player_Name_Clicked);
            // 
            // pnlPlayerColor
            // 
            this.pnlPlayerColor.BackColor = System.Drawing.Color.White;
            this.pnlPlayerColor.Controls.Add(this.lblPlayerColorName);
            this.pnlPlayerColor.Location = new System.Drawing.Point(11, 9);
            this.pnlPlayerColor.Name = "pnlPlayerColor";
            this.pnlPlayerColor.Size = new System.Drawing.Size(125, 32);
            this.pnlPlayerColor.TabIndex = 6;
            this.pnlPlayerColor.Click += new System.EventHandler(this.Player_Name_Clicked);
            // 
            // pbLongestRoad
            // 
            this.pbLongestRoad.Image = global::SettlersOfCatan.Properties.Resources.LongestRoad;
            this.pbLongestRoad.Location = new System.Drawing.Point(140, 47);
            this.pbLongestRoad.Name = "pbLongestRoad";
            this.pbLongestRoad.Size = new System.Drawing.Size(32, 32);
            this.pbLongestRoad.TabIndex = 7;
            this.pbLongestRoad.TabStop = false;
            this.pbLongestRoad.Visible = false;
            // 
            // pbLargestArmy
            // 
            this.pbLargestArmy.Image = global::SettlersOfCatan.Properties.Resources.LargestArmy;
            this.pbLargestArmy.Location = new System.Drawing.Point(102, 47);
            this.pbLargestArmy.Name = "pbLargestArmy";
            this.pbLargestArmy.Size = new System.Drawing.Size(32, 32);
            this.pbLargestArmy.TabIndex = 7;
            this.pbLargestArmy.TabStop = false;
            this.pbLargestArmy.Visible = false;
            // 
            // lblTurn
            // 
            this.lblTurn.BackColor = System.Drawing.Color.Yellow;
            this.lblTurn.Location = new System.Drawing.Point(141, 9);
            this.lblTurn.Name = "lblTurn";
            this.lblTurn.Size = new System.Drawing.Size(32, 32);
            this.lblTurn.TabIndex = 8;
            this.lblTurn.Text = "Your Turn";
            this.lblTurn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTurn.Visible = false;
            // 
            // pnlDevCards
            // 
            this.pnlDevCards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDevCards.AutoScroll = true;
            this.pnlDevCards.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlDevCards.Location = new System.Drawing.Point(3, 144);
            this.pnlDevCards.Name = "pnlDevCards";
            this.pnlDevCards.Size = new System.Drawing.Size(244, 128);
            this.pnlDevCards.TabIndex = 9;
            // 
            // pnlResources
            // 
            this.pnlResources.Location = new System.Drawing.Point(3, 86);
            this.pnlResources.Name = "pnlResources";
            this.pnlResources.Size = new System.Drawing.Size(244, 52);
            this.pnlResources.TabIndex = 10;
            // 
            // playerComboBox
            // 
            this.playerComboBox.FormattingEnabled = true;
            this.playerComboBox.Location = new System.Drawing.Point(11, 52);
            this.playerComboBox.Name = "playerComboBox";
            this.playerComboBox.Size = new System.Drawing.Size(85, 21);
            this.playerComboBox.TabIndex = 0;
            this.playerComboBox.Items.AddRange(AgentManager.availableAgents.ToArray());
            // 
            // Player
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.playerComboBox);
            this.Controls.Add(this.pnlResources);
            this.Controls.Add(this.pnlDevCards);
            this.Controls.Add(this.lblTurn);
            this.Controls.Add(this.pbLargestArmy);
            this.Controls.Add(this.pbLongestRoad);
            this.Controls.Add(this.pnlPlayerColor);
            this.Controls.Add(this.panel1);
            this.Name = "Player";
            this.Size = new System.Drawing.Size(249, 277);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlPlayerColor.ResumeLayout(false);
            this.pnlPlayerColor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLongestRoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLargestArmy)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblVictoryPoints;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblPlayerColorName;
        private System.Windows.Forms.Panel pnlPlayerColor;
        private System.Windows.Forms.PictureBox pbLongestRoad;
        private System.Windows.Forms.PictureBox pbLargestArmy;
        private System.Windows.Forms.Label lblTurn;
        private System.Windows.Forms.Panel pnlDevCards;
        private System.Windows.Forms.Panel pnlResources;
        private System.Windows.Forms.ComboBox playerComboBox;
    }
}
