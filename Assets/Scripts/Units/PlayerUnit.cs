using SellBro.Player;
using UnityEngine;

namespace SellBro.Units
{
    public class PlayerUnit : Unit
    {
        [Header("Player Unit Settings")]
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
