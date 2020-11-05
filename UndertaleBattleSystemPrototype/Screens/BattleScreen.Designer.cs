﻿namespace UndertaleBattleSystemPrototype
{
    partial class BattleScreen
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
            this.components = new System.ComponentModel.Container();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.textOutput = new System.Windows.Forms.Label();
            this.actLabel1 = new System.Windows.Forms.Label();
            this.actLabel2 = new System.Windows.Forms.Label();
            this.actLabel3 = new System.Windows.Forms.Label();
            this.actLabel4 = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.hpLabel = new System.Windows.Forms.Label();
            this.hpValueLabel = new System.Windows.Forms.Label();
            this.damageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Interval = 20;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // textOutput
            // 
            this.textOutput.BackColor = System.Drawing.Color.Transparent;
            this.textOutput.Enabled = false;
            this.textOutput.Font = new System.Drawing.Font("Determination Mono Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textOutput.ForeColor = System.Drawing.Color.White;
            this.textOutput.Location = new System.Drawing.Point(119, 515);
            this.textOutput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.textOutput.Name = "textOutput";
            this.textOutput.Size = new System.Drawing.Size(1124, 219);
            this.textOutput.TabIndex = 0;
            this.textOutput.Text = "* Hello. \r\n\r\n* Welcome to the Undetale Battle System Prototype.\r\n\r\n* Try some of " +
    "the buttons and menus out for testing.";
            // 
            // actLabel1
            // 
            this.actLabel1.BackColor = System.Drawing.Color.Transparent;
            this.actLabel1.Enabled = false;
            this.actLabel1.Font = new System.Drawing.Font("Determination Mono Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actLabel1.ForeColor = System.Drawing.Color.White;
            this.actLabel1.Location = new System.Drawing.Point(119, 515);
            this.actLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.actLabel1.Name = "actLabel1";
            this.actLabel1.Size = new System.Drawing.Size(378, 44);
            this.actLabel1.TabIndex = 1;
            this.actLabel1.Text = "Act1";
            this.actLabel1.Visible = false;
            // 
            // actLabel2
            // 
            this.actLabel2.BackColor = System.Drawing.Color.Transparent;
            this.actLabel2.Enabled = false;
            this.actLabel2.Font = new System.Drawing.Font("Determination Mono Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actLabel2.ForeColor = System.Drawing.Color.White;
            this.actLabel2.Location = new System.Drawing.Point(606, 515);
            this.actLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.actLabel2.Name = "actLabel2";
            this.actLabel2.Size = new System.Drawing.Size(378, 44);
            this.actLabel2.TabIndex = 2;
            this.actLabel2.Text = "Act2";
            this.actLabel2.Visible = false;
            // 
            // actLabel3
            // 
            this.actLabel3.BackColor = System.Drawing.Color.Transparent;
            this.actLabel3.Enabled = false;
            this.actLabel3.Font = new System.Drawing.Font("Determination Mono Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actLabel3.ForeColor = System.Drawing.Color.White;
            this.actLabel3.Location = new System.Drawing.Point(119, 624);
            this.actLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.actLabel3.Name = "actLabel3";
            this.actLabel3.Size = new System.Drawing.Size(378, 44);
            this.actLabel3.TabIndex = 3;
            this.actLabel3.Text = "Act3";
            this.actLabel3.Visible = false;
            // 
            // actLabel4
            // 
            this.actLabel4.BackColor = System.Drawing.Color.Transparent;
            this.actLabel4.Enabled = false;
            this.actLabel4.Font = new System.Drawing.Font("Determination Mono Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actLabel4.ForeColor = System.Drawing.Color.White;
            this.actLabel4.Location = new System.Drawing.Point(606, 624);
            this.actLabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.actLabel4.Name = "actLabel4";
            this.actLabel4.Size = new System.Drawing.Size(378, 44);
            this.actLabel4.TabIndex = 4;
            this.actLabel4.Text = "Act4";
            this.actLabel4.Visible = false;
            // 
            // nameLabel
            // 
            this.nameLabel.BackColor = System.Drawing.Color.Transparent;
            this.nameLabel.Enabled = false;
            this.nameLabel.Font = new System.Drawing.Font("Determination Sans Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.Color.White;
            this.nameLabel.Location = new System.Drawing.Point(104, 710);
            this.nameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(94, 44);
            this.nameLabel.TabIndex = 5;
            this.nameLabel.Text = "Nori";
            // 
            // hpLabel
            // 
            this.hpLabel.BackColor = System.Drawing.Color.Transparent;
            this.hpLabel.Enabled = false;
            this.hpLabel.Font = new System.Drawing.Font("Determination Sans Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpLabel.ForeColor = System.Drawing.Color.White;
            this.hpLabel.Location = new System.Drawing.Point(486, 710);
            this.hpLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.hpLabel.Name = "hpLabel";
            this.hpLabel.Size = new System.Drawing.Size(56, 44);
            this.hpLabel.TabIndex = 7;
            this.hpLabel.Text = "HP";
            // 
            // hpValueLabel
            // 
            this.hpValueLabel.BackColor = System.Drawing.Color.Transparent;
            this.hpValueLabel.Enabled = false;
            this.hpValueLabel.Font = new System.Drawing.Font("Determination Sans Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpValueLabel.ForeColor = System.Drawing.Color.White;
            this.hpValueLabel.Location = new System.Drawing.Point(656, 710);
            this.hpValueLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.hpValueLabel.Name = "hpValueLabel";
            this.hpValueLabel.Size = new System.Drawing.Size(128, 44);
            this.hpValueLabel.TabIndex = 8;
            this.hpValueLabel.Text = "40 / 40";
            // 
            // damageLabel
            // 
            this.damageLabel.BackColor = System.Drawing.Color.Transparent;
            this.damageLabel.Enabled = false;
            this.damageLabel.Font = new System.Drawing.Font("Determination Sans Web", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.damageLabel.ForeColor = System.Drawing.Color.Yellow;
            this.damageLabel.Location = new System.Drawing.Point(528, 125);
            this.damageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.damageLabel.Name = "damageLabel";
            this.damageLabel.Size = new System.Drawing.Size(125, 35);
            this.damageLabel.TabIndex = 9;
            this.damageLabel.Text = "0";
            this.damageLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.damageLabel.Visible = false;
            // 
            // BattleScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.damageLabel);
            this.Controls.Add(this.hpValueLabel);
            this.Controls.Add(this.hpLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.actLabel4);
            this.Controls.Add(this.actLabel3);
            this.Controls.Add(this.actLabel2);
            this.Controls.Add(this.actLabel1);
            this.Controls.Add(this.textOutput);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BattleScreen";
            this.Size = new System.Drawing.Size(1180, 851);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BattleScreen_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BattleScreen_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.BattleScreen_PreviewKeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Label textOutput;
        private System.Windows.Forms.Label actLabel1;
        private System.Windows.Forms.Label actLabel2;
        private System.Windows.Forms.Label actLabel3;
        private System.Windows.Forms.Label actLabel4;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label hpLabel;
        private System.Windows.Forms.Label hpValueLabel;
        private System.Windows.Forms.Label damageLabel;
    }
}
