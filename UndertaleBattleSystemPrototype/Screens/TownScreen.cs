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
using System.Xml;
using System.Media;


namespace UndertaleBattleSystemPrototype
{
    public partial class TownScreen : UserControl
    {
        #region global variables
        Boolean aDown, dDown, wDown, sDown, spaceDown, escapeDown;

        //music player
        SoundPlayer music = new SoundPlayer("Resources/Nori - The Winter City.wav");

        #region nori
        Player nori;
        Rectangle noriRec;
        const int HEROSPEED = 10;
        #endregion

        #region animation
        Image noriSprite;
        List<Image> noriAnimation = new List<Image>();
        int animationCounterH = 0, frameCounter = 1, spriteNumber = 0;
        #endregion

        #region objects
        List<Object> objects = new List<Object>(), roadList = new List<Object>();
        List<Rectangle> objectRecs = new List<Rectangle>();
        int buildingHeight;
        Boolean LeftSide = true;
        #endregion

        #region Text box
        Boolean displayText = false, displayTextBox = false;
        Rectangle textBoxSpriteRec, textBoxRec;
        Image textBoxSprite = Properties.Resources.textBoxSprite;
        SolidBrush textBrush = new SolidBrush(Color.White);
        List<string> textList = new List<string>();
        string text = "";
        int textNum = 0;
        XmlReader reader;
        #endregion

        //dialogue changes (counters)
        int talkingS = 0, talkingC = 0, talkingF = 0;


        #region fighting variables
        string cOutcome = "null", fOutcome = "null";
        public static string enemyName;
        #endregion

        #endregion


        public static Boolean timer = true;

        //TODO HP/attack/defense ints that pull from hero xml

        public TownScreen()
        {
            InitializeComponent();
            buildingHeight = (this.Height / 16) * 7;

            OnStart();

            //play the music on loop
            //music.PlayLooping();

            #region initializing nori animation images
            noriSprite = Properties.Resources.noriRR;
            noriAnimation.Add(Properties.Resources.noriBR);
            noriAnimation.Add(Properties.Resources.noriB1);
            noriAnimation.Add(Properties.Resources.noriB2);

            noriAnimation.Add(Properties.Resources.noriFR);
            noriAnimation.Add(Properties.Resources.noriF1);
            noriAnimation.Add(Properties.Resources.noriF2);

            noriAnimation.Add(Properties.Resources.noriRR);
            noriAnimation.Add(Properties.Resources.noriR1);
            noriAnimation.Add(Properties.Resources.noriR2);

            noriAnimation.Add(Properties.Resources.noriLR);
            noriAnimation.Add(Properties.Resources.noriL1);
            noriAnimation.Add(Properties.Resources.noriL2);
            #endregion

            #region initializing objects+object recs

            //library(0)
            objects.Add(new Object((this.Width / 4) * 7, 0, buildingHeight, (this.Width / 8) * 7, Properties.Resources.library));
            //arlos(1)
            objects.Add(new Object((this.Width / 4) * 11, 0, buildingHeight + 10, this.Width / 2, Properties.Resources.arlos));

            //appartements(2,3)
            objects.Add(new Object(0, 0, buildingHeight, (this.Width / 4) * 3, Properties.Resources.appartement1));
            objects.Add(new Object((this.Width / 4) * 3, 0, buildingHeight, (this.Width / 4) * 3, Properties.Resources.appartement2));

            //characters (4-6)
            objects.Add(new Object((this.Width / 8) * 15, buildingHeight - this.Height / 10 + 10, this.Height / 10, this.Width / 17, Properties.Resources.Sharol_WS));
            objects.Add(new Object((this.Width / 8) * 21, buildingHeight - this.Height / 6 + 10, this.Height / 6, this.Width / 16, Properties.Resources.Calum_WS));
            objects.Add(new Object((this.Width / 8) * 28, buildingHeight - this.Height / 6 + 10, this.Height / 6, this.Width / 12, Properties.Resources.Franky_WS));

            //bushes + trashcan
            objects.Add(new Object((this.Width / 8) * 12, buildingHeight - nori.size, nori.size, nori.size, Properties.Resources.bushGold));
            objects.Add(new Object((this.Width / 16) * 27, buildingHeight - (nori.size / 3) * 2, (nori.size / 3) * 2, this.Width / 20, Properties.Resources.trash));
            objects.Add(new Object((this.Width / 16) * 53, buildingHeight - nori.size, nori.size, nori.size, Properties.Resources.bushBook));
            objects.Add(new Object((this.Width / 8) * 30, buildingHeight - nori.size, nori.size, nori.size, Properties.Resources.bush));

            //just putting osmething in here so that the list isn't empty
            objectRecs.Add(new Rectangle(0, 0, 0, 0));

            #endregion

            textBoxSpriteRec = new Rectangle(this.Width / 8, (this.Height / 3) * 2, (this.Width / 4) * 3, this.Height / 4);
            textBoxRec = new Rectangle(textBoxSpriteRec.X + 40, textBoxSpriteRec.Y + 25, textBoxSpriteRec.Width - 40, textBoxSpriteRec.Height - 25);

        }

        //enables gametimer again when it becomes the main usercontrol (for returning from other screens)
        private void TownScreen_Enter(object sender, EventArgs e)
        {
            gameTimer.Enabled = true;
            Thread.Sleep(400);

            #region initializing object recs
            objectRecs.Clear();
            //library (0)
            objectRecs.Add(new Rectangle(objects[0].x + (this.Width / 32) * 9, objects[0].y + buildingHeight, 140, 20));

            //arlos(1)
            objectRecs.Add(new Rectangle(objects[1].x + (this.Width / 32) * 9, objects[1].y, 110, 20));

            //appartments(2-5)
            objectRecs.Add(new Rectangle(objects[2].x + (this.Width / 32) * 2, objects[2].y + buildingHeight, 100, 20));
            objectRecs.Add(new Rectangle(objects[2].x + (this.Width / 8) * 5, objects[2].y + buildingHeight, 100, 20));
            objectRecs.Add(new Rectangle(objects[2].x + (this.Width / 16) * 13, objects[2].y + buildingHeight, 100, 20));
            objectRecs.Add(new Rectangle(objects[2].x + (this.Width / 16) * 22, objects[2].y + buildingHeight, 100, 20));

            //characters (6-8)
            objectRecs.Add(new Rectangle(objects[4].x, objects[4].y, objects[4].width, objects[4].height));
            objectRecs.Add(new Rectangle(objects[5].x, objects[5].y, objects[5].width, objects[5].height));
            objectRecs.Add(new Rectangle(objects[6].x, objects[6].y, objects[6].width, objects[6].height));

            //special bushes(9,10)
            objectRecs.Add(new Rectangle((this.Width / 8) * 12, buildingHeight - nori.size, nori.size, nori.size));
            objectRecs.Add(new Rectangle((this.Width / 16) * 53, buildingHeight - nori.size, nori.size, nori.size));
            #endregion

            #region coming from battlescreen

            #region Franky dialogue

            //if you spare Franky
            if (noriRec.IntersectsWith(objectRecs[8]) && fOutcome == "spared")
            {
                switch (talkingF)
                {
                    case 3:
                        textNum = 15;
                        displayText = true;
                        spaceDown = false;
                        talkingF++;
                        break;
                    case 4:
                        textNum = 16;
                        displayText = true;
                        spaceDown = false;
                        talkingF++;
                        break;

                }

                Thread.Sleep(200);
                objects[6] = new Object(objects[5].x, objects[5].y - 10, this.Height / 6, this.Width / 12, Properties.Resources.Franky_WS);
            }

            //if you kill Franky
            if (noriRec.IntersectsWith(objectRecs[8]) && fOutcome == "killed")
            {
                textNum = 17;
                displayText = true;
                spaceDown = false;

                Thread.Sleep(200);
                objects[6] = new Object(0, 0, 0, 0, Properties.Resources.Franky_WS);

            }
            #endregion

            #region Callum
            //coming out of battlescreen and if you spared callum
            if (noriRec.IntersectsWith(objectRecs[7]) && cOutcome == "spared")
            {
                textNum = 7;
                displayText = true;
                spaceDown = false;

            }

            //coming out of battlescreen and if you killed callum
            if (noriRec.IntersectsWith(objectRecs[7]) && cOutcome == "killed")
            {
                textNum = 8;
                displayText = true;
                spaceDown = false;

                Thread.Sleep(200);
                objects[5] = new Object(0, 0, 0, 0, Properties.Resources.Calum_WS);
            }
            #endregion
            #endregion
        }

        public void OnStart()
        {
            nori = new Player(this.Width / 2 - 100, this.Height / 2 - 100, (buildingHeight / 8) * 3, 0, 0);

            //filling the text list with all of the dialogue that happens on this screen
            reader = XmlReader.Create("Resources/text.xml");
            while (reader.Read())
            {
                reader.ReadToFollowing("text");

                textList.Add(reader.GetAttribute("value"));
            }

            #region initializing roads
            roadList.Add(new Object(-10, 0, this.Height, this.Width, Properties.Resources.road2));
            roadList.Add(new Object(this.Width - 20, 0, this.Height, this.Width, Properties.Resources.road2));
            roadList.Add(new Object((this.Width * 2) - 30, 0, this.Height, this.Width, Properties.Resources.road2));
            roadList.Add(new Object((this.Width * 3) - 40, 0, this.Height, this.Width, Properties.Resources.roadTurn));
            #endregion

        }
        private void TownScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    break;
                case Keys.Escape:
                    escapeDown = true;
                    break;

            }
        }

        private void TownScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
                case Keys.Escape:
                    escapeDown = false;
                    break;
            }
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            #region set up xml reader
            reader = XmlReader.Create("Resources/Player.xml");

            reader.ReadToFollowing("Save");

            cOutcome = reader.GetAttribute("calum");
            fOutcome = reader.GetAttribute("franky");

            reader.Close();
            #endregion

            #region updating object recs
            objectRecs.Clear();
            //library
            objectRecs.Add(new Rectangle(objects[0].x + (this.Width / 32) * 9, objects[0].y + buildingHeight, 140, 20));
            //arlos
            objectRecs.Add(new Rectangle(objects[1].x + 85, objects[1].y + buildingHeight, 110, 20));

            //appartments(2-5)
            objectRecs.Add(new Rectangle(objects[2].x + (this.Width / 32) * 2, objects[2].y + buildingHeight, 100, 20));
            objectRecs.Add(new Rectangle(objects[2].x + (this.Width / 8) * 5, objects[2].y + buildingHeight, 100, 20));
            objectRecs.Add(new Rectangle(objects[2].x + (this.Width / 16) * 13, objects[2].y + buildingHeight, 100, 20));
            objectRecs.Add(new Rectangle(objects[2].x + (this.Width / 16) * 22, objects[2].y + buildingHeight, 100, 20));

            //characters (6-8)
            objectRecs.Add(new Rectangle(objects[4].x, objects[4].y, objects[4].width, objects[4].height));
            objectRecs.Add(new Rectangle(objects[5].x, objects[5].y, objects[5].width, objects[5].height));
            objectRecs.Add(new Rectangle(objects[6].x, objects[6].y, objects[6].width, objects[6].height));

            //special bushes(9,10)
            objectRecs.Add(new Rectangle((this.Width / 8) * 12, buildingHeight - nori.size, nori.size, nori.size));
            objectRecs.Add(new Rectangle((this.Width / 16) * 53, buildingHeight - nori.size, nori.size, nori.size));
            #endregion

            #region update nori/object movement

            //setting the rectangles to the updated x,y
            noriRec = new Rectangle(nori.x + (nori.size / 4), nori.y + (nori.size / 8) * 7, (nori.size / 2), nori.size / 8);

            if (wDown == true && sDown == false && nori.y >= buildingHeight - (this.Height / 64) * 9)
            {
                nori.MoveUpDown(-HEROSPEED);
                animationCounterH++;
                spriteNumber = 0;
                NoriAnimation();
            }
            if (sDown == true && wDown == false && nori.y <= this.Height - (nori.size / 8) * 11)
            {
                nori.MoveUpDown(HEROSPEED);
                animationCounterH++;
                spriteNumber = 3;
                NoriAnimation();
            }
            if (dDown == true && aDown == false)
            {
                #region stops you if you're at the end of the map
                if (nori.x >= this.Width / 2 - 100 && LeftSide == true)
                {
                    ObjectMovement(-HEROSPEED);
                }
                else if (nori.x < this.Width / 2 - 100 && LeftSide == true)
                {
                    nori.MoveLeftRight(HEROSPEED);
                }

                if (roadList[3].x + roadList[3].width >= this.Width && LeftSide == false)
                {
                    ObjectMovement(-HEROSPEED);
                }
                else if (nori.x <= this.Width - nori.size && LeftSide == false)
                {
                    nori.MoveLeftRight(HEROSPEED);
                }
                #endregion
                animationCounterH++;
                spriteNumber = 6;
                NoriAnimation();

            }
            if (aDown == true && dDown == false)
            {
                #region stops you if you get to the end of the map
                if (roadList[0].x <= -10 && LeftSide == true)
                {
                    ObjectMovement(HEROSPEED);
                }
                else if (nori.x >= -30 && LeftSide == true)
                {
                    nori.MoveLeftRight(-HEROSPEED);
                }

                if (nori.x <= this.Width / 2 - 100 && LeftSide == false)
                {
                    ObjectMovement(HEROSPEED);
                }
                else if (nori.x > this.Width / 2 - 100 && LeftSide == false)
                {
                    nori.MoveLeftRight(-HEROSPEED);
                }
                #endregion
                animationCounterH++;
                spriteNumber = 9;
                NoriAnimation();

            }
            #endregion

            #region tells if you're on the left or right side of the map
            if (objects[0].x + objects[0].width > this.Width / 2)
            {
                LeftSide = true;
            }
            else
            {
                LeftSide = false;
            }
            #endregion

            #region collision checks

            #region screen changing collisions
            //library door
            if (noriRec.IntersectsWith(objectRecs[0]) && spaceDown == true)
            {
                spaceDown = false;
                gameTimer.Enabled = false;
                LibraryScreen ls = new LibraryScreen();
                Form form = this.FindForm();
                form.Controls.Add(ls);

                ls.Location = new Point((this.Width - ls.Width) / 2, (this.Height - ls.Height) / 2);

                ls.Focus();
                ls.BringToFront();
            }

            //arlo's door
            if (noriRec.IntersectsWith(objectRecs[1]) && spaceDown == true)
            {
                spaceDown = false;
                gameTimer.Enabled = false;
                ShopScreen ss = new ShopScreen();
                Form form = this.FindForm();
                form.Controls.Add(ss);

                ss.Location = new Point((this.Width - ss.Width) / 2, (this.Height - ss.Height) / 2);

                ss.Focus();
                ss.BringToFront();
            }

            //Calum
            if (noriRec.IntersectsWith(objectRecs[7]) && spaceDown == true && cOutcome == "blank")
            {
                enemyName = "Calum";
                gameTimer.Enabled = false;
                BattleScreen bs = new BattleScreen();
                Form form = this.FindForm();
                form.Controls.Add(bs);

                bs.Location = new Point((this.Width - bs.Width) / 2, (this.Height - bs.Height) / 2);
                bs.Focus();
                bs.BringToFront();

            }

            //Franky
            if (noriRec.IntersectsWith(objectRecs[8]) && spaceDown == true && cOutcome == "spared")
            {
                enemyName = "Franky";
                gameTimer.Enabled = false;
                BattleScreen bs = new BattleScreen();
                Form form = this.FindForm();
                form.Controls.Add(bs);

                bs.Location = new Point((this.Width - bs.Width) / 2, (this.Height - bs.Height) / 2);
                bs.Focus();
                bs.BringToFront();

            }
            #endregion

            #region text displaying collisions
            if (displayText == false)
            {
                #region appartement doors
                for (int i = 2; i < 6; i++)
                {
                    if (noriRec.IntersectsWith(objectRecs[i]) && spaceDown == true)
                    {
                        textNum = i - 2;
                        displayText = true;
                        Thread.Sleep(200);
                    }
                }
                #endregion

                #region Sharol
                if (noriRec.IntersectsWith(objectRecs[6]) && spaceDown == true)
                {
                    switch (talkingS)
                    {
                        case 0:
                            textNum = 4;
                            displayText = true;
                            spaceDown = false;
                            talkingS++;
                            break;
                        case 1:
                            textNum = 5;
                            displayText = true;
                            spaceDown = false;
                            talkingS++;
                            break;
                        case 2:
                            textNum = 6;
                            displayText = true;
                            spaceDown = false;
                            break;
                    }
                }
                #endregion

                #region Franky

                if (noriRec.IntersectsWith(objectRecs[8]) && spaceDown == true)
                {
                    //if you haven't fought with callum yet
                    if (cOutcome == "blank")
                    {
                        switch (talkingF)
                        {
                            case 0:
                                textNum = 10;
                                displayText = true;
                                spaceDown = false;
                                talkingF++;
                                break;

                            case 1:
                                textNum = 11;
                                displayText = true;
                                spaceDown = false;
                                break;
                        }
                    }

                    //if you killed callum
                    if (cOutcome == "killed")
                    {
                        switch (talkingF)
                        {
                            case 1:
                                textNum = 12;
                                displayText = true;
                                spaceDown = false;
                                talkingF++;
                                break;
                            case 2:
                                textNum = 13;
                                displayText = true;
                                spaceDown = false;
                                talkingF++;
                                break;
                            case 3:
                                textNum = 14;
                                displayText = true;
                                spaceDown = false;
                                break;

                        }

                    }

                }

                #endregion

                #region bushes
                if (noriRec.IntersectsWith(objectRecs[9])&&spaceDown == true)
                {
                    textNum = 31;
                    displayText = true;
                    Thread.Sleep(200);
                }

                if (noriRec.IntersectsWith(objectRecs[10]) && spaceDown == true)
                {
                    textNum = 32;
                    displayText = true;
                    Thread.Sleep(200);
                }

                #endregion
            }
            else
            {
                DisplayTextCollisions();

            }


            #endregion

            #endregion


            if (escapeDown == true)
            {
                Pausing();
            }

            Refresh();
        }

        private void TownScreen_Paint(object sender, PaintEventArgs e)
        {
            //drawing road images
            foreach (Object o in roadList)
            {
                e.Graphics.DrawImage(o.sprite, o.x, o.y, o.width, o.height);
            }

            //drawing all objects on the screen
            foreach (Object o in objects)
            {
                e.Graphics.DrawImage(o.sprite, o.x, o.y, o.width, o.height);
            }

            // for testing rectangle collisions
            Pen test = new Pen(Color.Red);
            foreach (Rectangle r in objectRecs)
            {
                e.Graphics.DrawRectangle(test, r);
            }

            //drawing nori
            e.Graphics.DrawImage(noriSprite, nori.x, nori.y, nori.size, nori.size);

            e.Graphics.DrawRectangle(test, noriRec);

            if (displayTextBox == true)
            {
                e.Graphics.DrawImage(textBoxSprite, textBoxSpriteRec);
                e.Graphics.DrawString(text, Form1.dialogFont, textBrush, textBoxRec);
            }
        }
        private void DisplayTextCollisions()
        {

            text = textList[textNum];

            displayTextBox = true;
            if (spaceDown == true)
            {
                displayTextBox = false;
                displayText = false;
                Thread.Sleep(200);
            }

        }

        private void Pausing()
        {
            if (gameTimer.Enabled == true)
            {
                wDown = aDown = sDown = dDown = escapeDown = false;
                gameTimer.Enabled = false;
                Pause pauseForm = new Pause();

                DialogResult dr = Pause.Show();

                if (dr == DialogResult.Cancel)
                {
                    gameTimer.Enabled = true;
                }
                else if (dr == DialogResult.Abort)
                {
                    Form form = this.FindForm();
                    MenuScreen ms = new MenuScreen();

                    ms.Location = new Point((form.Width - ms.Width) / 2, (form.Height - ms.Height) / 2);

                    form.Controls.Add(ms);
                    form.Controls.Remove(this);
                }
            }
        }

        private void NoriAnimation()
        {
            if (animationCounterH == 4)
            {

                if (frameCounter == 1)
                {
                    noriSprite = noriAnimation[0 + spriteNumber];
                    animationCounterH = 0;
                    frameCounter = 2;
                }
                else if (frameCounter == 2)
                {
                    noriSprite = noriAnimation[1 + spriteNumber];
                    animationCounterH = 0;
                    frameCounter = 3;
                }
                else if (frameCounter == 3)
                {
                    noriSprite = noriAnimation[0 + spriteNumber];
                    animationCounterH = 0;
                    frameCounter = 4;
                }
                else if (frameCounter == 4)
                {
                    noriSprite = noriAnimation[2 + spriteNumber];
                    animationCounterH = 0;
                    frameCounter = 1;
                }

            }
        }
        private void ObjectMovement(int speed)
        {
            foreach (Object o in objects)
            {
                o.MoveLeftRight(speed);
            }

            foreach (Object r in roadList)
            {
                r.MoveLeftRight(speed);
            }
        }
    }
}

