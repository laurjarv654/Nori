using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace UndertaleBattleSystemPrototype
{
    public partial class Pause : Form
    {
        #region variables

        //pause screen exiting stuff...?
        Boolean escapeDown = false;
        private static Pause pauseForm;
        private static DialogResult buttonResult = new DialogResult();

        //booleans for key presses
        Boolean wDown, sDown, spaceDown;

        //rectangles for menu buttons and player
        Rectangle resumeRec, saveRec, exitRec, playerRec;

        //images for sprites
        Image playerSprite;

        //brush for menu buttons
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        #endregion variables

        public Pause()
        {
            InitializeComponent();

            //setup sprites
            playerSprite = Properties.Resources.heart;

            //set menu button sizes and positions
            resumeRec = new Rectangle(this.Width / 6, this.Height / 4, 100, 25);
            saveRec = new Rectangle(this.Width / 6, resumeRec.Y + 50, 100, 25);
            exitRec = new Rectangle(this.Width / 6, saveRec.Y + 50, 100, 25);

            //set player location
            playerRec = new Rectangle(resumeRec.X + 5, resumeRec.Y + 5, 20, 20);
        }

        public static DialogResult Show()
        {
            pauseForm = new Pause();
            pauseForm.StartPosition = FormStartPosition.CenterParent;

            pauseForm.ShowDialog();
            return buttonResult;
        }

        #region key down and up
        private void Pause_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    escapeDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
            }
        }
        private void Pause_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    escapeDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
            }
        }
        #endregion key down and up

        #region pause timer
        private void pauseTimer_Tick(object sender, EventArgs e)
        {
            ButtonMenu();
            Refresh();
        }
        #endregion pause timer

        #region paint method
        private void Pause_Paint(object sender, PaintEventArgs e)
        {
            if (playerRec.IntersectsWith(resumeRec))
            {
                e.Graphics.DrawString("  Resume", Form1.dialogFont, whiteBrush, resumeRec);
                e.Graphics.DrawString("* Save", Form1.dialogFont, whiteBrush, saveRec);
                e.Graphics.DrawString("* Exit", Form1.dialogFont, whiteBrush, exitRec);
            }
            if (playerRec.IntersectsWith(saveRec))
            {
                e.Graphics.DrawString("* Resume", Form1.dialogFont, whiteBrush, resumeRec);
                e.Graphics.DrawString("  Save", Form1.dialogFont, whiteBrush, saveRec);
                e.Graphics.DrawString("* Exit", Form1.dialogFont, whiteBrush, exitRec);
            }
            if (playerRec.IntersectsWith(exitRec))
            {
                e.Graphics.DrawString("* Resume", Form1.dialogFont, whiteBrush, resumeRec);
                e.Graphics.DrawString("* Save", Form1.dialogFont, whiteBrush, saveRec);
                e.Graphics.DrawString("  Exit", Form1.dialogFont, whiteBrush, exitRec);
            }

            e.Graphics.DrawImage(playerSprite, playerRec);
        }
        #endregion paint method

        #region button menu method
        private void ButtonMenu()
        {
            #region play
            if (playerRec.IntersectsWith(resumeRec))
            {
                if (spaceDown == true)
                {
                    //TODO -- exit the pause form and resume the game
                }
                if (sDown == true)
                {
                    playerRec = new Rectangle(saveRec.X + 5, saveRec.Y + 5, 20, 20);
                    sDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion play

            #region controls
            if (playerRec.IntersectsWith(saveRec))
            {
                //go into the settings menu
                if (spaceDown == true)
                {
                   //TODO -- save player game info (is calum spared or not?)
                }
                if (wDown == true)
                {
                    playerRec = new Rectangle(resumeRec.X + 5, resumeRec.Y + 5, 20, 20);
                    wDown = false;

                    Thread.Sleep(150);
                }
                if (sDown == true)
                {
                    playerRec = new Rectangle(exitRec.X + 5, exitRec.Y + 5, 20, 20);
                    sDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion controls

            #region exit
            if (playerRec.IntersectsWith(exitRec))
            {
                //exit the game
                if (spaceDown == true)
                {
                    Application.Exit();
                }
                if (wDown == true)
                {
                    playerRec = new Rectangle(saveRec.X + 5, saveRec.Y + 5, 20, 20);
                    wDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion exit
        }
        #endregion button menu method
    }
}