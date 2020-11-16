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
using UndertaleBattleSystemPrototype.Properties;

namespace UndertaleBattleSystemPrototype
{
    public partial class ControlsScreen : UserControl
    {
        #region variable declaractions

        //string for the game over text
        string controlsText = "Use WASD to move. " +
            "\nUse SPACE to interact with various things in-game. " +
            "\nUse SHIFT to exit menus. " +
            "\n\nYour name is Nori. You're just looking to pass through town, but on the horizon a gang is blocking your way. Talk to the townsfolk and try to resolve the conflict in the town. Have fun!" +
            "\n\n\nYou are now in the demo area. Feel free to get a feel for the battle system movement. When you are done, press SHIFT to go back to the main menu.";
        string controlsTextWritten = "";

        //booleans for key presses
        Boolean wDown, aDown, sDown, dDown, shiftDown;

        //rectangle for drawing the game over text into
        Rectangle textRec;

        //rectangles for the battle area demo
        Rectangle leftWall, rightWall, topWall, bottomWall;

        //brush for menu buttons
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        //image, player object, and rec for the player 
        Image playerSprite;
        Player player;

        //list for battle area walls
        List<Rectangle> battleWalls = new List<Rectangle>();

        #endregion variable declaractions

        public ControlsScreen()
        {
            InitializeComponent();
            OnStart();

            //set the player image correctly
            playerSprite = Resources.heart;
        }

        public void OnStart()
        {
            //initialize the text rec
            textRec = new Rectangle(this.Width / 8, titleSprite.Location.Y + 100, (this.Width / 2) + (this.Width / 6), this.Height / 2);

            //initialize the battle area walls
            leftWall = new Rectangle(this.Width / 2 - 125, textRec.Y + textRec.Height - 100, 5, 200);
            rightWall = new Rectangle(this.Width / 2 + 125, textRec.Y + textRec.Height - 100, 5, 200);
            topWall = new Rectangle(leftWall.X, leftWall.Y, rightWall.X - leftWall.X + 5, 5);
            bottomWall = new Rectangle(leftWall.X, leftWall.Y + leftWall.Height, rightWall.X - leftWall.X + 5, 5);

            //add the walla to the battleWalls list
            battleWalls.Add(leftWall);
            battleWalls.Add(rightWall);
            battleWalls.Add(topWall);
            battleWalls.Add(bottomWall);

            //initialize the player in the battle area
            player = new Player(leftWall.X + (rightWall.X - leftWall.X) / 2, topWall.Y + (bottomWall.Y - topWall.Y) / 2, 20, 0, 0);
        }

        #region key down and up
        private void ControlsScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player button presses
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    shiftDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
            }
        }

        private void ControlsScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player button releases
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    shiftDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
            }
        }
        #endregion key down and up

        #region game timer
        private void tutorialTimer_Tick(object sender, EventArgs e)
        {
            //if the player presses space, go back to the menu screen
            if (shiftDown == true)
            {
                //stop tutorial timer
                tutorialTimer.Enabled = false;

                //Goes to the menu screen
                MenuScreen ms = new MenuScreen();
                Form form = this.FindForm();

                form.Controls.Add(ms);
                form.Controls.Remove(this);

                ms.Location = new Point((form.Width - ms.Width) / 2, (form.Height - ms.Height) / 2);
            }
            //if the game over text hasn't been fully written out yet, continue to write it out
            if (controlsText != controlsTextWritten)
            {
                foreach (char c in controlsText)
                {
                    controlsTextWritten += c;
                    Refresh();
                    Thread.Sleep(10);
                }
            }

            #region player movement
            //player movement
            if (dDown == true && player.x < battleWalls[1].X - battleWalls[1].Width - player.size)
            {
                player.MoveLeftRight(8);
            }
            if (aDown == true && player.x > battleWalls[0].X + battleWalls[0].Width)
            {
                player.MoveLeftRight(-8);
            }
            if (wDown == true && player.y > battleWalls[2].Y + battleWalls[2].Height)
            {
                player.MoveUpDown(-8);
            }
            if (sDown == true && player.y < battleWalls[3].Y - battleWalls[3].Height - player.size)
            {
                player.MoveUpDown(8);
            }
            #endregion player movement
            Refresh();
        }
        #endregion game timer

        #region paint method
        private void ControlsScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(controlsTextWritten, Form1.dialogFont, whiteBrush, textRec);

            foreach (Rectangle r in battleWalls)
            {
                e.Graphics.FillRectangle(whiteBrush, r);
            }

            e.Graphics.DrawImage(playerSprite, player.x, player.y, player.size, player.size);
        }
        #endregion paint method
    }
}