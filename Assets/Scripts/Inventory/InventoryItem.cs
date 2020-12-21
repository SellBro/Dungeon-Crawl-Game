using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "Inventory Item", menuName = "Item", order = 1)]
    public class InventoryItem : ScriptableObject
    {
        [Header("UI")]
        public Sprite sprite;
        
        [Header("Stats")]
        public int additionalDamage = 0;
        public int additionalHealth = 0;
        public int additionalArmour = 0;
        [TextArea(10,5)]
        public string description = "";

        public void EquipItem()
        {
            
        }
        
        public void UnEquipItem()
        {
            
        }
    }
}
