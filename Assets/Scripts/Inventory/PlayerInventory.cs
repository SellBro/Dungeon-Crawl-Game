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
        [Range(1,20)]
        public int inventorySize = 3;
        public InventoryUIManager inventoryManager;
        
        public InventoryItem rightHand;
        public InventoryItem leftHand;
        
        public InventoryItem head;
        public InventoryItem body;
        public InventoryItem legs;
        public InventoryItem amulet;
        public InventoryItem leftRing;
        public InventoryItem rightRing;

        public List<InventoryItem> inventoryItems;

        private PlayerUnit _unit;
        private int _additionalDamage = 0;
        private int _additionalHealth = 0;
        private int _additionalArmour = 0;
        
        public void Start()
        {
            //_unit = GetComponent<PlayerUnit>();
            
            //_unit.additionalDamage = rightHand.additionalDamage + leftHand.additionalDamage;
        }

        public bool AddItem(InventoryItem item, int count)
        {
            item.count = count;
            if (inventoryItems.Count > inventorySize)
            {
                return false;
            }
            
            inventoryItems.Add(item);
            inventoryManager.AddItem(item);
            return true;
        }

        public bool EquipItem(InventoryItem item, InventoryItemType type, bool right = true)
        {
            switch (type)
            {
                case InventoryItemType.Head:
                    head = item;
                    item.isEquipped = true;
                    head.isEquipped = false;
                    inventoryManager.UpdateUI(item,type,right);
                    return true;
                case InventoryItemType.Body:
                    body = item;
                    item.isEquipped = true;
                    body.isEquipped = false;
                    inventoryManager.UpdateUI(item,type,right);
                    return true;
                case InventoryItemType.Hand:
                    if (right)
                    {
                        rightHand = item;
                        item.isEquipped = true;
                        rightHand.isEquipped = false;
                        inventoryManager.UpdateUI(item,type,right);
                        return true;
                    }
                    else
                    {
                        leftHand = item;
                        item.isEquipped = true;
                        leftHand.isEquipped = false;
                        inventoryManager.UpdateUI(item,type,right);
                        return true;
                    }
                case InventoryItemType.Legs:
                    legs = item;
                    item.isEquipped = true;
                    legs.isEquipped = false;
                    inventoryManager.UpdateUI(item,type,right);
                    return true;
                case InventoryItemType.Ring:
                    if (right)
                    {
                        rightRing = item;
                        item.isEquipped = true;
                        rightRing.isEquipped = false;
                        inventoryManager.UpdateUI(item,type,right);
                        return true;
                    }
                    else
                    {
                        leftRing = item;
                        item.isEquipped = true;
                        leftRing.isEquipped = false;
                        inventoryManager.UpdateUI(item,type,right);
                        return true;
                    }
                case InventoryItemType.Amulet:
                    amulet = item;
                    item.isEquipped = true;
                    amulet.isEquipped = false;
                    inventoryManager.UpdateUI(item,type,right);
                    return true;
            }

            return false;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Item"))
            {
                InventoryItem item = other.GetComponent<GameSceneItem>().GetInventoryItem();
                AddItem(item,1);
                EquipItem(item, item.itemType, true);
                Destroy(other.gameObject);
            }
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
