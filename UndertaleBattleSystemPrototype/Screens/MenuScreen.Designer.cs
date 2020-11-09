namespace UndertaleBattleSystemPrototype
{
    partial class MenuScreen
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
            this.titleSprite = new System.Windows.Forms.PictureBox();
            this.menuTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.titleSprite)).BeginInit();
            this.SuspendLayout();
            // 
            // titleSprite
            // 
            this.titleSprite.BackgroundImage = global::UndertaleBattleSystemPrototype.Properties.Resources.titleSprite;
            this.titleSprite.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.titleSprite.Location = new System.Drawing.Point(222, 100);
            this.titleSprite.Name = "titleSprite";
            this.titleSprite.Size = new System.Drawing.Size(500, 200);
            this.titleSprite.TabIndex = 0;
            this.titleSprite.TabStop = false;
            // 
            // menuTimer
            // 
            this.menuTimer.Enabled = true;
            this.menuTimer.Interval = 20;
            this.menuTimer.Tick += new System.EventHandler(this.menuTimer_Tick);
            // 
            // MenuScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.titleSprite);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MenuScreen";
            this.Size = new System.Drawing.Size(944, 681);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MenuScreen_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MenuScreen_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MenuScreen_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.titleSprite)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox titleSprite;
        private System.Windows.Forms.Timer menuTimer;
    }
}
