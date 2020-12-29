using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SellBro.DungeonCrawler.Inventory
{
    public class Slot : MonoBehaviour, IDropHandler
    {
        public int id;
        public EquippableItemType slotType = EquippableItemType.None;
        
        [HideInInspector]
        public Inventory inventory;

        public void OnDrop(PointerEventData eventData)
        {
            ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();


            EquipItem(droppedItem);
        }

        public void EquipItem(ItemData droppedItem)
        {
            bool wasEquipped = false;
            
            if (inventory.items[id] == inventory.empty && (droppedItem.item.equippableItemType == slotType || slotType == EquippableItemType.None))
            {
                inventory.items[droppedItem.slot] = inventory.empty;
                inventory.items[id] = droppedItem.item;
                droppedItem.slot = id;
                wasEquipped = true;
            }
            else if(droppedItem.item.equippableItemType == slotType || slotType == EquippableItemType.None)
            {
                Debug.Log("Swap");

                Transform item;
                if (transform.childCount > 1)
                {
                    item = transform.GetChild(1);
                }
                else
                {
                    item = transform.GetChild(0);
                }
                item.GetComponent<ItemData>().slot = droppedItem.slot;
                item.transform.SetParent(inventory.slots[droppedItem.slot].transform);
                item.transform.position = inventory.slots[droppedItem.slot].transform.position;
                if (droppedItem.slot < 7)
                {
                    item.GetComponent<ItemData>().isEquipped = true;
                }
                else
                {
                    item.GetComponent<ItemData>().isEquipped = false;
                }

                droppedItem.slot = id;
                droppedItem.transform.SetParent(transform);
                droppedItem.transform.position = transform.position;

                inventory.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
                inventory.items[id] = droppedItem.item;

                wasEquipped = true;
            }
            
            if(!wasEquipped) return;
            
            if (id < 7)
            {
                droppedItem.isEquipped = true;
            }
            else
            {
                droppedItem.isEquipped = false;
            }
        }
    }
}
