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
        public ShopScreen()
        {
            InitializeComponent();
            OnStart();

            bookshelfSize = (this.Width - (BORDERWIDTH * 2)) / 4;

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

            Refresh();
        }
        private void ShopScreen_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush borderbrush = new SolidBrush(Color.Black);

            //drawing borders
            foreach (Rectangle r in border) { e.Graphics.FillRectangle(borderbrush, r); }

            //drawing nori
            e.Graphics.DrawImage(noriSprite, nori.x, nori.y, nori.size, nori.size);

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
