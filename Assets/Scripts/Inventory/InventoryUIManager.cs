using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventory
{
    public class InventoryUIManager : MonoBehaviour
    {
        public InventoryUICell rightHand = null;
        public InventoryUICell leftHand = null;
        
        public InventoryUICell head = null;
        public InventoryUICell body = null;
        public InventoryUICell legs = null;
        public InventoryUICell amulet = null;
        public InventoryUICell leftRing = null;
        public InventoryUICell rightRing = null;
        
        public InventoryUICell[] inventoryItems;
        

        public void AddItem(InventoryItem item)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].item == null)
                {
                    inventoryItems[i].item = item;
                    inventoryItems[i].SetImage(item.sprite);
                    break;
                }
            }
        }
        
        public void UpdateUI(InventoryItem item, InventoryItemType type, bool right = true)
        {
            switch (type)
            {
                case InventoryItemType.Head:
                    head.item = item;
                    head.SetImage(item.sprite);
                    break;
                case InventoryItemType.Body:
                    body.item = item;
                    body.SetImage(item.sprite);
                    break;
                case InventoryItemType.Hand:
                    if (right)
                    {
                        rightHand.item = item;
                        rightHand.SetImage(item.sprite);
                        break;
                    }
                    else
                    {
                        leftHand.item = item;
                        leftHand.SetImage(item.sprite);
                        break;
                    }
                case InventoryItemType.Legs:
                    legs.item = item;
                    legs.SetImage(item.sprite);
                    break;
                case InventoryItemType.Ring:
                    if (right)
                    {
                        rightRing.item = item;
                        rightRing.SetImage(item.sprite);
                        break;
                    }
                    else
                    {
                        leftRing.item = item;
                        leftRing.SetImage(item.sprite);
                        break;
                    }
                case InventoryItemType.Amulet:
                    amulet.item = item;
                    amulet.SetImage(item.sprite);
                    break;
            }
        }
    }
}
