using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace UndertaleBattleSystemPrototype
{
    public partial class LoseScreen : UserControl
    {
        #region variable declaractions

        //string for the game over text
        string gameOverText = "It's not over yet...   Nori! Stay Determined!";
        string gameOverTextWritten = "";

        //booleans for key presses
        Boolean spaceDown;

        //rectangle for drawing the game over text into
        Rectangle textRec;

        //brush for menu buttons
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        #endregion variable declaractions

        public LoseScreen()
        {
            InitializeComponent();

            //initialize the text rec
            textRec = new Rectangle(this.Width / 4, this.Height - (this.Height / 4), this.Width / 2, this.Height / 8);
        }

        #region key down and up
        private void LoseScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player button presses
            switch (e.KeyCode)
            {
                case Keys.Space:
                    spaceDown = true;
                    break;
            }
        }

        private void LoseScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player button releases
            switch (e.KeyCode)
            {
                case Keys.Space:
                    spaceDown = false;
                    break;
            }
        }
        #endregion key down and up

        #region game timer
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //if the player presses space, go back to the town screen
            if (spaceDown == true)
            {
                //stop menu timer
                gameTimer.Enabled = false;

                // Goes to the town screen
                TownScreen ts = new TownScreen();
                Form form = this.FindForm();

                form.Controls.Add(ts);
                form.Controls.Remove(this);

                ts.Location = new Point((form.Width - ts.Width) / 2, (form.Height - ts.Height) / 2);
            }
            //else if the game over text hasn't been fully written out yet, continue to write it out
            else if (gameOverText != gameOverTextWritten)
            {
                foreach (char c in gameOverText)
                {
                    gameOverTextWritten += c;
                    Refresh();
                    Thread.Sleep(50);
                }
            }
        }
        #endregion game timer

        #region paint method
        private void LoseScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(gameOverTextWritten, Form1.dialogFontLarge, whiteBrush, textRec);
        }
        #endregion paint method
    }
}
