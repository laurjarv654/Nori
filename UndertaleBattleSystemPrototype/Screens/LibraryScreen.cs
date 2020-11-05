using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        #endregion

        #region Text box
        Boolean displayText = false, displayTextBox = false;
        Rectangle textBox;
        List<string> textList = new List<string>();
        string text = "";


        int textNum = 0;
        XmlReader reader;
        #endregion

        public LibraryScreen()
        {
            InitializeComponent();
            OnStart();

            int bookshelfSize = (this.Width - 396) / 4;

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
            objects.Add(new Object((this.Width/2)-(bookshelfSize/2), this.Height - 170, 50, bookshelfSize, Properties.Resources.libraryDoor));

            objects.Add(new Object(198, 150 - bookshelfSize, bookshelfSize, bookshelfSize, Properties.Resources.bookshelfB));
            objects.Add(new Object(198 + bookshelfSize, 150 - bookshelfSize, bookshelfSize, bookshelfSize, Properties.Resources.bookshelfR));
            objects.Add(new Object(this.Width - 198 - bookshelfSize, 150 - bookshelfSize, bookshelfSize, bookshelfSize, Properties.Resources.bookshelfG));
            objects.Add(new Object(this.Width - 198 - (bookshelfSize * 2), 150-bookshelfSize, bookshelfSize, bookshelfSize, Properties.Resources.bookshelfY));
            //objects.Add(new Object());
            #endregion

            #region initializing borders
            border.Add(new Rectangle(0, 0, this.Width, 150));
            border.Add(new Rectangle(0, 0, 198, this.Height));
            border.Add(new Rectangle(this.Width - 198, 0, 198, this.Height));
            border.Add(new Rectangle(0, this.Height - 150, this.Width, 150));

            #endregion

        }
        public void OnStart()
        {
            nori = new Player(this.Width / 2 - 75, this.Height - 300, 150, 0, 0);

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

            //setting the rectangles to the updated x,y
            noriRec = new Rectangle(nori.x + 40, nori.y + 150, 70, 20);

            if (wDown == true && sDown == false && nori.y >= 30)
            {
                nori.MoveUpDown(-HEROSPEED);
                direction = "up";
                animationCounterH++;
                spriteNumber = 0;
                NoriAnimation();
            }
            if (sDown == true && wDown == false && nori.y <= this.Height - 290)
            {
                nori.MoveUpDown(HEROSPEED);
                direction = "down";
                animationCounterH++;
                spriteNumber = 3;
                NoriAnimation();
            }
            if (dDown == true && aDown == false && nori.x <= this.Width - 310)
            {
                nori.MoveLeftRight(HEROSPEED);
                direction = "right";
                animationCounterH++;
                spriteNumber = 6;
                NoriAnimation();

            }
            if (aDown == true && dDown == false && nori.x >= 160)
            {
                nori.MoveLeftRight(-HEROSPEED);
                direction = "left";
                animationCounterH++;
                spriteNumber = 9;
                NoriAnimation();

            }
            #endregion

            //if (noriRec.IntersectsWith())

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
            foreach (Rectangle r in border)
            {
                e.Graphics.FillRectangle(borderbrush, r);
            }

            // for testing rectangle collisions
            Pen test = new Pen(Color.Red);
            foreach (Rectangle r in objectRecs)
            {
                e.Graphics.DrawRectangle(test, r);
            }

            //drawing objects
            foreach (Object o in objects)
            {
                e.Graphics.DrawImage(o.sprite, o.x, o.y, o.width, o.height);
            }

            //drawing nori
            e.Graphics.DrawImage(noriSprite, nori.x, nori.y, nori.size, nori.size);


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

    }
}
