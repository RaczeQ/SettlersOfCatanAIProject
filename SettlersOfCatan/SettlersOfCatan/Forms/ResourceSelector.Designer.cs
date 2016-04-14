namespace SettlersOfCatan.Forms
{
    partial class ResourceSelector
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
            this.lblCount = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnDecrease = new System.Windows.Forms.Button();
            this.btnIncrease = new System.Windows.Forms.Button();
            this.lblResName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(46, 65);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(13, 13);
            this.lblCount.TabIndex = 0;
            this.lblCount.Text = "0";
            this.lblCount.Click += new System.EventHandler(this.ChildClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(37, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.ChildClicked);
            // 
            // btnDecrease
            // 
            this.btnDecrease.Location = new System.Drawing.Point(15, 60);
            this.btnDecrease.Name = "btnDecrease";
            this.btnDecrease.Size = new System.Drawing.Size(25, 23);
            this.btnDecrease.TabIndex = 2;
            this.btnDecrease.Text = "-";
            this.btnDecrease.UseVisualStyleBackColor = true;
            this.btnDecrease.Click += new System.EventHandler(this.btnDecrease_Click);
            // 
            // btnIncrease
            // 
            this.btnIncrease.Location = new System.Drawing.Point(65, 60);
            this.btnIncrease.Name = "btnIncrease";
            this.btnIncrease.Size = new System.Drawing.Size(25, 23);
            this.btnIncrease.TabIndex = 2;
            this.btnIncrease.Text = "+";
            this.btnIncrease.UseVisualStyleBackColor = true;
            this.btnIncrease.Click += new System.EventHandler(this.btnIncrease_Click);
            // 
            // lblResName
            // 
            this.lblResName.Location = new System.Drawing.Point(3, 0);
            this.lblResName.Name = "lblResName";
            this.lblResName.Size = new System.Drawing.Size(100, 20);
            this.lblResName.TabIndex = 3;
            this.lblResName.Text = "Resource";
            this.lblResName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblResName.Click += new System.EventHandler(this.ChildClicked);
            // 
            // ResourceSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblResName);
            this.Controls.Add(this.btnIncrease);
            this.Controls.Add(this.btnDecrease);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblCount);
            this.Name = "ResourceSelector";
            this.Size = new System.Drawing.Size(106, 90);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnDecrease;
        private System.Windows.Forms.Button btnIncrease;
        private System.Windows.Forms.Label lblResName;
    }
}
