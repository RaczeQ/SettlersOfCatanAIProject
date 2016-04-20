namespace SettlersOfCatan
{
    partial class TradeWindow
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
            this.lblPlayerTitle = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.pbArrowLeft = new System.Windows.Forms.PictureBox();
            this.pbArrowRight = new System.Windows.Forms.PictureBox();
            this.pnlPlayer = new System.Windows.Forms.Panel();
            this.pnlOther = new System.Windows.Forms.Panel();
            this.btnClearSelection = new System.Windows.Forms.Button();
            this.lblTradeName = new System.Windows.Forms.Label();
            this.lblInstructions = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbArrowLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbArrowRight)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPlayerTitle
            // 
            this.lblPlayerTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayerTitle.Location = new System.Drawing.Point(13, 5);
            this.lblPlayerTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPlayerTitle.Name = "lblPlayerTitle";
            this.lblPlayerTitle.Size = new System.Drawing.Size(132, 42);
            this.lblPlayerTitle.TabIndex = 5;
            this.lblPlayerTitle.Text = "Player";
            this.lblPlayerTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(11, 416);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccept.Location = new System.Drawing.Point(274, 416);
            this.btnAccept.Margin = new System.Windows.Forms.Padding(2);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(84, 25);
            this.btnAccept.TabIndex = 8;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // pbArrowLeft
            // 
            this.pbArrowLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbArrowLeft.Location = new System.Drawing.Point(150, 335);
            this.pbArrowLeft.Margin = new System.Windows.Forms.Padding(2);
            this.pbArrowLeft.Name = "pbArrowLeft";
            this.pbArrowLeft.Size = new System.Drawing.Size(67, 32);
            this.pbArrowLeft.TabIndex = 4;
            this.pbArrowLeft.TabStop = false;
            // 
            // pbArrowRight
            // 
            this.pbArrowRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbArrowRight.Location = new System.Drawing.Point(150, 93);
            this.pbArrowRight.Margin = new System.Windows.Forms.Padding(2);
            this.pbArrowRight.Name = "pbArrowRight";
            this.pbArrowRight.Size = new System.Drawing.Size(67, 32);
            this.pbArrowRight.TabIndex = 3;
            this.pbArrowRight.TabStop = false;
            // 
            // pnlPlayer
            // 
            this.pnlPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlPlayer.AutoScroll = true;
            this.pnlPlayer.BackColor = System.Drawing.Color.White;
            this.pnlPlayer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlPlayer.Location = new System.Drawing.Point(13, 50);
            this.pnlPlayer.Name = "pnlPlayer";
            this.pnlPlayer.Size = new System.Drawing.Size(132, 337);
            this.pnlPlayer.TabIndex = 21;
            // 
            // pnlOther
            // 
            this.pnlOther.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOther.AutoScroll = true;
            this.pnlOther.BackColor = System.Drawing.Color.White;
            this.pnlOther.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlOther.Location = new System.Drawing.Point(223, 50);
            this.pnlOther.Name = "pnlOther";
            this.pnlOther.Size = new System.Drawing.Size(132, 337);
            this.pnlOther.TabIndex = 21;
            // 
            // btnClearSelection
            // 
            this.btnClearSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearSelection.Location = new System.Drawing.Point(130, 392);
            this.btnClearSelection.Margin = new System.Windows.Forms.Padding(2);
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Size = new System.Drawing.Size(100, 25);
            this.btnClearSelection.TabIndex = 7;
            this.btnClearSelection.Text = "Clear Selection";
            this.btnClearSelection.UseVisualStyleBackColor = true;
            this.btnClearSelection.Click += new System.EventHandler(this.clearSelection);
            // 
            // lblTradeName
            // 
            this.lblTradeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTradeName.Location = new System.Drawing.Point(223, 5);
            this.lblTradeName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTradeName.Name = "lblTradeName";
            this.lblTradeName.Size = new System.Drawing.Size(132, 42);
            this.lblTradeName.TabIndex = 5;
            this.lblTradeName.Text = "Other";
            this.lblTradeName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInstructions
            // 
            this.lblInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstructions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblInstructions.Location = new System.Drawing.Point(173, 127);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(177, 140);
            this.lblInstructions.TabIndex = 22;
            this.lblInstructions.Text = "You have selected the required number of resources. Click accept to continue or c" +
    "lear selection to start over.";
            this.lblInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TradeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 452);
            this.ControlBox = false;
            this.Controls.Add(this.pnlOther);
            this.Controls.Add(this.pnlPlayer);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.btnClearSelection);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblTradeName);
            this.Controls.Add(this.lblPlayerTitle);
            this.Controls.Add(this.pbArrowLeft);
            this.Controls.Add(this.pbArrowRight);
            this.Controls.Add(this.lblInstructions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TradeWindow";
            this.Text = "Trade Window";
            ((System.ComponentModel.ISupportInitialize)(this.pbArrowLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbArrowRight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pbArrowRight;
        private System.Windows.Forms.PictureBox pbArrowLeft;
        private System.Windows.Forms.Label lblPlayerTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Panel pnlPlayer;
        private System.Windows.Forms.Panel pnlOther;
        private System.Windows.Forms.Button btnClearSelection;
        private System.Windows.Forms.Label lblTradeName;
        private System.Windows.Forms.Label lblInstructions;
    }
}