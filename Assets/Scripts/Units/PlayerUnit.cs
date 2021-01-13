using System;
using SellBro.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Image hpBar;

        private PlayerController _controller;

        protected override void Start()
        {
            base.Start();

            hpBar.fillAmount = _health / maxHealth;
            
            healthText.text = "Health - " + _health + "/" + maxHealth;
            xpText.text = "Level - " + level + " \nXP -" + xP + "/" + xPToNextLevel;
        }

        private void Update()
        {
            hpBar.fillAmount = _health / maxHealth;
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
