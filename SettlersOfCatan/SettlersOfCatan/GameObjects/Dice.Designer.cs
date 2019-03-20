namespace SettlersOfCatan.GameObjects
{
    partial class Dice
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
            this.pbYellowDie = new System.Windows.Forms.PictureBox();
            this.pbRedDie = new System.Windows.Forms.PictureBox();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.lblRollValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbYellowDie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRedDie)).BeginInit();
            this.SuspendLayout();
            // 
            // pbYellowDie
            // 
            this.pbYellowDie.Image = global::SettlersOfCatan.Properties.Resources._1_Die_Face_Red;
            this.pbYellowDie.Location = new System.Drawing.Point(73, 26);
            this.pbYellowDie.Name = "pbYellowDie";
            this.pbYellowDie.Size = new System.Drawing.Size(64, 64);
            this.pbYellowDie.TabIndex = 0;
            this.pbYellowDie.TabStop = false;
            this.pbYellowDie.Click += new System.EventHandler(this.childClicked);
            // 
            // pbRedDie
            // 
            this.pbRedDie.Image = global::SettlersOfCatan.Properties.Resources._1_Die_Face_Yellow;
            this.pbRedDie.Location = new System.Drawing.Point(3, 26);
            this.pbRedDie.Name = "pbRedDie";
            this.pbRedDie.Size = new System.Drawing.Size(64, 64);
            this.pbRedDie.TabIndex = 0;
            this.pbRedDie.TabStop = false;
            this.pbRedDie.Click += new System.EventHandler(this.childClicked);
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Location = new System.Drawing.Point(36, 10);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(63, 13);
            this.lblInstructions.TabIndex = 1;
            this.lblInstructions.Text = "Click to Roll";
            this.lblInstructions.Click += new System.EventHandler(this.childClicked);
            // 
            // lblRollValue
            // 
            this.lblRollValue.AutoSize = true;
            this.lblRollValue.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRollValue.Location = new System.Drawing.Point(61, 95);
            this.lblRollValue.Name = "lblRollValue";
            this.lblRollValue.Size = new System.Drawing.Size(18, 18);
            this.lblRollValue.TabIndex = 2;
            this.lblRollValue.Text = "2";
            this.lblRollValue.Click += new System.EventHandler(this.childClicked);
            // 
            // Dice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblRollValue);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.pbYellowDie);
            this.Controls.Add(this.pbRedDie);
            this.Name = "Dice";
            this.Size = new System.Drawing.Size(142, 118);
            this.Load += new System.EventHandler(this.Dice_Load);
            this.EnabledChanged += new System.EventHandler(this.Dice_Enable);
            this.Click += new System.EventHandler(this.Dice_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pbYellowDie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRedDie)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbRedDie;
        private System.Windows.Forms.PictureBox pbYellowDie;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Label lblRollValue;
    }
}
