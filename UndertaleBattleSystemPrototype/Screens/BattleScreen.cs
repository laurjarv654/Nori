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
using UndertaleBattleSystemPrototype.Classes;
using UndertaleBattleSystemPrototype.Properties;
using System.IO;
using System.Drawing.Text;
using System.Resources;
using System.Diagnostics;

namespace UndertaleBattleSystemPrototype
{
    public partial class BattleScreen : UserControl
    {
        #region variables and lists

        #region enemy attack variables (sorry these are in a section of their own, it made it easier at the time to find them)
        //enemy attack variables
        Boolean attackVariablesSet = false;
        Boolean hornLeft = true;
        Boolean hornSpaceChange = false;
        Boolean leavesLeft = true;
        public static Boolean playerInvincible = false; //has to be global for access in the player class
        int invincibilityTimer = 50;
        int attackSpeed = 0;
        int spaceBetweenAttacks = 0;
        int attackPauseTimer = 0;

        #endregion enemy attack variables (sorry these are in a section of their own, it made it easier at the time to find them)

        #region xml readers, brushes, images, and other
        //create an xml reader for the enemy file and player file
        XmlReader eReader, pReader;

        //brush for walls, player attacks, and hp bars
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush attackBrush = new SolidBrush(Color.Indigo);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush yellowBrush = new SolidBrush(Color.Yellow);

        //create a new image for the player sprite and button sprites
        Image playerSprite, fightSprite, actSprite, itemSprite, mercySprite, fightUISprite;

        //create an image for the enemy sprite
        Image enemySprite;

        //create the player
        Player player = new Player();

        //create the enemy
        Enemy enemy = new Enemy();
        #endregion xml readers, brushes, images, and other

        #region ints and strings
        //string for after battle result
        public static string enemySpared = "blank";

        //int for after turn counting and enemy turn counting
        int afterTurnCounter = 0;
        int enemyTurnCounter = 500;

        //ints for sparing and dialog
        int spareNum = -1;
        int lastSpareNum = -2;

        //string for damage number drawing
        string playerDamageNum;

        //string for enemy attack name
        string attackName;
        #endregion ints and strings

        #region booleans
        //make a globally useable spare variable
        public static Boolean canSpare = false;

        //player key press variables
        Boolean wDown, aDown, sDown, dDown, spaceDown, shiftDown;

        //boolean for checking if it's the enemy's turn or not 
        Boolean enemyTurn = false;

        //booleans for checking what menu the player is in
        Boolean fightMenuSelected = false, actMenuSelected = false, itemMenuSelected = false;

        //boolean for checking if the player has made an attack
        Boolean playerAttack = false;

        //boolean for checking if the enemy dialog should be shown or not
        Boolean showEnemyDialog = false;
        #endregion booleans

        #region rectangles
        //create rectangles for buttons
        Rectangle fightRec, actRec, itemRec, mercyRec;

        //rectangle for attacking UI
        Rectangle attackRec;

        //create a rectangle for player collision
        Rectangle playerRec;

        //health bar rectangles
        Rectangle maxHPRec, remainingHPRec, enemyMaxHPRec, enemyRemainingHPRec;

        //rectangle for enemy dialog box
        Rectangle enemyDialogBox;
        #endregion rectangles

        #region lists
        //new list for battle area walls
        List<Rectangle> arenaWalls = new List<Rectangle>();

        //lists for acting
        List<string> actNames = new List<string>() { " ", " ", " ", " " };
        List<string> actText = new List<string>() { " ", " ", " ", " " };
        List<int> spareValues = new List<int>() { -1, -1, -1, -1 };
        List<int> itemHeals = new List<int>() { 0, 0, 0, 0 };

        //lists for enemy turn
        List<string> enemyAttacks = new List<string>();
        List<int> enemyAttackValues = new List<int>();
        List<Rectangle> attackRecs = new List<Rectangle>();
        List<Projectile> attacks = new List<Projectile>();

        //lists for enemy dialog and after enemy turn text
        List<string> enemyDialog = new List<string>();
        List<string> afterEnemyTurnText = new List<string>();
        #endregion lists

        //random number generator
        Random randNum = new Random();

        #endregion variables and lists

        #region battle system brought up
        public BattleScreen()
        {
            InitializeComponent();

            //screen setup
            OnStart();

            //call the attack type method
            AttackType();

            //hide the cursor
            Cursor.Hide();

            //set images
            playerSprite = Resources.heart;
            fightSprite = Resources.fightButton;
            actSprite = Resources.actButton;
            itemSprite = Resources.itemButton;
            mercySprite = Resources.mercyButton;
            fightUISprite = Resources.fightUISprite;
            if (TownScreen.enemyName == "Calum") { enemySprite = Resources.Calum_FS; }
            if (TownScreen.enemyName == "Franky") { enemySprite = Resources.Franky_FS; }

            //music player
            SoundPlayer music = new SoundPlayer("Resources/Nori - " + TownScreen.enemyName + "'s Theme.wav");

            //play the music on loop
            //music.PlayLooping();
        }
        #endregion battle system brought up

        #region setup
        public void OnStart()
        {
            //ensure these variables are reset correctly (VERY IMPORTANT)
            enemySpared = "blank";
            canSpare = false;

            //health bar rectangles
            maxHPRec = new Rectangle(this.Width / 2 - 40, this.Height - 130, 80, 20);
            remainingHPRec = new Rectangle(this.Width / 2 - 40, this.Height - 130, 80, 20);
            enemyMaxHPRec = new Rectangle(this.Width / 2 - 100, 50, 200, 20);
            enemyRemainingHPRec = new Rectangle(this.Width / 2 - 100, 50, 200, 20);

            //set button positions and sizes
            fightRec = new Rectangle(this.Width / 4 - 190, this.Height - 100, 140, 50);
            actRec = new Rectangle(this.Width / 2 - 190, this.Height - 100, 140, 50);
            itemRec = new Rectangle((this.Width / 2) + (this.Width / 4) - 190, this.Height - 100, 140, 50);
            mercyRec = new Rectangle(this.Width - 190, this.Height - 100, 140, 50);

            //create rectangles for the arena walls
            Rectangle leftWall = new Rectangle(fightRec.X, fightRec.Y - 250, 5, 200);
            Rectangle rightWall = new Rectangle(mercyRec.X + 135, mercyRec.Y - 250, 5, 200);
            Rectangle topWall = new Rectangle(fightRec.X, fightRec.Y - 250, rightWall.X - leftWall.X + 5, 5);
            Rectangle bottomWall = new Rectangle(fightRec.X, fightRec.Y - 50, rightWall.X - leftWall.X + 5, 5);

            //add the walls to the arena walls list
            arenaWalls.Add(leftWall);
            arenaWalls.Add(rightWall);
            arenaWalls.Add(topWall);
            arenaWalls.Add(bottomWall);

            //set the enemy dialog box position
            enemyDialogBox = new Rectangle(mercyRec.X - 60, this.Height / 8, mercyRec.Width + 80, 150);

            //set the text of the act labels
            actLabel1.Text = "  " + actNames[0];
            actLabel2.Text = "* " + actNames[1];
            actLabel3.Text = "* " + actNames[2];
            actLabel4.Text = "* " + actNames[3];

            //fill in enemy details for battle use
            eReader = XmlReader.Create("Resources/" + TownScreen.enemyName + ".xml");

            eReader.ReadToFollowing("Stats");
            enemy.hp = Convert.ToInt16(eReader.GetAttribute("hp"));
            enemy.atk = Convert.ToInt16(eReader.GetAttribute("atk"));
            enemy.gold = Convert.ToInt16(eReader.GetAttribute("gold"));

            while (eReader.Read())
            {
                eReader.ReadToFollowing("Text");
                enemyDialog.Add(eReader.GetAttribute("string"));
            }

            eReader.Close();

            eReader = XmlReader.Create("Resources/" + TownScreen.enemyName + ".xml");

            while (eReader.Read())
            {
                eReader.ReadToFollowing("FlavorText");
                afterEnemyTurnText.Add("* " + eReader.GetAttribute("line1") + "\n\n* " + eReader.GetAttribute("line2") + "\n\n* " + eReader.GetAttribute("line3"));
            }

            eReader.Close();

            //fill in player details for battle use
            pReader = XmlReader.Create("Resources/Player.xml");

            pReader.ReadToFollowing("Battle");
            player.hp = Convert.ToInt16(pReader.GetAttribute("currentHP"));
            player.atk = Convert.ToInt16(pReader.GetAttribute("atk"));

            pReader.Close();

            //fill in player start position
            player.x = fightRec.X + 15;
            player.y = fightRec.Y + 15;
            player.size = 20;
        }
        #endregion setup

        #region key down and up
        private void BattleScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player button presses
            switch (e.KeyCode)
            {
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
                case Keys.Space:
                    spaceDown = true;
                    break;
                case Keys.ShiftKey:
                    shiftDown = true;
                    break;
            }
        }

        private void BattleScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player button releases
            switch (e.KeyCode)
            {
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
                case Keys.Space:
                    spaceDown = false;
                    break;
                case Keys.ShiftKey:
                    shiftDown = false;
                    break;
            }
        }

        #endregion key down and up

        #region movement, collisions, and menus (gameloop)
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //update the position of the player collision
            playerRec = new Rectangle(player.x, player.y, player.size, player.size);

            #region fighting area code

            //if it is the enemy's turn...
            if (enemyTurn == true && enemyTurnCounter > 0 && canSpare == false)
            {
                EnemyAttacks(enemyTurnCounter);

                enemyTurnCounter--;

                #region player collision
                //check if the player collides with any attack rec
                foreach (Rectangle r in attackRecs)
                {
                    //if the player is out of invincibility frames, make the player not invincible anymore.
                    //if the player still has invincibility left, the timer goes down.
                    if (playerInvincible == true && invincibilityTimer <= 0)
                    {
                        playerInvincible = false;
                        invincibilityTimer = 50;
                    }
                    else if (playerInvincible == true)
                    {
                        invincibilityTimer--;
                    }

                    //if it does, do damage accordingly
                    if (playerRec.IntersectsWith(r))
                    {
                        player.AttackCollision(enemy.atk, playerInvincible);

                        if (player.hp <= 0)
                        {
                            //stop the game timer
                            gameTimer.Enabled = false;

                            //go back to the town screen
                            LoseScreen ls = new LoseScreen();
                            TownScreen ts = new TownScreen();
                            Form form = this.FindForm();
                            form.Controls.Add(ls);
                            form.Controls.Remove(this);
                            form.Controls.Remove(ts);
                            ls.Focus();
                        }
                    }
                }
                #endregion player collision

                #region player movement
                //player movement
                if (dDown == true && player.x < arenaWalls[1].X - arenaWalls[1].Width - player.size)
                {
                    player.MoveLeftRight(8);
                }
                if (aDown == true && player.x > arenaWalls[0].X + arenaWalls[0].Width)
                {
                    player.MoveLeftRight(-8);
                }
                if (wDown == true && player.y > arenaWalls[2].Y + arenaWalls[2].Height)
                {
                    player.MoveUpDown(-8);
                }
                if (sDown == true && player.y < arenaWalls[3].Y - arenaWalls[3].Height - player.size)
                {
                    player.MoveUpDown(8);
                }
                #endregion player movement
            }

            //if the enemy turn is over
            #region enemy turn over

            else if (enemyTurn == true && (enemyTurnCounter <= 0 || canSpare == true))
            {
                //reset the enemy turn counter
                enemyTurnCounter = 500;
                enemyTurn = false;

                //display the output box with the appropriate text
                if (spareNum == 0) { textOutput.Text = afterEnemyTurnText[0]; }
                else { textOutput.Text = afterEnemyTurnText[randNum.Next(1, afterEnemyTurnText.Count - 1)]; }
                textOutput.Visible = true;

                //clear any attacks off the screen
                attackRecs.Clear();
                attacks.Clear();

                //reset the arena walls
                Rectangle leftWall = new Rectangle(fightRec.X, fightRec.Y - 250, 5, 200);
                Rectangle rightWall = new Rectangle(mercyRec.X + 135, mercyRec.Y - 250, 5, 200);
                Rectangle topWall = new Rectangle(fightRec.X, fightRec.Y - 250, rightWall.X - leftWall.X + 5, 5);
                Rectangle bottomWall = new Rectangle(fightRec.X, fightRec.Y - 50, rightWall.X - leftWall.X + 5, 5);

                //add the walls to the arena walls list
                arenaWalls.Clear();
                arenaWalls.Add(leftWall);
                arenaWalls.Add(rightWall);
                arenaWalls.Add(topWall);
                arenaWalls.Add(bottomWall);

                //set the player on the fight button
                player.x = fightRec.X + 15;
                player.y = fightRec.Y + 15;
            }

            #endregion enemy turn over

            #endregion fighting area code

            #region buttons code

            //if player is in the buttons and menus...
            else
            {
                if (fightMenuSelected == true) { FightUI(); }
                if (actMenuSelected == true) { ActMenu(); }
                if (itemMenuSelected == true) { ItemMenu(); }

                //check which button the player is currently on and set it to the blank version of the button's sprite
                //if player moves to a different button, change button sprite back and set player position to the new button
                #region fight
                if (playerRec.IntersectsWith(fightRec))
                {
                    fightSprite = Resources.fightButtonBlank;

                    //go into the fight menu
                    if (spaceDown == true)
                    {
                        //hide the text output box
                        textOutput.Visible = false;

                        //set boolean for fight menu check to true
                        fightMenuSelected = true;

                        //set the attackRec position for fighting
                        attackRec = new Rectangle(arenaWalls[0].X + 5, arenaWalls[0].Y + 5, 15, 190);

                        //move the player off-screen during player attack
                        player.x = -20;
                        player.y = -20;

                        Thread.Sleep(150);
                    }
                    if (dDown == true)
                    {
                        fightSprite = Resources.fightButton;
                        player.x = actRec.X + 15;
                        player.y = actRec.Y + 15;

                        Thread.Sleep(150);
                    }
                }
                #endregion fight

                #region act
                if (playerRec.IntersectsWith(actRec))
                {
                    actSprite = Resources.actButtonBlank;
                    actMenuSelected = false;

                    //go into the act menu
                    if (spaceDown == true)
                    {
                        //set actions to be visible and put player in the action menu
                        MenuDisplay();

                        //setup the act menu for the current enemy
                        ActMenuText();

                        //set boolean for act menu check to true
                        actMenuSelected = true;

                        Thread.Sleep(150);
                    }
                    if (aDown == true)
                    {
                        actSprite = Resources.actButton;
                        player.x = fightRec.X + 15;
                        player.y = fightRec.Y + 15;

                        Thread.Sleep(150);
                    }
                    if (dDown == true)
                    {
                        actSprite = Resources.actButton;
                        player.x = itemRec.X + 15;
                        player.y = itemRec.Y + 15;

                        Thread.Sleep(150);
                    }
                }

                #endregion act

                #region item
                if (playerRec.IntersectsWith(itemRec))
                {
                    itemSprite = Resources.itemButtonBlank;
                    itemMenuSelected = false;

                    //go into the item menu
                    if (spaceDown == true)
                    {
                        //setup the item menu for the current enemy
                        ItemMenuText();

                        if (actLabel1.Text == "*  " || actNames[0] == null)
                        {
                            actLabel1.Visible = false;
                            textOutput.Text = "You can't do that!";

                            Refresh();
                            Thread.Sleep(150);

                            player.x = itemRec.X + 15;
                            player.y = itemRec.Y + 15;
                        }
                        else
                        {
                            //set actions to be visible and put player in the action menu
                            MenuDisplay();

                            //set boolean for item menu check to true
                            itemMenuSelected = true;

                            Thread.Sleep(150);
                        }
                    }
                    if (aDown == true)
                    {
                        itemSprite = Resources.itemButton;
                        player.x = actRec.X + 15;
                        player.y = actRec.Y + 15;

                        Thread.Sleep(150);
                    }
                    if (dDown == true)
                    {
                        itemSprite = Resources.itemButton;
                        player.x = mercyRec.X + 15;
                        player.y = mercyRec.Y + 15;

                        Thread.Sleep(150);
                    }
                }
                #endregion item

                #region mercy
                if (playerRec.IntersectsWith(mercyRec))
                {
                    //check if the enemy is spare-able and set the button sprite accordingly
                    if (canSpare == true) { mercySprite = Resources.mercyButtonSpareBlank; }
                    else { mercySprite = Resources.mercyButtonBlank; }

                    //go into the mercy menu
                    if (spaceDown == true)
                    {
                        //check if the enemy is spare-able or not
                        if (canSpare == false)
                        {
                            //set the action result
                            actText[0] = "* You showed mercy." + "\n\n* ...";

                            //call the menu disappear method
                            MenuDisappear(0);
                        }
                        else
                        {
                            //set the action result to show that the player won
                            actText[0] = "* You Won!";

                            //set the enemy spared string to "spared"
                            enemySpared = "spared";

                            //call the player save update method
                            playerSaveUpdate();

                            //call the menu disappear method
                            MenuDisappear(0);

                            gameTimer.Enabled = false;

                            //go back to the town screen
                            gameTimer.Enabled = false;
                            Form form = this.FindForm();
                            form.Controls.Remove(this);
                        }
                    }
                    if (aDown == true)
                    {
                        if (canSpare == true) { mercySprite = Resources.mercyButtonSpare; }
                        else { mercySprite = Resources.mercyButton; }

                        player.x = itemRec.X + 15;
                        player.y = itemRec.Y + 15;

                        Thread.Sleep(150);
                    }
                }
                #endregion mercy
            }

            #endregion buttons code

            //update the health bar and hp label depending on the player's hp
            remainingHPRec.Width = player.hp * 2;
            hpValueLabel.Text = player.hp + " / 40";

            //update the enemy's health bar depending on the enemey's hp
            enemyRemainingHPRec.Width = enemy.hp * 2;

            Refresh();
        }
        #endregion movement, collisions, and menus (gameloop)

        #region paint graphics
        private void BattleScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw the enemy sprite
            e.Graphics.DrawImage(enemySprite, this.Width / 2 - enemySprite.Width / 2, this.Height / 20);

            #region enemy attacks

            foreach (Projectile p in attacks)
            {
               e.Graphics.DrawImage(p.image, p.x, p.y, p.width, p.height);
            }

            if (attackName != "HoofAttack")
            {
                e.Graphics.FillRectangle(blackBrush, 0, arenaWalls[2].Y - 50, this.Width, 50);
                e.Graphics.FillRectangle(blackBrush, 0, arenaWalls[3].Y + 5, this.Width, 100);
            }

            #endregion enemy attacks

            #region enemy health bar

            //draw the enemy health bar and damage if the player has attacked, then pause before going to enemy turn
            if (playerAttack == true)
            {
                e.Graphics.FillRectangle(redBrush, enemyMaxHPRec);
                e.Graphics.FillRectangle(yellowBrush, enemyRemainingHPRec);

                damageLabel.Visible = true;
                damageLabel.Text = playerDamageNum;

                afterTurnCounter++;

                //if 2 seconds have passed then hide the enemy health bar and reset the after turn counter
                if (afterTurnCounter >= 100)
                {
                    playerAttack = false;
                    damageLabel.Visible = false;

                    //call the turn made method if enemy is not defeated yet, else "You Win!", and gain gold
                    if (enemy.hp > 0) { TurnMade(); }
                    else 
                    {
                        //set the action result to show that the player won
                        actText[0] = "* You Won! \n\n* You got " + enemy.gold + " gold.";

                        //set the enemy spared string to "killed"
                        enemySpared = "killed";

                        //call the player save update method
                        playerSaveUpdate();

                        //call the menu disappear method
                        MenuDisappear(0);

                        //go back to the town screen
                        gameTimer.Enabled = false;
                        Form form = this.FindForm();
                        form.Controls.Remove(this);
                    }

                    afterTurnCounter = 0;
                }
            }

            #endregion enemy health bar

            #region player UI

            //draw the buttons
            e.Graphics.DrawImage(fightSprite, fightRec);
            e.Graphics.DrawImage(actSprite, actRec);
            e.Graphics.DrawImage(itemSprite, itemRec);
            e.Graphics.DrawImage(mercySprite, mercyRec);

            //draw the health bar
            e.Graphics.FillRectangle(redBrush, maxHPRec);
            e.Graphics.FillRectangle(yellowBrush, remainingHPRec);

            //draw the player
            e.Graphics.DrawImage(playerSprite, player.x, player.y);

            #endregion player UI

            //draw the text box/arena walls
            foreach (Rectangle r in arenaWalls)
            {
                e.Graphics.FillRectangle(whiteBrush, r);
            }

            #region enemy dialog box

            //draw the enemy dialog box if it's time
            if (showEnemyDialog == true)
            {
                e.Graphics.FillRectangle(whiteBrush, enemyDialogBox);

                //draw the correct enemy dialog string in the dialog box
                if (spareNum == -1 || spareNum == lastSpareNum)
                {
                    e.Graphics.DrawString("...", Form1.dialogFont, blackBrush, enemyDialogBox);
                }
                else 
                {
                    e.Graphics.DrawString(enemyDialog[spareNum], Form1.dialogFont, blackBrush, enemyDialogBox);
                }

                //set the last spare number to the current one for the next time dialog is written
                lastSpareNum = spareNum;
            }

            #endregion enemy dialog box

            //draw the fight UI if the player is in the fight menu
            if (fightMenuSelected == true)
            {
                e.Graphics.DrawImage(fightUISprite, arenaWalls[0].X + 5, arenaWalls[2].Y + 5, arenaWalls[1].X - arenaWalls[0].X - 5, 195);
                e.Graphics.FillRectangle(attackBrush, attackRec);
            }
        }
        #endregion paint graphics

        #region menu methods

        #region fight UI
        private void FightUI()
        {
            //if the player presses space, check where the attack rec is and do a damage calulation accordingly
            if (spaceDown == true)
            {
                //int for the center of the attack rec and the damage number for doing damage to the enemy
                int atkCenter = attackRec.X + attackRec.Width / 2;
                int damageNum = 0;

                //if attack is in the red areas of the fight UI do damage accordingly
                if (atkCenter > 50 && atkCenter < 300 || atkCenter > 638 && atkCenter < 888)
                {
                    damageNum = randNum.Next(player.atk, player.atk * 2);
                }
                //if attack is in the yellow areas of the fight UI do damage accordingly
                if (atkCenter > 300 && atkCenter < 450 || atkCenter > 488 && atkCenter < 638)
                {
                    damageNum = 2 * (randNum.Next(player.atk, player.atk * 2));
                }
                //if attack is in the green area of the fight UI do damage accordingly
                if (atkCenter > 450 && atkCenter < 488)
                {
                    damageNum = 3 * (randNum.Next(player.atk, player.atk * 2));
                }

                //if the enemy is spare-able, instant kill. (Betrayal, they aren't on-guard anymore)
                if (canSpare == true) { damageNum = enemy.hp; }

                //subtract the damage number from the enemy's hp
                enemy.hp -= damageNum;

                //set the player damage number string to the damage dealt for drawing
                playerDamageNum = Convert.ToString(damageNum);

                //set player attack boolean to true for drawing the enemy health bar and damage
                //also set the fightMenuSelected and spaceDown boolean to false so no fight UI reappears
                playerAttack = true;
                fightMenuSelected = false;
                spaceDown = false;
            }
            else if (attackRec.X > arenaWalls[1].X)
            {
                //set the player damage number string to "miss" for drawing
                playerDamageNum = "MISS";

                //set player attack boolean to true for drawing the enemy health bar and damage
                //also set the fightMenuSelected and false so no fight UI reappears
                playerAttack = true;
                fightMenuSelected = false;
            }
            else
            {
                attackRec.X += 20;
            }
        }
        #endregion fight UI

        #region act menu
        private void ActMenu()
        {
            //if for player exiting the act menu
            if (shiftDown == true)
            {
                //show the main text output
                textOutput.Visible = true;

                //hide the act labels
                actLabel1.Visible = false;
                actLabel2.Visible = false;
                actLabel3.Visible = false;
                actLabel4.Visible = false;

                //set player back to the act button
                player.x = actRec.X + 15;
                player.y = actRec.Y + 15;

                //stop ActMenu() from being called when act menu is exited
                actMenuSelected = false;

                Thread.Sleep(150);
            }

            //call the menus method
            Menus();
        }
        #endregion act menu
        #region act menu text
        private void ActMenuText()
        {
            //create a counter
            int i = 0;

            //set reader to beginning of enemy file
            eReader = XmlReader.Create("Resources/" + TownScreen.enemyName + ".xml");

            while (eReader.Read() && i < 4)
            {
                //check what step of sparing the player is on 
                //(0 is spare-able, -1 is a negative action, 1 to infinity is a step forward/neutral action)
                if (spareNum == 0 || spareNum == -1) { eReader.ReadToFollowing("Act"); }
                else { eReader.ReadToFollowing("Act" + spareNum); }

                //fill out the proper details for each act option
                spareValues[i] = Convert.ToInt16(eReader.GetAttribute("spareValue"));
                actNames[i] = eReader.GetAttribute("actName");
                actText[i] = "* " + eReader.GetAttribute("actLine1") + "\n\n* " + eReader.GetAttribute("actLine2") + "\n\n* " + eReader.GetAttribute("actLine3");

                //add 1 to the counter
                i++;
            }

            //add an extra value to the spare values list that will always be -1
            spareValues.Add(-1);

            eReader.Close();

            //these lines of code are nessecary for the initial display of each label
            actLabel1.Text = "* " + actNames[0];
            actLabel2.Text = "* " + actNames[1];
            actLabel3.Text = "* " + actNames[2];
            actLabel4.Text = "* " + actNames[3];
        }
        #endregion act menu text

        #region item menu
        private void ItemMenu()
        {
            //if for player exiting the item menu
            if (shiftDown == true)
            {
                //show the main text output
                textOutput.Visible = true;

                //hide the act labels
                actLabel1.Visible = false;
                actLabel2.Visible = false;
                actLabel3.Visible = false;
                actLabel4.Visible = false;

                //set player back to the item button
                player.x = itemRec.X + 15;
                player.y = itemRec.Y + 15;

                //stop ItemMenu() from being called when item menu is exited
                itemMenuSelected = false;

                Thread.Sleep(150);
            }

            //call the menus method
            Menus();
        }
        #endregion item menu
        #region item menu text
        private void ItemMenuText()
        {
            //create a counter
            int i = 0;

            //set reader to the Items section of the player xml file
            pReader = XmlReader.Create("Resources/Player.xml");

            while (pReader.Read() && i < 4)
            {
                //gather and set item info for each item
                pReader.ReadToFollowing("Item");
                actNames[i] = pReader.GetAttribute("name");
                itemHeals[i] = Convert.ToInt16(pReader.GetAttribute("heal"));

                actText[i] = "* You ate the " + actNames[i] + "\n\n* ..." + "\n\n* You recovered " + itemHeals[i] + " HP!";

                i++;
            }

            pReader.Close();

            //display items
            actLabel1.Text = "* " + actNames[0];
            actLabel2.Text = "* " + actNames[1];
            actLabel3.Text = "* " + actNames[2];
            actLabel4.Text = "* " + actNames[3];
        }
        #endregion item menu text

        #region general menu code
        private void Menus()
        {
            //disable act options if they are blank
            if (actLabel2.Text == "*  " || actNames[1] == null) { actLabel2.Visible = false; }
            if (actLabel3.Text == "*  " || actNames[2] == null) { actLabel3.Visible = false; }
            if (actLabel4.Text == "*  " || actNames[3] == null) { actLabel4.Visible = false; }

            //check which act option the player is on and do things accordingly
            #region option selection
            if (player.x == actLabel1.Location.X && player.y == actLabel1.Location.Y + 5)
            {
                actLabel1.Text = "  " + actNames[0];

                //call the menu disappear method and go back to the fight button
                if (spaceDown == true)
                {
                    MenuDisappear(0);
                }
                //move to option 3
                if (sDown == true && actLabel3.Visible == true)
                {
                    actLabel1.Text = "* " + actNames[0];
                    player.x = actLabel3.Location.X;
                    player.y = actLabel3.Location.Y + 5;

                    Thread.Sleep(150);
                }
                //move to option 2
                if (dDown == true && actLabel2.Visible == true)
                {
                    actLabel1.Text = "* " + actNames[0];
                    player.x = actLabel2.Location.X;
                    player.y = actLabel2.Location.Y + 5;

                    Thread.Sleep(150);
                }
            }
            if (player.x == actLabel2.Location.X && player.y == actLabel2.Location.Y + 5)
            {
                actLabel2.Text = "  " + actNames[1];

                //call the menu disappear method and go back to the fight button
                if (spaceDown == true)
                {
                    MenuDisappear(1);
                }
                //move to option 1
                if (aDown == true && actLabel1.Visible == true)
                {
                    actLabel2.Text = "* " + actNames[1];
                    player.x = actLabel1.Location.X;
                    player.y = actLabel1.Location.Y + 5;

                    Thread.Sleep(150);
                }
                //move to option 4
                if (sDown == true && actLabel4.Visible == true)
                {
                    actLabel2.Text = "* " + actNames[1];
                    player.x = actLabel4.Location.X;
                    player.y = actLabel4.Location.Y + 5;

                    Thread.Sleep(150);
                }
            }
            if (player.x == actLabel3.Location.X && player.y == actLabel3.Location.Y + 5)
            {
                actLabel3.Text = "  " + actNames[2];

                //call the menu disappear method and go back to the fight button
                if (spaceDown == true)
                {
                    MenuDisappear(2);
                }
                //move to option 1
                if (wDown == true && actLabel1.Visible == true)
                {
                    actLabel3.Text = "* " + actNames[2];
                    player.x = actLabel1.Location.X;
                    player.y = actLabel1.Location.Y + 5;

                    Thread.Sleep(150);
                }
                //move to option 4
                if (dDown == true && actLabel4.Visible == true)
                {
                    actLabel3.Text = "* " + actNames[2];
                    player.x = actLabel4.Location.X;
                    player.y = actLabel4.Location.Y + 5;

                    Thread.Sleep(150);
                }
            }
            if (player.x == actLabel4.Location.X && player.y == actLabel4.Location.Y + 5)
            {
                actLabel4.Text = "  " + actNames[3];

                //call the menu disappear method and go back to the fight button
                if (spaceDown == true)
                {
                    MenuDisappear(3);
                }
                //move to option 2
                if (wDown == true && actLabel2.Visible == true)
                {
                    actLabel4.Text = "* " + actNames[3];
                    player.x = actLabel2.Location.X;
                    player.y = actLabel2.Location.Y + 5;

                    Thread.Sleep(150);
                }
                //move to option 3
                if (aDown == true && actLabel3.Visible == true)
                {
                    actLabel4.Text = "* " + actNames[3];
                    player.x = actLabel3.Location.X;
                    player.y = actLabel3.Location.Y + 5;

                    Thread.Sleep(150);
                }
            }
            #endregion option selection
        }
        #endregion general menu code
        #region menu disappear code
        private void MenuDisappear(int i)
        {
            if (actText[i] != "* You showed mercy." + "\n\n* ...")
            {
                //set the spare number depending on the action selected
                spareNum = spareValues[i];
            }

            //check if the player has made the correct choices for the enemy to be spared
            if (spareNum == 0) { canSpare = true; }

            //set output text to the appropraite message and make it visible
            textOutput.Text = actText[i];
            textOutput.Visible = true;

            //make all act options invisible
            actLabel1.Visible = false;
            actLabel2.Visible = false;
            actLabel3.Visible = false;
            actLabel4.Visible = false;

            //add hp to player if item was used and remove the item
            if (itemMenuSelected == true)
            {
                player.hp += itemHeals[i];
                if (player.hp > 40) { player.hp = 40; }

                PlayerXmlUpdate(i);
            }

            //set all buttons to their non-active state
            fightSprite = Resources.fightButton;
            actSprite = Resources.actButton;
            itemSprite = Resources.itemButton;
            if (canSpare == true) { mercySprite = Resources.mercyButtonSpare; }
            else { mercySprite = Resources.mercyButton; }

            //call the turn made method
            TurnMade();
        }
        #endregion menu disappear code
        #region menu display code
        private void MenuDisplay()
        {
            //make the text output not visible
            textOutput.Visible = false;

            ///make the act labels visible
            actLabel1.Visible = true;
            actLabel2.Visible = true;
            actLabel3.Visible = true;
            actLabel4.Visible = true;

            //set player position to the act1 label
            player.x = actLabel1.Location.X;
            player.y = actLabel1.Location.Y + 5;
        }
        #endregion menu display code
        #region turn made code (going into the enemy turn)
        private void TurnMade()
        {
            //make the player disappear
            player.x = -20;
            player.y = -20;

            //wait for 3 seconds before showing the enemy dialog if an act was made (so that the player can read lol)
            if (textOutput.Visible == true)
            {
                Refresh();
                Thread.Sleep(3000);
            }

            //make the main output label invisible
            textOutput.Visible = false;

            //wait for 3 seconds before the enemy turn (again, so the player can read lol)
            if (enemySpared == "blank")
            {
                //display the enemy dialog box
                showEnemyDialog = true;
                Refresh();
                Thread.Sleep(3000);
            }

            //stop showing the enemy dialog
            showEnemyDialog = false;

            //set enemy turn boolean to true
            enemyTurn = true;

            //set the player in the middle of the battle area
            player.x = this.Width / 2 - 10;
            player.y = arenaWalls[2].Y + (arenaWalls[3].Y - arenaWalls[2].Y) / 2;

            //resize the arena area
            Rectangle leftWall = new Rectangle(actRec.X, actRec.Y - 250, 5, 200);
            Rectangle rightWall = new Rectangle(itemRec.X + 135, itemRec.Y - 250, 5, 200);
            Rectangle topWall = new Rectangle(actRec.X, actRec.Y - 250, rightWall.X - leftWall.X + 5, 5);
            Rectangle bottomWall = new Rectangle(actRec.X, actRec.Y - 50, rightWall.X - leftWall.X + 5, 5);

            //add the new walls to the arena walls list
            arenaWalls.Clear();
            arenaWalls.Add(leftWall);
            arenaWalls.Add(rightWall);
            arenaWalls.Add(topWall);
            arenaWalls.Add(bottomWall);

            //randomly choose an attack to do from the available attacks
            int attackValue = randNum.Next(enemyAttackValues.Count());

            //set the attack name correctly
            attackName = enemyAttacks[attackValue];
        }
        #endregion turn made code (going into the enemy turn)

        #endregion menu methods

        #region enemy turn methods

        #region attack type method (filling in the possible attacks)
        private void AttackType()
        {
            //read from the enemy xml file
            eReader = XmlReader.Create("Resources/" + TownScreen.enemyName + ".xml");

            //fill in the enemy attacks lists from the enemy xml
            while (eReader.Read())
            {
                eReader.ReadToFollowing("Attack");
                enemyAttacks.Add(eReader.GetAttribute("name"));
                enemyAttackValues.Add(Convert.ToInt16(eReader.GetAttribute("value")));
            }

            //remove the last thing in both lists
            enemyAttacks.RemoveAt(enemyAttacks.Count() - 1);
            enemyAttackValues.RemoveAt(enemyAttackValues.Count() - 1);

            //stop the reader
            eReader.Close();
        }
        #endregion attack type method (filling in the possible attacks)
        #region enemy attacks
        private void EnemyAttacks(int timer)
        {
            //check which attack was randomly selected and do it
            if (attackName == "HornAttack")
            {
                //set attack variables correctly
                if (attackVariablesSet == false)
                {
                    spaceBetweenAttacks = 40;
                    attackSpeed = 5;
                    hornLeft = true;
                    hornSpaceChange = false;
                    attackVariablesSet = true;
                }

                //if the correct amount of time has passed, and the enemy turn isn't over, spawn a new horn attack
                if (timer % spaceBetweenAttacks == 0 && timer != 0)
                {
                    //alternate between left and right attacks
                    if (hornLeft == true)
                    {
                        Projectile hornProjL = new Projectile(arenaWalls[0].X + 5, arenaWalls[3].Y, (arenaWalls[1].X - arenaWalls[0].X) / 2 + 10, 100, Resources.attackHornO);
                        attacks.Add(hornProjL);
                        hornLeft = false;
                    }
                    else
                    {
                        Projectile hornProjR = new Projectile(arenaWalls[1].X - (arenaWalls[1].X - arenaWalls[0].X) / 2 - 10, arenaWalls[3].Y, (arenaWalls[1].X - arenaWalls[0].X) / 2 + 10, 100, Resources.attackHorn);
                        attacks.Add(hornProjR);
                        hornLeft = true;

                        //make the attack get more difficult as time goes on
                        if (hornSpaceChange == true && spaceBetweenAttacks >= 30)
                        {
                            attackSpeed += 1;
                            spaceBetweenAttacks -= 10;
                            hornSpaceChange = false;
                        }
                        else
                        {
                            hornSpaceChange = true;
                        }
                    }
                }
                //reset attack variables
                else if (timer == 1)
                {
                    attackVariablesSet = false;
                }

                //clear the attack rec list
                attackRecs.Clear();

                //for each projectile, create a rec for collisions
                foreach (Projectile p in attacks)
                {
                    Rectangle horn = new Rectangle(p.x, p.y + 20, p.width, p.height - 40);
                    attackRecs.Add(horn);
                }

                //move the projectiles according to the attack and get rid of the first one if it goes out of the arena box
                foreach (Projectile p in attacks)
                {
                    p.HornAttack(attackSpeed);

                    if (p.y <= arenaWalls[2].Y - 50)
                    {
                        attacks.Remove(p);
                        break;
                    }
                }
            }
            if (attackName == "HoofAttack")
            {
                //set attack variables correctly
                if (attackVariablesSet == false)
                {
                    spaceBetweenAttacks = 75;
                    attackSpeed = 10;
                    attackVariablesSet = true;
                }

                //if the correct amount of time has passed, and the enemy turn isn't over, spawn a new hoof attack pattern
                if (timer % spaceBetweenAttacks == 0 && timer != 0)
                {
                    //set the pause timer
                    attackPauseTimer = timer - 25;

                    //initialize a hoof attack in each of the 4 sections of the arena
                    for (int i = 0; i < 4; i++)
                    {
                        Projectile hoof = new Projectile(arenaWalls[0].X + ((arenaWalls[1].X - arenaWalls[0].X) / 4) * i, arenaWalls[2].Y - 200, 100, 200, Resources.attackHoof);
                        Rectangle hoofRec = new Rectangle(hoof.x, hoof.y, hoof.width, hoof.height);
                        attacks.Add(hoof);
                        attackRecs.Add(hoofRec);
                    }

                    //generate a random number(0, 1, 2, or 3)
                    int x = randNum.Next(0, 4);

                    //remove the hoof at the index of the random number
                    attacks.RemoveAt(x);
                    attackRecs.RemoveAt(x);
                }
                //reset attack variables
                else if (timer == 1)
                {
                    attackVariablesSet = false;
                }

                //clear the attack rec list
                attackRecs.Clear();

                //for each projectile, create a rec for collisions
                foreach (Projectile p in attacks)
                {
                    Rectangle horn = new Rectangle(p.x, p.y, p.width, p.height);
                    attackRecs.Add(horn);
                }

                //move the projectiles according to the attack
                foreach (Projectile p in attacks)
                {
                    if (timer < attackPauseTimer)
                    {
                        p.HoofAttack(attackSpeed, arenaWalls[2].Y);
                    }
                }

                //once the attacks reach the bottom of the arena, remove them after half a second
                try
                {
                    if (attacks[0].y >= arenaWalls[2].Y)
                    {
                        attacks.Clear();
                    }
                }
                catch (Exception) { }
            }
            if (attackName == "FistAttack")
            {
                //set attack variables correctly
                if (attackVariablesSet == false)
                {
                    spaceBetweenAttacks = 20;
                    attackVariablesSet = true;
                }

                //if the correct amount of time has passed, and the enemy turn isn't over, spawn a new fist attack
                if (timer % spaceBetweenAttacks == 0 && timer != 0)
                {
                    //if there is attacks on screen, set the oldest attack to actually do damage
                    if (attacks.Count > 0)
                    {
                        //remove the oldest attack if there are 3 on screen
                        if (attacks.Count == 3)
                        {
                            attacks.RemoveAt(0);
                            attackRecs.RemoveAt(0);
                        }

                        //create a rec for the first attack
                        if (attacks.Count() == 1)
                        {
                            Rectangle fistRec = new Rectangle(attacks[0].x + 16, attacks[0].y + 16, attacks[0].width - 32, attacks[0].height - 32);
                            attackRecs.Add(fistRec);
                        }

                        //create a rec for the second attack
                        if (attacks.Count() == 2)
                        {
                            Rectangle fistRec = new Rectangle(attacks[1].x + 16, attacks[1].y + 16, attacks[1].width - 32, attacks[1].height - 32);
                            attackRecs.Add(fistRec);
                        }

                        //update the sprite of each attack that will do daamage
                        for (int i = 0; i < attackRecs.Count(); i++)
                        {
                            attacks[i].image = Resources.attackFistL;
                        }
                    }

                    //generate a random x and y coordinate for the next fist attack
                    int x = randNum.Next(arenaWalls[0].X, arenaWalls[1].X - 80);
                    int y = randNum.Next(arenaWalls[2].Y, arenaWalls[3].Y - 80);

                    //create a projectile for the fist and set it's image to the dark version (aka it won't do damage yet)
                    Projectile fist = new Projectile(x, y, 150, 105, Resources.attackFistD);
                    attacks.Add(fist);
                }
                //reset attack variables
                else if (timer == 1)
                {
                    attackVariablesSet = false;
                }
            }
            if (attackName == "LeafAttack")
            {
                //set attack variables correctly
                if (attackVariablesSet == false)
                {
                    spaceBetweenAttacks = 25;
                    attackSpeed = 4;
                    leavesLeft = true;
                    attackVariablesSet = true;
                }

                //if the correct amount of time has passed, and the enemy turn isn't over, spawn a new hoof attack pattern
                if (timer % spaceBetweenAttacks == 0 && timer != 0)
                {
                    if (leavesLeft == true)
                    {
                        //create 2 random leaf attacks
                        for (int i = 0; i < 2; i++)
                        {
                            int x = randNum.Next(arenaWalls[0].X, arenaWalls[1].X);

                            Projectile leaf = new Projectile(x, arenaWalls[2].Y - 50, 76, 50, Resources.attackLeaves);
                            attacks.Add(leaf);
                        }

                        leavesLeft = false;
                    }
                    else
                    {
                        //create 2 random leaf attacks
                        for (int i = 0; i < 2; i++)
                        {
                            int x = randNum.Next(arenaWalls[0].X, arenaWalls[1].X);

                            Projectile leaf = new Projectile(x, arenaWalls[2].Y - 50, 76, 50, Resources.attackLeavesO);
                            attacks.Add(leaf);
                        }

                        leavesLeft = true;
                    }
                }
                //reset attack variables
                else if (timer == 1)
                {
                    attackVariablesSet = false;
                }

                //clear the attack rec list
                attackRecs.Clear();

                //for each projectile, create a rec for collisions
                foreach (Projectile p in attacks)
                {
                    Rectangle leafRec = new Rectangle(p.x + 10, p.y + 10, p.width - 10, p.height - 10);
                    attackRecs.Add(leafRec);
                }


                if (leavesLeft == true)
                {
                    //move the projectiles according to the attack
                    foreach (Projectile p in attacks)
                    {
                        p.LeafAttack(attackSpeed, leavesLeft);
                    }
                }
                else
                {
                    //move the projectiles according to the attack
                    foreach (Projectile p in attacks)
                    {
                        p.LeafAttack(attackSpeed, leavesLeft);
                    }
                }

                //once a wave of attacks reach the bottom of the arena, remove them
                if (attacks[0].y >= arenaWalls[3].Y - 20)
                {
                    attacks.RemoveRange(0, 2);
                }
            }
        }
        #endregion enemy attacks

        #endregion enemy turn methods

        #region player xml update method (items)
        private void PlayerXmlUpdate(int i)
        {
            //open the player xml file and place it in doc
            XmlDocument doc = new XmlDocument();
            doc.Load("Resources/Player.xml");

            //create a list of all nodes called "Item"
            XmlNodeList itemList = doc.GetElementsByTagName("Item");

            //search each Item node in the list until the text matches the item used
            //then change it to nothing to get rid of the item
            foreach (XmlNode n in itemList)
            {
                if (n.Attributes[0].InnerText == actNames[i])
                {
                    n.Attributes[0].InnerText = " ";
                    n.Attributes[1].InnerText = "0";
                }
            }

            //int for counting
            i = 0;

            //search each Item node in the list until the text is empty (aka item is empty)
            //then change it to the next item's info and change the next item's info to be empty
            //this should move all items back one item in the xml file
            foreach (XmlNode n in itemList)
            {
                if (n.Attributes[0].InnerText == " ")
                {
                    if (i < 3)
                    {
                        n.Attributes[0].InnerText = itemList[i + 1].Attributes[0].InnerText;
                        n.Attributes[1].InnerText = itemList[i + 1].Attributes[1].InnerText;

                        itemList[i + 1].Attributes[0].InnerText = " ";
                        itemList[i + 1].Attributes[1].InnerText = "0";
                    }
                    else
                    {
                        n.Attributes[0].InnerText = " ";
                        n.Attributes[1].InnerText = "0";
                    }
                }

                i++;
            }

            //save and close the player xml
            doc.Save("Resources/Player.xml");
        }
        #endregion player xml update method (items)

        #region player xml save update method
        private void playerSaveUpdate()
        {
            //open the player xml file and place it in doc
            XmlDocument doc = new XmlDocument();
            doc.Load("Resources/Player.xml");

            //create a nodelist for "Save" nodes and "General" nodes
            XmlNodeList saveInfo = doc.GetElementsByTagName("Save");
            XmlNodeList gold = doc.GetElementsByTagName("General");

            //update the fight outcome depending on the enemy fought
            if (TownScreen.enemyName == "Calum")
            {
                saveInfo[0].Attributes[0].InnerText = enemySpared;
            }
            if (TownScreen.enemyName == "Franky")
            {
                saveInfo[0].Attributes[1].InnerText = enemySpared;
            }


            if (enemySpared == "killed")
            {
                //update the player gold amount
                gold[0].Attributes[0].InnerText = Convert.ToInt16(gold[0].Attributes[0].InnerText) + enemy.gold + "";
            }

            //save and close the player xml
            doc.Save("Resources/Player.xml");
        }
        #endregion player xml save update method
    }
}