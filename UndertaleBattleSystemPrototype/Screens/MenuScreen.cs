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
using System.Xml;
using UndertaleBattleSystemPrototype.Properties;

namespace UndertaleBattleSystemPrototype
{
    public partial class MenuScreen : UserControl
    {
        #region variable declaractions

        //booleans for key presses
        Boolean aDown, dDown, spaceDown;

        //rectangles for menu buttons and player
        Rectangle newGameRec, resumeRec, controlsRec, exitRec, playerRec;

        //images for sprites
        Image playerSprite;

        //brush for menu buttons
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        //sound player for the background music
        SoundPlayer music = new SoundPlayer("Resources/Nori - Nori.wav");

        //SFX
        SoundPlayer menuMoveSound = new SoundPlayer("Resources/Nori - MenuMove.wav");
        SoundPlayer menuSelectSound = new SoundPlayer("Resources/Nori - MenuSelect.wav");

        #endregion variable declaractions

        public MenuScreen()
        {
            InitializeComponent();
            OnStart();

            //setup sprites
            playerSprite = Resources.heart;

            //start the music and have it loop
            music.PlayLooping();
        }

        public void OnStart()
        {
            //set menu button sizes and positions
            newGameRec = new Rectangle(this.Width / 8 - 75, titleSprite.Location.Y + titleSprite.Height + 100, 150, 25);
            resumeRec = new Rectangle(this.Width / 8 * 3 - 75, newGameRec.Y, 150, 25);
            controlsRec = new Rectangle(this.Width / 8 * 5 - 75, newGameRec.Y, 150, 25);
            exitRec = new Rectangle(this.Width / 8 * 7 - 75, newGameRec.Y, 150, 25);

            //set player location
            playerRec = new Rectangle(newGameRec.X + 5, newGameRec.Y + 5, 20, 20);
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
            if (playerRec.IntersectsWith(newGameRec))
            {
                e.Graphics.DrawString("  New Game", Form1.dialogFont, whiteBrush, newGameRec);
                e.Graphics.DrawString("* Resume", Form1.dialogFont, whiteBrush, resumeRec);
                e.Graphics.DrawString("* Controls", Form1.dialogFont, whiteBrush, controlsRec);
                e.Graphics.DrawString("* Exit", Form1.dialogFont, whiteBrush, exitRec);
            }
            if (playerRec.IntersectsWith(resumeRec))
            {
                e.Graphics.DrawString("* New Game", Form1.dialogFont, whiteBrush, newGameRec);
                e.Graphics.DrawString("  Resume", Form1.dialogFont, whiteBrush, resumeRec);
                e.Graphics.DrawString("* Controls", Form1.dialogFont, whiteBrush, controlsRec);
                e.Graphics.DrawString("* Exit", Form1.dialogFont, whiteBrush, exitRec);
            }
            if (playerRec.IntersectsWith(controlsRec))
            {
                e.Graphics.DrawString("* New Game", Form1.dialogFont, whiteBrush, newGameRec);
                e.Graphics.DrawString("* Resume", Form1.dialogFont, whiteBrush, resumeRec);
                e.Graphics.DrawString("  Controls", Form1.dialogFont, whiteBrush, controlsRec);
                e.Graphics.DrawString("* Exit", Form1.dialogFont, whiteBrush, exitRec);
            }
            if (playerRec.IntersectsWith(exitRec))
            {
                e.Graphics.DrawString("* New Game", Form1.dialogFont, whiteBrush, newGameRec);
                e.Graphics.DrawString("* Resume", Form1.dialogFont, whiteBrush, resumeRec);
                e.Graphics.DrawString("* Controls", Form1.dialogFont, whiteBrush, controlsRec);
                e.Graphics.DrawString("  Exit", Form1.dialogFont, whiteBrush, exitRec);
            }

            e.Graphics.DrawImage(playerSprite, playerRec);
        }
        #endregion paint method

        #region button menu method
        private void ButtonMenu()
        {
            #region new game
            if (playerRec.IntersectsWith(newGameRec))
            {
                if (spaceDown == true)
                {
                    //play menu select sound
                    menuSelectSound.Play();

                    //stop menu timer
                    menuTimer.Enabled = false;

                    //stop the music
                    music.Stop();

                    //reset the player xml
                    playerXmlReset();

                    // Goes to the town screen
                    TownScreen ts = new TownScreen();
                    Form form = this.FindForm();

                    form.Controls.Add(ts);
                    form.Controls.Remove(this);
                }
                if (dDown == true)
                {
                    //play menu move sound
                    menuMoveSound.Play();

                    playerRec = new Rectangle(resumeRec.X + 5, resumeRec.Y + 5, 20, 20);
                    dDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion new game

            #region resume
            if (playerRec.IntersectsWith(resumeRec))
            {
                if (spaceDown == true)
                {
                    //play menu select sound
                    menuSelectSound.Play();

                    //stop menu timer
                    menuTimer.Enabled = false;

                    //stop the music
                    music.Stop();

                    // Goes to the town screen
                    TownScreen ts = new TownScreen();
                    Form form = this.FindForm();

                    form.Controls.Add(ts);
                    form.Controls.Remove(this);
                }
                if (aDown == true)
                {
                    //play menu move sound
                    menuMoveSound.Play();

                    playerRec = new Rectangle(newGameRec.X + 5, newGameRec.Y + 5, 20, 20);
                    aDown = false;

                    Thread.Sleep(150);
                }
                if (dDown == true)
                {
                    //play menu move sound
                    menuMoveSound.Play();

                    playerRec = new Rectangle(controlsRec.X + 5, controlsRec.Y + 5, 20, 20);
                    dDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion resume

            #region controls
            if (playerRec.IntersectsWith(controlsRec))
            {
                //go into the settings menu
                if (spaceDown == true)
                {
                    //play menu select sound
                    menuSelectSound.Play();

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
                    //play menu move sound
                    menuMoveSound.Play();

                    playerRec = new Rectangle(resumeRec.X + 5, resumeRec.Y + 5, 20, 20);
                    aDown = false;

                    Thread.Sleep(150);
                }
                if (dDown == true)
                {
                    //play menu move sound
                    menuMoveSound.Play();

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
                    //play menu select sound
                    menuSelectSound.Play();

                    Application.Exit();
                }
                if (aDown == true)
                {
                    //play menu move sound
                    menuMoveSound.Play();

                    playerRec = new Rectangle(controlsRec.X + 5, controlsRec.Y + 5, 20, 20);
                    aDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion exit
        }
        #endregion button menu method

        #region player xml reset method
        public void playerXmlReset()
        {
            //open the player xml file and place it in doc
            XmlDocument doc = new XmlDocument();
            doc.Load("Resources/Player.xml");

            //create lists for all nodes in the player xml
            XmlNodeList gold = doc.GetElementsByTagName("General");
            XmlNodeList stats = doc.GetElementsByTagName("Battle");
            XmlNodeList itemList = doc.GetElementsByTagName("Item");
            XmlNodeList saveInfo = doc.GetElementsByTagName("Save");

            //reset gold
            gold[0].Attributes[0].InnerText = "50";

            //reset hp and atk
            stats[0].Attributes[0].InnerText = "40";
            stats[0].Attributes[1].InnerText = "5";

            //reset items
            foreach (XmlNode n in itemList)
            {
                n.Attributes[0].InnerText = " ";
                n.Attributes[1].InnerText = "0";
            }

            //reset save info
            saveInfo[0].Attributes[0].InnerText = "blank";
            saveInfo[0].Attributes[1].InnerText = "blank";

            doc.Save("Resources/Player.xml");
        }
        #endregion player xml reset method
    }
}
