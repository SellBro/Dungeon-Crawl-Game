using System.Collections;
using System.Collections.Generic;
using RPG.Player;
using UnityEngine;

namespace RPG.Units
{
    public class PlayerUnit : Unit
    {

        public int level;
        public int xP;
        public int additionalDamage = 0;
        public int additionalHealth = 0;
        public int additionalArmour = 0;
        
        
        private PlayerController _controller;

        public override int GetDamage()
        {
            return damage + additionalDamage;
        }
    }
}
