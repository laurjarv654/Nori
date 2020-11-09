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

namespace UndertaleBattleSystemPrototype
{
    public partial class LibraryScreen : UserControl
    {
        Boolean aDown, dDown, wDown, sDown, spaceDown, escapeDown;

        #region nori
        Player nori;
        Rectangle noriRec;
        const int HEROSPEED = 10;
        string noriRow = "row1";
        #endregion

        #region animation
        Image noriSprite;
        List<Image> noriAnimation = new List<Image>(), roadAnimation = new List<Image>();
        int animationCounterH = 0, frameCounter = 1, spriteNumber = 0;
        string direction = "up";
        #endregion

        #region objects
        List<Object> objects = new List<Object>();
        List<Rectangle> objectRecs = new List<Rectangle>();
        List<Rectangle> border = new List<Rectangle>();
        const int BORDERWIDTH = 250;
        int bookshelfSize;
        #endregion

        #region Text box
        Boolean displayText = false, displayTextBox = false;
        Rectangle textBox;
        List<string> textList = new List<string>();
        string text = "";
        int talkingD = 0;

        int textNum = 0;
        XmlReader reader;
        #endregion

        public LibraryScreen()
        {
            InitializeComponent();
            OnStart();

            bookshelfSize = (this.Width - (BORDERWIDTH * 2)) / 4;

            #region initializing nori animation images
            noriSprite = Properties.Resources.noriBR;
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

            #region initializing objects
            //door
            objects.Add(new Object((this.Width / 2) - (bookshelfSize / 2), this.Height - 170, 50, bookshelfSize, Properties.Resources.libraryDoor));

            //bookshelves back row (1-4)
            objects.Add(new Object(BORDERWIDTH, 20, bookshelfSize, bookshelfSize, Properties.Resources.bookshelfB));
            objects.Add(new Object(BORDERWIDTH + bookshelfSize, 20, bookshelfSize, bookshelfSize, Properties.Resources.bookshelfR));
            objects.Add(new Object(this.Width - BORDERWIDTH - bookshelfSize, 20, bookshelfSize, bookshelfSize, Properties.Resources.bookshelfG));
            objects.Add(new Object(this.Width - BORDERWIDTH - (bookshelfSize * 2), 20, bookshelfSize, bookshelfSize, Properties.Resources.bookshelfY));

            //librarian (donna)
            objects.Add(new Object(BORDERWIDTH, this.Height - 120 - bookshelfSize, bookshelfSize - 30, bookshelfSize, Properties.Resources.Donna));

            //bookshelves row 2
            objects.Add(new Object(BORDERWIDTH, bookshelfSize, bookshelfSize, bookshelfSize, Properties.Resources.bookshelfY));
            objects.Add(new Object(this.Width - BORDERWIDTH - bookshelfSize, bookshelfSize, bookshelfSize, bookshelfSize, Properties.Resources.bookshelfR));

            //bookshelves row 1
            objects.Add(new Object(BORDERWIDTH, (bookshelfSize * 2) - 20, bookshelfSize, bookshelfSize, Properties.Resources.bookshelfG));
            objects.Add(new Object(this.Width - BORDERWIDTH - bookshelfSize, (bookshelfSize * 2) - 20, bookshelfSize, bookshelfSize, Properties.Resources.bookshelfB));


            #endregion

            #region object rectangles
            //backrow 
            objectRecs.Add(new Rectangle(BORDERWIDTH, bookshelfSize + 20, bookshelfSize, bookshelfSize - 20));
            objectRecs.Add(new Rectangle(this.Width - BORDERWIDTH - bookshelfSize, bookshelfSize + 20, bookshelfSize, bookshelfSize - 20));

            //row 2
            objectRecs.Add(new Rectangle(BORDERWIDTH, (bookshelfSize * 2), bookshelfSize, bookshelfSize - 20));
            objectRecs.Add(new Rectangle(this.Width - BORDERWIDTH - bookshelfSize, (bookshelfSize * 2), bookshelfSize, bookshelfSize - 20));

            //row 1
            objectRecs.Add(new Rectangle(BORDERWIDTH, (bookshelfSize * 3) - 20, bookshelfSize, 30));
            objectRecs.Add(new Rectangle(this.Width - BORDERWIDTH - bookshelfSize, (bookshelfSize * 3) - 20, bookshelfSize, 30));

            //mid back row
            objectRecs.Add(new Rectangle(BORDERWIDTH + bookshelfSize, bookshelfSize + 20, bookshelfSize, 30));
            objectRecs.Add(new Rectangle(this.Width - BORDERWIDTH - (bookshelfSize * 2), bookshelfSize + 20, bookshelfSize, 30));

            //donna
            objectRecs.Add(new Rectangle(objects[5].x, objects[5].y + 30, objects[5].width - 20, objects[5].height - 30));

            //door
            objectRecs.Add(new Rectangle(objects[0].x, objects[0].y + 10, objects[0].width - 20, objects[0].height - 30));
            #endregion

            #region initializing borders
            //top
            border.Add(new Rectangle(0, 0, this.Width, bookshelfSize + 20));

            //left
            border.Add(new Rectangle(0, 0, BORDERWIDTH, this.Height));

            //right
            border.Add(new Rectangle(this.Width - BORDERWIDTH, 0, BORDERWIDTH, this.Height));

            //bottom
            border.Add(new Rectangle(0, this.Height - 150, this.Width, 150));

            #endregion

        }
        public void OnStart()
        {
            nori = new Player(this.Width / 2 - 75, this.Height - 300, 150, 0, 0);

            //filling the text list with all of the dialogue that happens on this screen
            reader = XmlReader.Create("Resources/text.xml");
            while (reader.Read())
            {
                reader.ReadToFollowing("text");

                textList.Add(reader.GetAttribute("value"));
            }
        }
        private void LibraryScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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
        private void LibraryScreen_KeyUp(object sender, KeyEventArgs e)
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



            #region update nori/object movement
            for (int i = 0; i <= 3; i++)
            {
                if (noriRec.IntersectsWith(objectRecs[i]) == true)
                {
                    wDown = false;
                    sDown = false;
                }
            }
            for (int i = 4; i <= 7; i++)
            {
                if (noriRec.IntersectsWith(objectRecs[i]))
                {
                    wDown = false;
                }
            }

            //donna
            if (noriRec.IntersectsWith(objectRecs[8]))
            {
                if (direction == "down")
                {
                    sDown = false;
                }
                if (direction == "up")
                {
                    sDown = false;
                }

                if (direction == "left")
                {
                    aDown = false;
                }
                if (direction == "right")
                {
                    aDown = false;
                }
            }

            //setting the rectangles to the updated x,y
            noriRec = new Rectangle(nori.x + 40, nori.y + 120, 70, 30);

            if (wDown == true && nori.y >= bookshelfSize - 90)
            {
                nori.MoveUpDown(-HEROSPEED);
                direction = "up";
                animationCounterH++;
                spriteNumber = 0;
                NoriAnimation();
            }
            if (sDown == true && wDown == false && nori.y <= this.Height - 310)
            {
                nori.MoveUpDown(HEROSPEED);
                direction = "down";
                animationCounterH++;
                spriteNumber = 3;
                NoriAnimation();
            }
            if (dDown == true && aDown == false && nori.x <= this.Width - BORDERWIDTH - 100)
            {
                nori.MoveLeftRight(HEROSPEED);
                direction = "right";
                animationCounterH++;
                spriteNumber = 6;
                NoriAnimation();

            }
            if (aDown == true && dDown == false && nori.x >= BORDERWIDTH - 40)
            {
                nori.MoveLeftRight(-HEROSPEED);
                direction = "left";
                animationCounterH++;
                spriteNumber = 9;
                NoriAnimation();

            }


            #endregion

            #region colisions
            if (displayText == false)
            {
                //bookshelves
                for (int i = 0; i <= 7; i++)
                {
                    if (noriRec.IntersectsWith(objectRecs[i]) && spaceDown == true)
                    {
                        textNum = i + 15;
                        displayText = true;
                        Thread.Sleep(200);
                    }
                }

                //donna
                if (noriRec.IntersectsWith(objectRecs[8]) && spaceDown == true)
                {
                    switch (talkingD)
                    {
                        case 0:
                            textNum = 25;
                            displayText = true;
                            spaceDown = false;
                            talkingD++;
                            break;
                        case 1:
                            textNum = 26;
                            displayText = true;
                            spaceDown = false;
                            talkingD++;
                            break;
                        case 2:
                            textNum = 27;
                            displayText = true;
                            spaceDown = false;
                            break;
                    }
                }

            }
            else
            {
                DisplayTextCollisions();

            }



            //door
            if (noriRec.IntersectsWith(objectRecs[9]) && spaceDown == true)
            {
                TownScreen.timer = true;
                gameTimer.Enabled = false;
                Form form = this.FindForm();
                form.Controls.Remove(this);

            }

            

            #endregion

            //pausing
            if (escapeDown == true)
            {
                Pausing();
            }

            Refresh();
        }

        private void LibraryScreen_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush borderbrush = new SolidBrush(Color.Black);

            //drawing borders
            foreach (Rectangle r in border) { e.Graphics.FillRectangle(borderbrush, r); }

            #region drawing bookshelves vs nori
            //drawing back shelves
            for (int i = 0; i <= 4; i++) { e.Graphics.DrawImage(objects[i].sprite, objects[i].x, objects[i].y, objects[i].width, objects[i].height); }

            //drawing nori if behind both rows 1 + 2
            if (noriRec.Y <= bookshelfSize * 2) { e.Graphics.DrawImage(noriSprite, nori.x, nori.y, nori.size, nori.size); }

            e.Graphics.DrawImage(objects[6].sprite, objects[6].x, objects[6].y, objects[6].width, objects[6].height);
            e.Graphics.DrawImage(objects[7].sprite, objects[7].x, objects[7].y, objects[7].width, objects[7].height);

            //drawing nori if between row 1 + 2
            if (noriRec.Y < (bookshelfSize * 3) - 20 && noriRec.Y > bookshelfSize * 2) { e.Graphics.DrawImage(noriSprite, nori.x, nori.y, nori.size, nori.size); }

            e.Graphics.DrawImage(objects[8].sprite, objects[8].x, objects[8].y, objects[8].width, objects[8].height);
            e.Graphics.DrawImage(objects[9].sprite, objects[9].x, objects[9].y, objects[9].width, objects[9].height);

            //drawing nori if infront of row 1
            if (noriRec.Y > (bookshelfSize * 3) - 20 || nori.y == 420) { e.Graphics.DrawImage(noriSprite, nori.x, nori.y, nori.size, nori.size); }
            #endregion

            //drawing donna
            e.Graphics.DrawImage(objects[5].sprite, objects[5].x, objects[5].y, objects[5].width, objects[5].height);

            // for testing rectangle collisions
            Pen test = new Pen(Color.Red);
            foreach (Rectangle r in objectRecs) { e.Graphics.DrawRectangle(test, r); }
            e.Graphics.DrawRectangle(test, noriRec);

            if (displayTextBox == true)
            {
                e.Graphics.DrawRectangle(test, textBox);
            }

        }

        private void Pausing()
        {

            if (gameTimer.Enabled == true)
            {

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
                //loop
                //if (frameCounter == 5)
                //{
                //    noriSprite = noriAnimation[0 + spriteNumber];
                //    animationCounterH = 0;
                //    frameCounter = 6;
                //}
                //else if (frameCounter == 6)
                //{
                //    noriSprite = noriAnimation[1 + spriteNumber];
                //    animationCounterH = 0;
                //    frameCounter = 7;
                //}
                //else if (frameCounter == 7)
                //{
                //    noriSprite = noriAnimation[0 + spriteNumber];
                //    animationCounterH = 0;
                //    frameCounter = 1;
                //}

            }
        }
        private void DisplayTextCollisions()
        {

            text = textList[textNum];

            displayTextBox = true;
            textLabel.Visible = true;
            textLabel.Text = text;
            if (spaceDown == true)
            {
                displayTextBox = false;
                displayText = false;
                textLabel.Visible = false;
                textLabel.Text = "";
                Thread.Sleep(200);
            }

        }
    }
}
