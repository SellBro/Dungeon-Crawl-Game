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
        public string itemName = "";
        public InventoryItemType itemType;
        public bool isEquipped = false;
        public int count = 0;
        public int additionalDamage = 0;
        public int additionalHealth = 0;
        public int additionalArmour = 0;
        [TextArea(10,5)]
        public string description = "";
    }

    public enum InventoryItemType
    {
        Head,
        Body,
        Hand,
        Legs,
        Ring,
        Amulet,
        Usable
    }
}
