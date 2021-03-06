﻿using System;
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
using System.Media;

namespace UndertaleBattleSystemPrototype
{
    public partial class ShopMenu : Form
    {
        #region variables

        //pause screen exiting stuff
        private static ShopMenu shopForm;
        private static DialogResult buttonResult = new DialogResult();

        //booleans for key presses
        Boolean wDown, sDown, spaceDown, shiftDown;

        //string for description drawing
        string description;

        //int for player gold and item selected
        int gold, itemSelected;

        //rectangles for description text, player, and shop buttons
        Rectangle descriptionRec, playerRec, buy1Rec, buy2Rec, buy3Rec, confirmRec;

        //image for player sprite
        Image playerSprite;

        //brush for menu buttons and text
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        //xml reader for the player xml file
        XmlReader reader = XmlReader.Create("Resources/Player.xml");

        //lists for item stuff
        List<string> itemNames = new List<string>() { "Caesar Salad", "Protein Bar", "Hot Chocolate" };
        List<int> itemGoldAmounts = new List<int>() { 40, 20, 30 };
        List<int> itemHealAmounts = new List<int>() { 25, 10, 15 };

        //SFX
        SoundPlayer menuMoveSound = new SoundPlayer("Resources/Nori - MenuMove.wav");
        SoundPlayer menuSelectSound = new SoundPlayer("Resources/Nori - MenuSelect.wav");
        SoundPlayer buySound = new SoundPlayer("Resources/Nori - BuySound.wav");

        #endregion variables

        public ShopMenu()
        {
            InitializeComponent();

            //setup sprites
            playerSprite = Properties.Resources.heart;

            //initialize the description rec
            descriptionRec = new Rectangle(this.Width / 12, this.Height / 3, this.Width / 2, this.Height);

            //initialize the player rec and button recs
            buy1Rec = new Rectangle((this.Width / 2) + (this.Width / 5), this.Height / 3, 200, 20);
            buy2Rec = new Rectangle((this.Width / 2) + (this.Width / 5), buy1Rec.Y + 60, 200, 20);
            buy3Rec = new Rectangle((this.Width / 2) + (this.Width / 5), buy2Rec.Y + 60, 200, 20);
            confirmRec = new Rectangle((this.Width / 2) + (this.Width / 4), buy3Rec.Y + 60, 200, 40);
            playerRec = new Rectangle(buy1Rec.X + 5, buy1Rec.Y + 5, 20, 20);

            //get the player's gold amount
            reader.ReadToFollowing("General");
            gold = Convert.ToInt16(reader.GetAttribute("gold"));
            reader.Close();
        }

        public static DialogResult Show()
        {
            shopForm = new ShopMenu();
            shopForm.StartPosition = FormStartPosition.CenterParent;

            shopForm.ShowDialog();
            return buttonResult;
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
            if (playerRec.IntersectsWith(confirmRec) == false)
            {
                if (shiftDown == true)
                {
                    //exit the shop form and resume the game
                    buttonResult = DialogResult.Cancel;
                    shopForm.Close();
                }

                description = "Arlo's shop. There are a few things that pique your interest. \n\nUse SPACE to select and SHIFT to exit.";
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
                e.Graphics.DrawString("* Protein Bar", Form1.dialogFont, whiteBrush, buy2Rec);
                e.Graphics.DrawString("  Hot Chocolate", Form1.dialogFont, whiteBrush, buy3Rec);
            }

            //draw the confirm buy button if it is timer
            if (playerRec.IntersectsWith(confirmRec))
            {
                e.Graphics.DrawString("  Buy?", Form1.dialogFont, whiteBrush, confirmRec);
            }

            //draw the player heart sprite
            e.Graphics.DrawImage(playerSprite, playerRec);
        }
        #endregion paint method

        #region button menu method
        private void ButtonMenu()
        {
            #region confirm button
            if (playerRec.IntersectsWith(confirmRec))
            {
                if (spaceDown == true)
                {
                    //play buy sound
                    buySound.Play();

                    PlayerItemUpdate();
                }
                if (shiftDown == true)
                {
                    playerRec = new Rectangle(buy1Rec.X + 5, buy1Rec.Y + 5, 20, 20);
                    Thread.Sleep(150);
                }
            }
            #endregion confirm button

            #region buy1 button
            if (playerRec.IntersectsWith(buy1Rec))
            {
                if (spaceDown == true)
                {
                    //play menu select sound
                    menuSelectSound.Play();

                    playerRec = new Rectangle(confirmRec.X + 5, confirmRec.Y + 5, 20, 20);
                    description = "A delicious salad and a personal favorite of some guy named Christopher. A classic. \n\nThis item costs 40G and will heal you 25HP.";
                    itemSelected = 0;
                    Thread.Sleep(150);
                }
                if (sDown == true)
                {
                    //play menu move sound
                    menuMoveSound.Play();

                    playerRec = new Rectangle(buy2Rec.X + 5, buy2Rec.Y + 5, 20, 20);
                    sDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion buy1 button

            #region buy2 button
            if (playerRec.IntersectsWith(buy2Rec))
            {
                if (spaceDown == true)
                {
                    //play menu select sound
                    menuSelectSound.Play();

                    playerRec = new Rectangle(confirmRec.X + 5, confirmRec.Y + 5, 20, 20);
                    description = "A protein bar. Not what you'd expect at a restaurant, but hey- who cares? \n\nThis item costs 20G and will heal you 10HP.";
                    itemSelected = 1;
                    Thread.Sleep(150);
                }
                if (wDown == true)
                {
                    //play menu move sound
                    menuMoveSound.Play();

                    playerRec = new Rectangle(buy1Rec.X + 5, buy1Rec.Y + 5, 20, 20);
                    wDown = false;

                    Thread.Sleep(150);
                }
                if (sDown == true)
                {
                    //play menu move sound
                    menuMoveSound.Play();

                    playerRec = new Rectangle(buy3Rec.X + 5, buy3Rec.Y + 5, 20, 20);
                    sDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion buy2 button

            #region buy3 button
            if (playerRec.IntersectsWith(buy3Rec))
            {
                if (spaceDown == true)
                {
                    //play menu select sound
                    menuSelectSound.Play();

                    playerRec = new Rectangle(confirmRec.X + 5, confirmRec.Y + 5, 20, 20);
                    description = "Hot chocolate. It's the best drink to warm up your life! ...or so Arlo's slogan says. \n\nThis item costs 30G and will heal you 15HP.";
                    itemSelected = 2;
                    Thread.Sleep(150);
                }
                if (wDown == true)
                {
                    //play menu move sound
                    menuMoveSound.Play();

                    playerRec = new Rectangle(buy2Rec.X + 5, buy2Rec.Y + 5, 20, 20);
                    wDown = false;

                    Thread.Sleep(150);
                }
            }
            #endregion buy3 button
        }
        #endregion button menu method

        #region player item update method
        public void PlayerItemUpdate()
        {
            //open the player xml file and place it in doc
            XmlDocument doc = new XmlDocument();
            doc.Load("Resources/Player.xml");

            //create a list of all nodes called "Item" and one for "General"
            XmlNodeList itemList = doc.GetElementsByTagName("Item");
            XmlNodeList playerGold = doc.GetElementsByTagName("General");

            //search each Item node in the list until there is an empty node
            //then change it to the correct item if the player has enough gold
            foreach (XmlNode n in itemList)
            {
                if (n.Attributes[0].InnerText == " ")
                {
                    if (gold - itemGoldAmounts[itemSelected] > 0)
                    {
                        n.Attributes[0].InnerText = itemNames[itemSelected];
                        n.Attributes[1].InnerText = Convert.ToString(itemHealAmounts[itemSelected]);
                        gold -= itemGoldAmounts[itemSelected];
                        playerGold[0].Attributes[0].InnerText = Convert.ToInt16(playerGold[0].Attributes[0].InnerText) - itemGoldAmounts[itemSelected] + "";
                        
                        description = "Item Bought.";
                        Refresh();
                        Thread.Sleep(1000);
                        playerRec = new Rectangle(buy1Rec.X + 5, buy1Rec.Y + 5, 20, 20);
                        break;
                    }
                    //if the player doesn't have enough gold, tell them and go back to the first buying option
                    else
                    {
                        description = "You can't afford that!";
                        Refresh();
                        Thread.Sleep(1000);
                        playerRec = new Rectangle(buy1Rec.X + 5, buy1Rec.Y + 5, 20, 20);
                        break;
                    }
                }
            }

            //save and close the player xml
            doc.Save("Resources/Player.xml");
        }
        #endregion player item update method
    }
}
