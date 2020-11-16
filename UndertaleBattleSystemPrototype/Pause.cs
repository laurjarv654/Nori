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
using System.Xml;

namespace UndertaleBattleSystemPrototype
{
    public partial class Pause : Form
    {
        #region variables

        //pause screen exiting stuff...?
        private static Pause pauseForm;
        private static DialogResult buttonResult = new DialogResult();

        //booleans for key presses
        Boolean wDown, sDown, spaceDown;

        //rectangles for menu buttons and player
        Rectangle resumeRec, exitRec, playerRec;

        //rectangle and string for drawing player stats
        Rectangle statsRec;
        string stats;

        //images for sprites
        Image playerSprite, noriSprite;

        //brush for menu buttons and text
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        //xml reader for the player xml file
        XmlReader reader = XmlReader.Create("Resources/Player.xml");

        #endregion variables

        public Pause()
        {
            InitializeComponent();

            //setup sprites
            playerSprite = Properties.Resources.heart;
            noriSprite = Properties.Resources.noriFR;

            //set menu button sizes and positions
            resumeRec = new Rectangle(this.Width / 12, this.Height / 3, 150, 25);
            exitRec = new Rectangle(this.Width / 12, resumeRec.Y + 50, 150, 25);

            //set player location
            playerRec = new Rectangle(resumeRec.X + 5, resumeRec.Y + 5, 20, 20);

            //setup the stats string
            reader.ReadToFollowing("General");
            string gold = reader.GetAttribute("gold");
            reader.ReadToFollowing("Battle");
            string hp = reader.GetAttribute("currentHP");
            string atk = reader.GetAttribute("atk");

            stats = "Gold: " + gold + "\n\nHP: " + hp + "\n\nAtk: " + atk;

            //set the statsRec position
            statsRec = new Rectangle(this.Width / 3, this.Height / 3, this.Width / 4, this.Height / 2);
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
            //draw the stats string
            e.Graphics.DrawString(stats, Form1.dialogFontLarge, whiteBrush, statsRec);

            //draw an image of nori
            e.Graphics.DrawImage(noriSprite, this.Width - this.Width / 3, this.Height / 3, 200, 200);

            //draw the buttons according to which button the player is on
            if (playerRec.IntersectsWith(resumeRec))
            {
                e.Graphics.DrawString("  Resume", Form1.dialogFont, whiteBrush, resumeRec);
                e.Graphics.DrawString("* Exit", Form1.dialogFont, whiteBrush, exitRec);
            }
            if (playerRec.IntersectsWith(exitRec))
            {
                e.Graphics.DrawString("* Resume", Form1.dialogFont, whiteBrush, resumeRec);
                e.Graphics.DrawString("  Exit", Form1.dialogFont, whiteBrush, exitRec);
            }

            //draw the player heart sprite
            e.Graphics.DrawImage(playerSprite, playerRec);
        }
        #endregion paint method

        #region button menu method
        private void ButtonMenu()
        {
            #region resume
            if (playerRec.IntersectsWith(resumeRec))
            {
                if (spaceDown == true)
                {
                    //exit the pause form and resume the game
                    buttonResult = DialogResult.Cancel;
                    pauseForm.Close();
                }
                if (sDown == true)
                {
                    playerRec = new Rectangle(exitRec.X + 5, exitRec.Y + 5, 20, 20);
                    sDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion resume

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
                    playerRec = new Rectangle(resumeRec.X + 5, resumeRec.Y + 5, 20, 20);
                    wDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion exit
        }
        #endregion button menu method
    }
}