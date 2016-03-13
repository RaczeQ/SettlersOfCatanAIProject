namespace SettlersOfCatan
{
    partial class Board
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlBoardArea = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlBoardArea
            // 
            this.pnlBoardArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBoardArea.BackgroundImage = global::SettlersOfCatan.Properties.Resources.ocean_surface;
            this.pnlBoardArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlBoardArea.Location = new System.Drawing.Point(12, 12);
            this.pnlBoardArea.Name = "pnlBoardArea";
            this.pnlBoardArea.Size = new System.Drawing.Size(1087, 715);
            this.pnlBoardArea.TabIndex = 0;
            // 
            // Board
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1202, 789);
            this.Controls.Add(this.pnlBoardArea);
            this.Name = "Board";
            this.Text = "Settlers of Catan";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBoardArea;
    }
}

