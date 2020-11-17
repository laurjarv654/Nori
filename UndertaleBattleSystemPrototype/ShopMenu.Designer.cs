namespace UndertaleBattleSystemPrototype
{
    partial class ShopMenu
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
            this.shopTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // shopTimer
            // 
            this.shopTimer.Enabled = true;
            this.shopTimer.Interval = 20;
            this.shopTimer.Tick += new System.EventHandler(this.shopTimer_Tick);
            // 
            // ShopMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = global::UndertaleBattleSystemPrototype.Properties.Resources.ShopFormBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ShopMenu";
            this.Text = "ShopMenu";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShopMenu_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ShopMenu_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ShopMenu_PreviewKeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer shopTimer;
    }
}