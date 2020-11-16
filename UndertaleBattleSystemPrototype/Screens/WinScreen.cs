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

namespace UndertaleBattleSystemPrototype
{
    public partial class WinScreen : UserControl
    {
        #region variables

        //int for nori animation
        int noriAnimationCounter = -1;

        //strings for drawing the win text
        string winText = "Congratulations! You Win!";
        string winTextWritten = "";

        //object for nori
        Object nori;

        //rec and brush for drawing the win text
        Rectangle textRec;
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        #endregion variables

        public WinScreen()
        {
            InitializeComponent();

            //initialize nori start position
            nori = new Object(0, this.Height / 2, 120, 120, Properties.Resources.noriR1);

            //inititalize the text rec position
            textRec = new Rectangle(this.Width / 2 - 275, this.Height / 4, 550, 40);
        }

        #region win timer
        private void winTimer_Tick(object sender, EventArgs e)
        {
            if (nori.x < this.Width)
            {
                //move nori to the right until off-screen
                nori.MoveLeftRight(5);

                //draw the correct nori animation sprite
                noriAnimationCounter++;
                if (noriAnimationCounter == 0) { nori.sprite = Properties.Resources.noriR1; }
                if (noriAnimationCounter == 8) { nori.sprite = Properties.Resources.noriRR; }
                if (noriAnimationCounter == 16) { nori.sprite = Properties.Resources.noriR2; }
                if (noriAnimationCounter == 24)
                { 
                    nori.sprite = Properties.Resources.noriRR;
                    noriAnimationCounter = -8;
                }

                Refresh();
            }
            else if (winTextWritten != winText)
            {
                //write out the win text one character at a time until it has been fully written out
                foreach (char c in winText)
                {
                    winTextWritten += c;
                    Refresh();
                    Thread.Sleep(50);
                }
            }
            else
            {
                //wait 4 seconds before going back to the title screen
                Thread.Sleep(4000);

                winTimer.Enabled = false;

                MenuScreen ms = new MenuScreen();
                Form form = this.FindForm();

                form.Controls.Add(ms);
                form.Controls.Remove(this);

                ms.Focus();
                ms.Location = new Point((form.Width - ms.Width) / 2, (form.Height - ms.Height) / 2);
            }
        }
        #endregion win timer

        #region paint method
        private void WinScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(winTextWritten, Form1.dialogFontLarge, whiteBrush, textRec);

            e.Graphics.DrawImage(nori.sprite, nori.x, nori.y, nori.width, nori.height);
        }
        #endregion paint method
    }
}
