using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Player;
using RPG.Units;
using UnityEngine;

namespace RPG.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        public InventoryItem rightHand;
        public InventoryItem leftHand;
        
        public InventoryItem head;
        public InventoryItem body;
        public InventoryItem legs;
        public InventoryItem amulet;
        public InventoryItem leftRing;
        public InventoryItem rightRing;

        public InventoryItem[] inventoryItems;

        private PlayerUnit _unit;
        private int _additionalDamage = 0;
        private int _additionalHealth = 0;
        private int _additionalArmour = 0;
        
        public void Start()
        {
            _unit = GetComponent<PlayerUnit>();
            
            _unit.additionalDamage = rightHand.additionalDamage + leftHand.additionalDamage;
        }

        private int CalculateAdditionalDamage()
        {
            return rightHand.additionalDamage + leftHand.additionalDamage + head.additionalDamage +
                   body.additionalDamage + legs.additionalDamage + amulet.additionalDamage + leftRing.additionalDamage +
                   rightRing.additionalDamage;
        }
        
        private int CalculateAdditionalArmour()
        {
            return rightHand.additionalArmour + leftHand.additionalArmour + head.additionalArmour +
                   body.additionalArmour + legs.additionalArmour + amulet.additionalArmour + leftRing.additionalArmour +
                   rightRing.additionalArmour;
        }
        
        private int CalculateAdditionalHealth()
        {
            return rightHand.additionalHealth + leftHand.additionalHealth + head.additionalHealth +
                   body.additionalHealth + legs.additionalHealth + amulet.additionalHealth + leftRing.additionalHealth +
                   rightRing.additionalHealth;
        }
    }
}
