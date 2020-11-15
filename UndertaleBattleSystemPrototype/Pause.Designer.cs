namespace UndertaleBattleSystemPrototype
{
    partial class Pause
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
            this.components = new System.ComponentModel.Container();
            this.pauseTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // pauseTimer
            // 
            this.pauseTimer.Enabled = true;
            this.pauseTimer.Interval = 20;
            this.pauseTimer.Tick += new System.EventHandler(this.pauseTimer_Tick);
            // 
            // Pause
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::UndertaleBattleSystemPrototype.Properties.Resources.PauseFormBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Pause";
            this.Text = "Pause";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Pause_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Pause_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Pause_PreviewKeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer pauseTimer;
    }
}