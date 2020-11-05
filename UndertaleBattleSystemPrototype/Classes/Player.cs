using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndertaleBattleSystemPrototype
{
    class Player
    {
        public int x, y, size, hp, atk;
        
        public Player()
        {
        }

        public Player(int _x, int _y, int _size, int _hp, int _atk)
        {
            x = _x;
            y = _y;
            size = _size;
            hp = _hp;
            atk = _atk;
        }

        public void MoveUpDown(int speed)
        {
            y += speed;
        }

        public void MoveLeftRight(int speed)
        {
            x += speed;
        }

        public void Stop(string direction, int speed)
        {
            switch (direction)
            {
                case "up":
                    y += speed;
                    break;
                case "left":
                    x += speed;
                    break;
                case "down":
                    y -= speed;
                    break;
                case "right":
                    x -= speed;
                    break;
            }
        }

        public void AttackCollision(int enemyDamage, Boolean invincible)
        {
            if (invincible == false)
            {
                hp -= enemyDamage;
                BattleScreen.playerInvincible = true;
            }
        }
    }
}
