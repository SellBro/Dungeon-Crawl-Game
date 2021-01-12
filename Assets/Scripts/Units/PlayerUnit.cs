using System;
using SellBro.Player;
using TMPro;
using UnityEngine;

namespace SellBro.Units
{
    public class PlayerUnit : Unit
    {
        [Header("Player Unit Settings")]
        public int level = 1;
        public int xP;
        public int xPToNextLevel = 100;
        public int additionalDamage = 0;
        public int additionalHealth = 0;
        public int additionalArmour = 0;

        [Header("UI Objects")] 
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI xpText;

        private PlayerController _controller;

        protected override void Start()
        {
            base.Start();
            
            healthText.text = "Health - " + _health + "/" + maxHealth;
            xpText.text = "Level - " + level + " \nXP -" + xP + "/" + xPToNextLevel;
        }

        private void Update()
        {
            
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);

            healthText.text = "Health - " + _health + "/" + maxHealth;
        }

        public void AddXP(int amount)
        {
            xP += amount;
            if (xP >= xPToNextLevel)
            {
                int temp = xP - xPToNextLevel;
                ++level;
                xP = temp;
                xPToNextLevel += 100;
            }
            
            xpText.text = "Level - " + level + " \nXP - " + xP + "/" + xPToNextLevel;
        }

        public override int GetDamage()
        {
            return damage + additionalDamage;
        }
    }
}
