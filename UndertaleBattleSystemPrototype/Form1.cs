using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UndertaleBattleSystemPrototype
{
    public partial class Form1 : Form
    {
        //undertale font for drawing
        PrivateFontCollection programFonts = new PrivateFontCollection();
        public static Font dialogFont;

        public Form1()
        {
            InitializeComponent();

            //initialize the dialog font
            programFonts.AddFontFile("Resources/dialogFont.ttf");
            dialogFont = new Font(programFonts.Families[0], 18, FontStyle.Regular);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MenuScreen ms = new MenuScreen();
            this.Controls.Add(ms);
            ms.Focus();
        }
    }
}
