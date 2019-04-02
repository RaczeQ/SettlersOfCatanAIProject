using System.Windows.Forms;

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
            this.pictureBox17 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pnlUpgradedToolTip = new System.Windows.Forms.Panel();
            this.pictureBox16 = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pnlOwnedToolTip = new System.Windows.Forms.Panel();
            this.pictureBox14 = new System.Windows.Forms.PictureBox();
            this.pictureBox15 = new System.Windows.Forms.PictureBox();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.pictureBox12 = new System.Windows.Forms.PictureBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlCityToolTip = new System.Windows.Forms.Panel();
            this.lstGameEvents = new System.Windows.Forms.ListBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlDevelopmentCardToolTip = new System.Windows.Forms.Panel();
            this.pbBuildDevelopmentCard = new System.Windows.Forms.PictureBox();
            this.btnEndTurn = new System.Windows.Forms.Button();
            this.btnBankTrade = new System.Windows.Forms.Button();
            this.btnSetupBoard = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numberOfPlayersLabel = new System.Windows.Forms.Label();
            this.numberOfPlayers = new System.Windows.Forms.NumericUpDown();
            this.numberOfPlayers.ValueChanged += new System.EventHandler(this.numberOfPlayersChanged);
            this.btnStartGame = new System.Windows.Forms.Button();
            this.dice = new SettlersOfCatan.GameObjects.Dice();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlSettlementToolTip = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlRoadToolTip = new System.Windows.Forms.Panel();
            this.pnlPlayerInfo = new System.Windows.Forms.Panel();
            this.pnlBoardArea = new SettlersOfCatan.TransparencyFix.BoardArea();
            this.btnCheat = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox17)).BeginInit();
            this.pnlUpgradedToolTip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox16)).BeginInit();
            this.pnlOwnedToolTip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            this.pnlCityToolTip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            this.pnlDevelopmentCardToolTip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBuildDevelopmentCard)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfPlayers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.pnlSettlementToolTip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlRoadToolTip.SuspendLayout();
            this.pnlBoardArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox17
            // 
            this.pictureBox17.Image = global::SettlersOfCatan.Properties.Resources.Error;
            this.pictureBox17.Location = new System.Drawing.Point(29, 20);
            this.pictureBox17.Name = "pictureBox17";
            this.pictureBox17.Size = new System.Drawing.Size(32, 32);
            this.pictureBox17.TabIndex = 0;
            this.pictureBox17.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(78, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Fully Upgraded";
            // 
            // pnlUpgradedToolTip
            // 
            this.pnlUpgradedToolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(156)))), ((int)(((byte)(126)))));
            this.pnlUpgradedToolTip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUpgradedToolTip.Controls.Add(this.label10);
            this.pnlUpgradedToolTip.Controls.Add(this.pictureBox17);
            this.pnlUpgradedToolTip.Location = new System.Drawing.Point(800, 644);
            this.pnlUpgradedToolTip.Name = "pnlUpgradedToolTip";
            this.pnlUpgradedToolTip.Size = new System.Drawing.Size(90, 54);
            this.pnlUpgradedToolTip.TabIndex = 3;
            this.pnlUpgradedToolTip.Visible = false;
            // 
            // pictureBox16
            // 
            this.pictureBox16.Image = global::SettlersOfCatan.Properties.Resources.Error;
            this.pictureBox16.Location = new System.Drawing.Point(29, 20);
            this.pictureBox16.Name = "pictureBox16";
            this.pictureBox16.Size = new System.Drawing.Size(32, 32);
            this.pictureBox16.TabIndex = 0;
            this.pictureBox16.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Already Owned";
            // 
            // pnlOwnedToolTip
            // 
            this.pnlOwnedToolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(156)))), ((int)(((byte)(126)))));
            this.pnlOwnedToolTip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOwnedToolTip.Controls.Add(this.label9);
            this.pnlOwnedToolTip.Controls.Add(this.pictureBox16);
            this.pnlOwnedToolTip.Location = new System.Drawing.Point(701, 644);
            this.pnlOwnedToolTip.Name = "pnlOwnedToolTip";
            this.pnlOwnedToolTip.Size = new System.Drawing.Size(90, 54);
            this.pnlOwnedToolTip.TabIndex = 3;
            this.pnlOwnedToolTip.Visible = false;
            // 
            // pictureBox14
            // 
            this.pictureBox14.Image = global::SettlersOfCatan.Properties.Resources.Ore_Icon;
            this.pictureBox14.Location = new System.Drawing.Point(139, 20);
            this.pictureBox14.Name = "pictureBox14";
            this.pictureBox14.Size = new System.Drawing.Size(32, 32);
            this.pictureBox14.TabIndex = 0;
            this.pictureBox14.TabStop = false;
            // 
            // pictureBox15
            // 
            this.pictureBox15.Image = global::SettlersOfCatan.Properties.Resources.Ore_Icon;
            this.pictureBox15.Location = new System.Drawing.Point(171, 20);
            this.pictureBox15.Name = "pictureBox15";
            this.pictureBox15.Size = new System.Drawing.Size(32, 32);
            this.pictureBox15.TabIndex = 0;
            this.pictureBox15.TabStop = false;
            // 
            // pictureBox13
            // 
            this.pictureBox13.Image = global::SettlersOfCatan.Properties.Resources.Ore_Icon;
            this.pictureBox13.Location = new System.Drawing.Point(107, 20);
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.Size = new System.Drawing.Size(32, 32);
            this.pictureBox13.TabIndex = 0;
            this.pictureBox13.TabStop = false;
            // 
            // pictureBox12
            // 
            this.pictureBox12.Image = global::SettlersOfCatan.Properties.Resources.Wheat_Icon;
            this.pictureBox12.Location = new System.Drawing.Point(75, 20);
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.Size = new System.Drawing.Size(32, 32);
            this.pictureBox12.TabIndex = 0;
            this.pictureBox12.TabStop = false;
            // 
            // pictureBox11
            // 
            this.pictureBox11.Image = global::SettlersOfCatan.Properties.Resources.Wheat_Icon;
            this.pictureBox11.Location = new System.Drawing.Point(43, 20);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(32, 32);
            this.pictureBox11.TabIndex = 0;
            this.pictureBox11.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "City: Click to Build";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "=2 VP";
            // 
            // pnlCityToolTip
            // 
            this.pnlCityToolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(156)))), ((int)(((byte)(126)))));
            this.pnlCityToolTip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCityToolTip.Controls.Add(this.label7);
            this.pnlCityToolTip.Controls.Add(this.label8);
            this.pnlCityToolTip.Controls.Add(this.pictureBox11);
            this.pnlCityToolTip.Controls.Add(this.pictureBox12);
            this.pnlCityToolTip.Controls.Add(this.pictureBox13);
            this.pnlCityToolTip.Controls.Add(this.pictureBox15);
            this.pnlCityToolTip.Controls.Add(this.pictureBox14);
            this.pnlCityToolTip.Location = new System.Drawing.Point(485, 644);
            this.pnlCityToolTip.Name = "pnlCityToolTip";
            this.pnlCityToolTip.Size = new System.Drawing.Size(205, 54);
            this.pnlCityToolTip.TabIndex = 3;
            this.pnlCityToolTip.Visible = false;
            // 
            // lstGameEvents
            // 
            this.lstGameEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstGameEvents.FormattingEnabled = true;
            this.lstGameEvents.Location = new System.Drawing.Point(484, 591);
            this.lstGameEvents.Name = "lstGameEvents";
            this.lstGameEvents.ScrollAlwaysVisible = true;
            this.lstGameEvents.Size = new System.Drawing.Size(492, 134);
            this.lstGameEvents.TabIndex = 6;
            // 
            // pictureBox9
            // 
            this.pictureBox9.Image = global::SettlersOfCatan.Properties.Resources.Ore_Icon;
            this.pictureBox9.Location = new System.Drawing.Point(134, 20);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(32, 32);
            this.pictureBox9.TabIndex = 0;
            this.pictureBox9.TabStop = false;
            // 
            // pictureBox8
            // 
            this.pictureBox8.Image = global::SettlersOfCatan.Properties.Resources.Wheat_Icon;
            this.pictureBox8.Location = new System.Drawing.Point(102, 20);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(32, 32);
            this.pictureBox8.TabIndex = 0;
            this.pictureBox8.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = global::SettlersOfCatan.Properties.Resources.Sheep_Icon;
            this.pictureBox7.Location = new System.Drawing.Point(70, 21);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(32, 32);
            this.pictureBox7.TabIndex = 0;
            this.pictureBox7.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Development Card: Click to Purchase";
            // 
            // pictureBox10
            // 
            this.pictureBox10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox10.Image = global::SettlersOfCatan.Properties.Resources.Development_Card_Back;
            this.pictureBox10.Location = new System.Drawing.Point(3, 20);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(20, 32);
            this.pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox10.TabIndex = 4;
            this.pictureBox10.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "=? VPs";
            // 
            // pnlDevelopmentCardToolTip
            // 
            this.pnlDevelopmentCardToolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(156)))), ((int)(((byte)(126)))));
            this.pnlDevelopmentCardToolTip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDevelopmentCardToolTip.Controls.Add(this.label5);
            this.pnlDevelopmentCardToolTip.Controls.Add(this.pictureBox10);
            this.pnlDevelopmentCardToolTip.Controls.Add(this.label6);
            this.pnlDevelopmentCardToolTip.Controls.Add(this.pictureBox7);
            this.pnlDevelopmentCardToolTip.Controls.Add(this.pictureBox8);
            this.pnlDevelopmentCardToolTip.Controls.Add(this.pictureBox9);
            this.pnlDevelopmentCardToolTip.Location = new System.Drawing.Point(773, 586);
            this.pnlDevelopmentCardToolTip.Name = "pnlDevelopmentCardToolTip";
            this.pnlDevelopmentCardToolTip.Size = new System.Drawing.Size(188, 54);
            this.pnlDevelopmentCardToolTip.TabIndex = 3;
            this.pnlDevelopmentCardToolTip.Visible = false;
            // 
            // pbBuildDevelopmentCard
            // 
            this.pbBuildDevelopmentCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbBuildDevelopmentCard.Image = global::SettlersOfCatan.Properties.Resources.Development_Card_Back;
            this.pbBuildDevelopmentCard.Location = new System.Drawing.Point(8, 29);
            this.pbBuildDevelopmentCard.Name = "pbBuildDevelopmentCard";
            this.pbBuildDevelopmentCard.Size = new System.Drawing.Size(64, 92);
            this.pbBuildDevelopmentCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbBuildDevelopmentCard.TabIndex = 4;
            this.pbBuildDevelopmentCard.TabStop = false;
            // 
            // btnEndTurn
            // 
            this.btnEndTurn.Location = new System.Drawing.Point(78, 63);
            this.btnEndTurn.Name = "btnEndTurn";
            this.btnEndTurn.Size = new System.Drawing.Size(122, 23);
            this.btnEndTurn.TabIndex = 7;
            this.btnEndTurn.Text = "End Turn";
            this.btnEndTurn.UseVisualStyleBackColor = true;
            // 
            // btnBankTrade
            // 
            this.btnBankTrade.Location = new System.Drawing.Point(78, 29);
            this.btnBankTrade.Name = "btnBankTrade";
            this.btnBankTrade.Size = new System.Drawing.Size(122, 23);
            this.btnBankTrade.TabIndex = 8;
            this.btnBankTrade.Text = "Trade With Bank";
            this.btnBankTrade.UseVisualStyleBackColor = true;
            // 
            // btnSetupBoard
            // 
            this.btnSetupBoard.Location = new System.Drawing.Point(206, 29);
            this.btnSetupBoard.Name = "btnSetupBoard";
            this.btnSetupBoard.Size = new System.Drawing.Size(105, 23);
            this.btnSetupBoard.TabIndex = 9;
            this.btnSetupBoard.Text = "Set Up Board";
            this.btnSetupBoard.UseVisualStyleBackColor = true;
            this.btnSetupBoard.Click += new System.EventHandler(this.btnSetupBoard_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.groupBox1.Controls.Add(this.numberOfPlayersLabel);
            this.groupBox1.Controls.Add(this.numberOfPlayers);
            this.groupBox1.Controls.Add(this.btnStartGame);
            this.groupBox1.Controls.Add(this.btnSetupBoard);
            this.groupBox1.Controls.Add(this.btnBankTrade);
            this.groupBox1.Controls.Add(this.btnEndTurn);
            this.groupBox1.Controls.Add(this.dice);
            this.groupBox1.Controls.Add(this.pbBuildDevelopmentCard);
            this.groupBox1.Location = new System.Drawing.Point(12, 585);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(465, 138);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Actions";
            // 
            // numberOfPlayersLabel
            // 
            this.numberOfPlayersLabel.AutoSize = true;
            this.numberOfPlayersLabel.Location = new System.Drawing.Point(211, 68);
            this.numberOfPlayersLabel.Name = "numberOfPlayersLabel";
            this.numberOfPlayersLabel.Size = new System.Drawing.Size(92, 13);
            this.numberOfPlayersLabel.TabIndex = 12;
            this.numberOfPlayersLabel.Text = "Number of players";
            // 
            // numberOfPlayers
            // 
            this.numberOfPlayers.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.numberOfPlayers.Location = new System.Drawing.Point(206, 101);
            this.numberOfPlayers.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numberOfPlayers.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numberOfPlayers.Name = "numberOfPlayers";
            this.numberOfPlayers.Size = new System.Drawing.Size(105, 20);
            this.numberOfPlayers.TabIndex = 11;
            this.numberOfPlayers.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // btnStartGame
            // 
            this.btnStartGame.Location = new System.Drawing.Point(206, 29);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(105, 23);
            this.btnStartGame.TabIndex = 10;
            this.btnStartGame.Text = "Start Game";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Visible = false;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
            // 
            // dice
            // 
            this.dice.Location = new System.Drawing.Point(317, 14);
            this.dice.Name = "dice";
            this.dice.Size = new System.Drawing.Size(142, 118);
            this.dice.TabIndex = 5;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::SettlersOfCatan.Properties.Resources.Sheep_Icon;
            this.pictureBox6.Location = new System.Drawing.Point(139, 20);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(32, 32);
            this.pictureBox6.TabIndex = 0;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::SettlersOfCatan.Properties.Resources.Wheat_Icon;
            this.pictureBox5.Location = new System.Drawing.Point(107, 20);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(32, 32);
            this.pictureBox5.TabIndex = 0;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::SettlersOfCatan.Properties.Resources.Wood_Icon;
            this.pictureBox4.Location = new System.Drawing.Point(75, 20);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(32, 32);
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::SettlersOfCatan.Properties.Resources.Brick_Icon;
            this.pictureBox3.Location = new System.Drawing.Point(43, 20);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(32, 32);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Settlement: Click to Build";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "=1 VP";
            // 
            // pnlSettlementToolTip
            // 
            this.pnlSettlementToolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(156)))), ((int)(((byte)(126)))));
            this.pnlSettlementToolTip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSettlementToolTip.Controls.Add(this.label3);
            this.pnlSettlementToolTip.Controls.Add(this.label4);
            this.pnlSettlementToolTip.Controls.Add(this.pictureBox3);
            this.pnlSettlementToolTip.Controls.Add(this.pictureBox4);
            this.pnlSettlementToolTip.Controls.Add(this.pictureBox5);
            this.pnlSettlementToolTip.Controls.Add(this.pictureBox6);
            this.pnlSettlementToolTip.Location = new System.Drawing.Point(483, 586);
            this.pnlSettlementToolTip.Name = "pnlSettlementToolTip";
            this.pnlSettlementToolTip.Size = new System.Drawing.Size(173, 54);
            this.pnlSettlementToolTip.TabIndex = 3;
            this.pnlSettlementToolTip.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::SettlersOfCatan.Properties.Resources.Wood_Icon;
            this.pictureBox2.Location = new System.Drawing.Point(75, 20);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SettlersOfCatan.Properties.Resources.Brick_Icon;
            this.pictureBox1.Location = new System.Drawing.Point(43, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Road: Click to Build";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "=0 VP";
            // 
            // pnlRoadToolTip
            // 
            this.pnlRoadToolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(156)))), ((int)(((byte)(126)))));
            this.pnlRoadToolTip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRoadToolTip.Controls.Add(this.label2);
            this.pnlRoadToolTip.Controls.Add(this.label1);
            this.pnlRoadToolTip.Controls.Add(this.pictureBox1);
            this.pnlRoadToolTip.Controls.Add(this.pictureBox2);
            this.pnlRoadToolTip.Location = new System.Drawing.Point(659, 586);
            this.pnlRoadToolTip.Name = "pnlRoadToolTip";
            this.pnlRoadToolTip.Size = new System.Drawing.Size(109, 54);
            this.pnlRoadToolTip.TabIndex = 1;
            this.pnlRoadToolTip.Visible = false;
            // 
            // pnlPlayerInfo
            // 
            this.pnlPlayerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPlayerInfo.AutoScroll = true;
            this.pnlPlayerInfo.BackColor = System.Drawing.Color.Transparent;
            this.pnlPlayerInfo.Location = new System.Drawing.Point(984, 0);
            this.pnlPlayerInfo.Name = "pnlPlayerInfo";
            this.pnlPlayerInfo.Size = new System.Drawing.Size(286, 736);
            this.pnlPlayerInfo.TabIndex = 5;
            // 
            // pnlBoardArea
            // 
            this.pnlBoardArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBoardArea.AutoScroll = true;
            this.pnlBoardArea.BackColor = System.Drawing.Color.White;
            this.pnlBoardArea.BackgroundImage = global::SettlersOfCatan.Properties.Resources.ocean_surface;
            this.pnlBoardArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlBoardArea.Controls.Add(this.btnCheat);
            this.pnlBoardArea.Location = new System.Drawing.Point(0, 0);
            this.pnlBoardArea.Name = "pnlBoardArea";
            this.pnlBoardArea.Size = new System.Drawing.Size(978, 580);
            this.pnlBoardArea.TabIndex = 0;
            // 
            // btnCheat
            // 
            this.btnCheat.BackColor = System.Drawing.Color.Transparent;
            this.btnCheat.Location = new System.Drawing.Point(860, 3);
            this.btnCheat.Name = "btnCheat";
            this.btnCheat.Size = new System.Drawing.Size(142, 23);
            this.btnCheat.TabIndex = 10;
            this.btnCheat.Text = "Super Secret Cheat Button";
            this.btnCheat.UseVisualStyleBackColor = false;
            this.btnCheat.Click += new System.EventHandler(this.btnCheat_Click);
            // 
            // Board
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1268, 735);
            this.WindowState = FormWindowState.Maximized;
            this.Controls.Add(this.lstGameEvents);
            this.Controls.Add(this.pnlPlayerInfo);
            this.Controls.Add(this.pnlDevelopmentCardToolTip);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlUpgradedToolTip);
            this.Controls.Add(this.pnlOwnedToolTip);
            this.Controls.Add(this.pnlCityToolTip);
            this.Controls.Add(this.pnlSettlementToolTip);
            this.Controls.Add(this.pnlRoadToolTip);
            this.Controls.Add(this.pnlBoardArea);
            this.Name = "Board";
            this.Text = "Settlers of Catan";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox17)).EndInit();
            this.pnlUpgradedToolTip.ResumeLayout(false);
            this.pnlUpgradedToolTip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox16)).EndInit();
            this.pnlOwnedToolTip.ResumeLayout(false);
            this.pnlOwnedToolTip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            this.pnlCityToolTip.ResumeLayout(false);
            this.pnlCityToolTip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            this.pnlDevelopmentCardToolTip.ResumeLayout(false);
            this.pnlDevelopmentCardToolTip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBuildDevelopmentCard)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfPlayers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.pnlSettlementToolTip.ResumeLayout(false);
            this.pnlSettlementToolTip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlRoadToolTip.ResumeLayout(false);
            this.pnlRoadToolTip.PerformLayout();
            this.pnlBoardArea.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCheat;
        private TransparencyFix.BoardArea pnlBoardArea;
        private System.Windows.Forms.PictureBox pictureBox17;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.Panel pnlUpgradedToolTip;
        private System.Windows.Forms.PictureBox pictureBox16;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.Panel pnlOwnedToolTip;
        private System.Windows.Forms.PictureBox pictureBox14;
        private System.Windows.Forms.PictureBox pictureBox15;
        private System.Windows.Forms.PictureBox pictureBox13;
        private System.Windows.Forms.PictureBox pictureBox12;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.Panel pnlCityToolTip;
        private System.Windows.Forms.ListBox lstGameEvents;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox10;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Panel pnlDevelopmentCardToolTip;
        public System.Windows.Forms.PictureBox pbBuildDevelopmentCard;
        public SettlersOfCatan.GameObjects.Dice dice;
        public System.Windows.Forms.Button btnEndTurn;
        public System.Windows.Forms.Button btnBankTrade;
        private System.Windows.Forms.Button btnSetupBoard;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Panel pnlSettlementToolTip;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Panel pnlRoadToolTip;
        private System.Windows.Forms.Panel pnlPlayerInfo;
        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.NumericUpDown numberOfPlayers;
        private System.Windows.Forms.Label numberOfPlayersLabel;
    }
}

