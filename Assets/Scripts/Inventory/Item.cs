using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SellBro.DungeonCrawler.Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Add Item/Item", order = 1)]
    public class Item : ScriptableObject
    {
        public string name;
        [TextArea(10, 15)] public string description;

        public int damage;
        public int armour;
        public int amount = 1;

        public bool isStackable = false;
        public ItemType itemType = ItemType.Usable;
        public EquippableItemType equippableItemType = EquippableItemType.None;

        public Sprite sprite;
        
    }
    
    public enum ItemType
    {
        Equippable,
        Usable
    }

    public enum EquippableItemType
    {
        None,
        Head,
        Body,
        Legs,
        LHand,
        RHand,
        Ring,
        Amulet
    }
}


    
