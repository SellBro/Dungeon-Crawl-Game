using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SellBro.DungeonCrawler.Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Add Item/Item", order = 1)]
    public class Item : ScriptableObject
    {
        public string name;
        [TextArea(10,15)]
        public string description;
        
        public int damage;
        public int armour;
        public int amount = 1;

        public bool isStackable = false;
        public bool isEquippable = false;

        public Sprite sprite;
    }
}
