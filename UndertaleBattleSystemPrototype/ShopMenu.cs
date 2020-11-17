using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.Windows.Forms;

namespace UndertaleBattleSystemPrototype
{
    public partial class ShopMenu : Form
    {
        #region variables

        //pause screen exiting stuff
        private static Pause pauseForm;
        private static DialogResult buttonResult = new DialogResult();

        //booleans for key presses
        Boolean wDown, sDown, spaceDown, shiftDown;

        //string for description drawing
        string description;

        //rectangles for description text, player, and shop buttons
        Rectangle descriptionRec, playerRec, buy1Rec, buy2Rec, buy3Rec;

        //image for player sprite
        Image playerSprite;

        //brush for menu buttons and text
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        //xml reader for the player xml file
        XmlReader reader = XmlReader.Create("Resources/Player.xml");

        #endregion variables

        public ShopMenu()
        {
            InitializeComponent();


        }

        #region key down and up
        private void ShopMenu_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player button presses
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    break;
                case Keys.ShiftKey:
                    shiftDown = true;
                    break;
            }
        }
        private void ShopMenu_KeyUp(object sender, KeyEventArgs e)
        {
            //player button releases
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
                case Keys.ShiftKey:
                    shiftDown = false;
                    break;
            }
        }
        #endregion key down and up

        #region shop timer
        private void shopTimer_Tick(object sender, EventArgs e)
        {
            if (shiftDown == true)
            {
                //exit the shop form and resume the game
                buttonResult = DialogResult.Cancel;
                pauseForm.Close();
            }

            ButtonMenu();
            Refresh();
        }
        #endregion shop timer

        #region paint method
        private void ShopMenu_Paint(object sender, PaintEventArgs e)
        {
            //draw the description string
            e.Graphics.DrawString(description, Form1.dialogFont, whiteBrush, descriptionRec);

            //draw the buttons according to which button the player is on
            if (playerRec.IntersectsWith(buy1Rec))
            {
                e.Graphics.DrawString("  Caesar Salad", Form1.dialogFont, whiteBrush, buy1Rec);
                e.Graphics.DrawString("* Protein Bar", Form1.dialogFont, whiteBrush, buy2Rec);
                e.Graphics.DrawString("* Hot Chocolate", Form1.dialogFont, whiteBrush, buy3Rec);
            }
            if (playerRec.IntersectsWith(buy2Rec))
            {
                e.Graphics.DrawString("* Caesar Salad", Form1.dialogFont, whiteBrush, buy1Rec);
                e.Graphics.DrawString("  Protein Bar", Form1.dialogFont, whiteBrush, buy2Rec);
                e.Graphics.DrawString("* Hot Chocolate", Form1.dialogFont, whiteBrush, buy3Rec);
            }
            if (playerRec.IntersectsWith(buy3Rec))
            {
                e.Graphics.DrawString("* Caesar Salad", Form1.dialogFont, whiteBrush, buy1Rec);
                e.Graphics.DrawString("*  Protein Bar", Form1.dialogFont, whiteBrush, buy2Rec);
                e.Graphics.DrawString(" Hot Chocolate", Form1.dialogFont, whiteBrush, buy3Rec);
            }

            //draw the player heart sprite
            e.Graphics.DrawImage(playerSprite, playerRec);
        }
        #endregion paint method

        #region button menu method
        private void ButtonMenu()
        {
            #region buy1 button
            if (playerRec.IntersectsWith(buy1Rec))
            {
                if (spaceDown == true)
                {
                    //TODO -- show description and y/n to buying
                }
                if (sDown == true)
                {
                    playerRec = new Rectangle(buy2Rec.X + 5, buy2Rec.Y + 5, 20, 20);
                    sDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion buy1 button

            #region buy2 button
            if (playerRec.IntersectsWith(buy2Rec))
            {
                //exit the game
                if (spaceDown == true)
                {
                    //TODO -- show description and y/n to buying
                }
                if (wDown == true)
                {
                    playerRec = new Rectangle(buy1Rec.X + 5, buy1Rec.Y + 5, 20, 20);
                    wDown = false;

                    Thread.Sleep(150);
                }
                if (sDown == true)
                {
                    playerRec = new Rectangle(buy3Rec.X + 5, buy3Rec.Y + 5, 20, 20);
                    sDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion buy2 button

            #region buy3 button
            if (playerRec.IntersectsWith(buy3Rec))
            {
                //exit the game
                if (spaceDown == true)
                {
                    //TODO -- show description and y/n to buying
                }
                if (wDown == true)
                {
                    playerRec = new Rectangle(buy2Rec.X + 5, buy2Rec.Y + 5, 20, 20);
                    wDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion buy3 button
        }
        #endregion button menu method
    }
}
