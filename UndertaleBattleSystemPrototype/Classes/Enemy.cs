﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndertaleBattleSystemPrototype.Classes
{
    class Enemy
    {
        public int hp, atk, def, gold;

        public Enemy()
        {
        }

        public Enemy (int _hp, int _atk, int _def, int _gold)
        {
            hp = _hp;
            atk = _atk;
            def = _def;
            gold = _gold;
        }
    }
}
