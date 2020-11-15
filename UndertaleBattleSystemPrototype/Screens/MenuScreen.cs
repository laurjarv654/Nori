using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using UndertaleBattleSystemPrototype.Properties;

namespace UndertaleBattleSystemPrototype
{
    public partial class MenuScreen : UserControl
    {
        #region variable declaractions

        //booleans for key presses
        Boolean aDown, dDown, spaceDown;

        //rectangles for menu buttons and player
        Rectangle playRec, controlsRec, exitRec, playerRec;

        //images for sprites
        Image playerSprite;

        //brush for menu buttons
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        //sound player for the background music
        SoundPlayer music = new SoundPlayer("Resources/Nori - Nori.wav");

        #endregion variable declaractions

        public MenuScreen()
        {
            InitializeComponent();
            OnStart();

            //setup sprites
            playerSprite = Resources.heart;

            //start the music and have it loop
            //music.PlayLooping();
        }

        public void OnStart()
        {
            //set menu button sizes and positions
            playRec = new Rectangle(this.Width / 4 - 40, (this.Height / 2) + (this.Height / 4), 150, 40);
            controlsRec = new Rectangle(this.Width / 2 - 75, (this.Height / 2) + (this.Height / 4), 150, 40);
            exitRec = new Rectangle(this.Width - (this.Width / 4) - 40, (this.Height / 2) + (this.Height / 4), 150, 40);

            //set player location
            playerRec = new Rectangle(playRec.X + 5, playRec.Y + 5, 20, 20);
        }

        #region key down and up
        private void MenuScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player button presses
            switch (e.KeyCode)
            {
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    break;
            }
        }

        private void MenuScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player button releases
            switch (e.KeyCode)
            {
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
            }
        }
        #endregion key down and up

        #region menu timer
        private void menuTimer_Tick(object sender, EventArgs e)
        {
            ButtonMenu();
            Refresh();
        }
        #endregion menu timer

        #region paint method
        private void MenuScreen_Paint(object sender, PaintEventArgs e)
        {
            if (playerRec.IntersectsWith(playRec))
            {
                e.Graphics.DrawString("  Play", Form1.dialogFont, whiteBrush, playRec);
                e.Graphics.DrawString("* Controls", Form1.dialogFont, whiteBrush, controlsRec);
                e.Graphics.DrawString("* Exit", Form1.dialogFont, whiteBrush, exitRec);
            }
            if (playerRec.IntersectsWith(controlsRec))
            {
                e.Graphics.DrawString("* Play", Form1.dialogFont, whiteBrush, playRec);
                e.Graphics.DrawString("  Controls", Form1.dialogFont, whiteBrush, controlsRec);
                e.Graphics.DrawString("* Exit", Form1.dialogFont, whiteBrush, exitRec);
            }
            if (playerRec.IntersectsWith(exitRec))
            {
                e.Graphics.DrawString("* Play", Form1.dialogFont, whiteBrush, playRec);
                e.Graphics.DrawString("* Controls", Form1.dialogFont, whiteBrush, controlsRec);
                e.Graphics.DrawString("  Exit", Form1.dialogFont, whiteBrush, exitRec);
            }

            e.Graphics.DrawImage(playerSprite, playerRec);
        }
        #endregion paint method

        #region button menu method
        private void ButtonMenu()
        {
            #region play
            if (playerRec.IntersectsWith(playRec))
            {
                if (spaceDown == true)
                {
                    //stop menu timer
                    menuTimer.Enabled = false;

                    //stop the music
                    music.Stop();

                    // Goes to the town screen
                    TownScreen ts = new TownScreen();
                    Form form = this.FindForm();

                    form.Controls.Add(ts);
                    form.Controls.Remove(this);

                    ts.Location = new Point((form.Width - ts.Width) / 2, (form.Height - ts.Height) / 2);
                }
                if (dDown == true)
                {
                    playerRec = new Rectangle(controlsRec.X + 5, controlsRec.Y + 5, 20, 20);
                    dDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion play

            #region controls
            if (playerRec.IntersectsWith(controlsRec))
            {
                //go into the settings menu
                if (spaceDown == true)
                {
                    //stop menu timer
                    menuTimer.Enabled = false;

                    // Goes to the controls screen
                    ControlsScreen cs = new ControlsScreen();
                    Form form = this.FindForm();

                    form.Controls.Add(cs);
                    form.Controls.Remove(this);

                    cs.Location = new Point((form.Width - cs.Width) / 2, (form.Height - cs.Height) / 2);
                }
                if (aDown == true)
                {
                    playerRec = new Rectangle(playRec.X + 5, playRec.Y + 5, 20, 20);
                    aDown = false;

                    Thread.Sleep(150);
                }
                if (dDown == true)
                {
                    playerRec = new Rectangle(exitRec.X + 5, exitRec.Y + 5, 20, 20);
                    dDown = false;

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
                if (aDown == true)
                {
                    playerRec = new Rectangle(controlsRec.X + 5, controlsRec.Y + 5, 20, 20);
                    aDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion exit
        }
        #endregion button menu method
    }
}
