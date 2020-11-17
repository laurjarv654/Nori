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
    public partial class ShopScreen : UserControl
    {
        Boolean wDown, aDown, sDown, dDown, spaceDown, escapeDown;

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
        const int BORDERWIDTH = 300;
        int boothWidth, boothHeight;
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
        public ShopScreen()
        {
            InitializeComponent();
            OnStart();

            boothWidth = (this.Width - (BORDERWIDTH * 2)) / 3;
            boothHeight = (boothWidth / 5) * 4;

            #region initializing nori animation

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

            #region initializing borders
            //top
            border.Add(new Rectangle(0, 0, this.Width, ((boothWidth / 5) * 4) + 70));

            //left
            border.Add(new Rectangle(0, 0, BORDERWIDTH, this.Height));

            //right
            border.Add(new Rectangle(this.Width - BORDERWIDTH, 0, BORDERWIDTH, this.Height));

            //bottom
            border.Add(new Rectangle(0, this.Height - 150, this.Width, 150));

            #endregion

            #region initializing objects
            //door (0)
            objects.Add(new Object((this.Width / 2) - (boothWidth / 2), this.Height - 170, 50, boothWidth, Properties.Resources.libraryDoor));

            //back wall(1-3)
            objects.Add(new Object(BORDERWIDTH, 70, boothHeight, boothWidth + 10, Properties.Resources.wall));
            objects.Add(new Object(this.Width - BORDERWIDTH - boothWidth * 2, 70, boothHeight, boothWidth, Properties.Resources.bench1));
            objects.Add(new Object(this.Width - BORDERWIDTH - boothWidth, 70, boothHeight, boothWidth, Properties.Resources.mysteriousFigure));

            //side booths (4-7)
            objects.Add(new Object(BORDERWIDTH, 110, (boothWidth / 10) * 7, (boothHeight / 5) * 4, Properties.Resources.benchF2));
            objects.Add(new Object(BORDERWIDTH, 100 + (boothWidth / 10) * 7, (boothWidth / 10) * 7, (boothHeight / 5) * 4, Properties.Resources.benchF2));
            objects.Add(new Object(BORDERWIDTH, 90 + ((boothWidth / 10) * 7) * 2, (boothWidth / 10) * 7, (boothHeight / 5) * 4, Properties.Resources.benchF2));
            objects.Add(new Object(BORDERWIDTH, 80 + ((boothWidth / 10) * 7) * 3, ((boothWidth / 10) * 7) * 2, (boothHeight / 5) * 4, Properties.Resources.benchF1));

            //carpet (8)
            objects.Add(new Object(this.Width - BORDERWIDTH - boothWidth*2, (boothWidth/2) * 3, (boothHeight/2)*3,  (boothWidth/4)*7, Properties.Resources.carpet));

            //arlo(9)
            objects.Add(new Object(this.Width - BORDERWIDTH - (boothWidth / 10) * 9, this.Height - 150 - (boothWidth / 10) * 9, (boothWidth / 10) * 9, (boothWidth / 10) * 9, Properties.Resources.Arlo));

            #endregion

            //arlo rectangle
            objectRecs.Add(new Rectangle(objects[9].x+60, objects[9].y+100, objects[9].width-60, objects[9].height-100));

            //mysterious figure
            //objectRecs.Add(new Rectangle());
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
        private void ShopScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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
        private void ShopScreen_KeyUp(object sender, KeyEventArgs e)
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
            #region updating nori movement

            //setting the rectangles to the updated x,y
            noriRec = new Rectangle(nori.x + 40, nori.y + 120, 70, 30);

            if (wDown == true && nori.y >= (boothWidth / 4) * 5 - nori.size)
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
            if (aDown == true && dDown == false && nori.x >= BORDERWIDTH + (boothHeight / 5) * 4 - 20)
            {
                nori.MoveLeftRight(-HEROSPEED);
                direction = "left";
                animationCounterH++;
                spriteNumber = 9;
                NoriAnimation();

            }
            #endregion

            #region collisions

            if (noriRec.IntersectsWith(objectRecs[0]) && spaceDown == true)
            {
                if (gameTimer.Enabled == true)
                {
                    wDown = aDown = sDown = dDown = spaceDown = false;
                    gameTimer.Enabled = false;
                    ShopMenu shopForm = new ShopMenu();
                    DialogResult dr = ShopMenu.Show();

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

            #endregion

            Refresh();
        }
        private void ShopScreen_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush borderbrush = new SolidBrush(Color.Black);

            //drawing borders
            foreach (Rectangle r in border) { e.Graphics.FillRectangle(borderbrush, r); }

            for (int i = 0; i<=8; i++) { e.Graphics.DrawImage(objects[i].sprite, objects[i].x, objects[i].y, objects[i].width, objects[i].height); }

            //drawing nori
            e.Graphics.DrawImage(noriSprite, nori.x, nori.y, nori.size, nori.size);

            //drawing Arlo
            e.Graphics.DrawImage(objects[9].sprite, objects[9].x, objects[9].y, objects[9].width, objects[9].height);

            //rectangles
            Pen test = new Pen(Color.Red);
            foreach (Rectangle r in objectRecs) { e.Graphics.DrawRectangle(test, r); }
            e.Graphics.DrawRectangle(test, noriRec);

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
    }
}
