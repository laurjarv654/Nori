namespace UndertaleBattleSystemPrototype
{
    partial class ControlsScreen
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
            this.tutorialTimer = new System.Windows.Forms.Timer(this.components);
            this.titleSprite = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.titleSprite)).BeginInit();
            this.SuspendLayout();
            // 
            // tutorialTimer
            // 
            this.tutorialTimer.Enabled = true;
            this.tutorialTimer.Interval = 20;
            this.tutorialTimer.Tick += new System.EventHandler(this.tutorialTimer_Tick);
            // 
            // titleSprite
            // 
            this.titleSprite.BackgroundImage = global::UndertaleBattleSystemPrototype.Properties.Resources.controlsSprite;
            this.titleSprite.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.titleSprite.Location = new System.Drawing.Point(242, 80);
            this.titleSprite.Name = "titleSprite";
            this.titleSprite.Size = new System.Drawing.Size(460, 85);
            this.titleSprite.TabIndex = 1;
            this.titleSprite.TabStop = false;
            // 
            // ControlsScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.titleSprite);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ControlsScreen";
            this.Size = new System.Drawing.Size(944, 861);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ControlsScreen_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ControlsScreen_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ControlsScreen_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.titleSprite)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tutorialTimer;
        private System.Windows.Forms.PictureBox titleSprite;
    }
}
