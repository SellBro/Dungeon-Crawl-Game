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
        
        private Inventory _inventory;

        private void Start()
        {
            _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();


            EquipItem(droppedItem);
        }

        public void EquipItem(ItemData droppedItem)
        {
            if (_inventory.items[id] == _inventory.empty && (droppedItem.item.equippableItemType == slotType || slotType == EquippableItemType.None))
            {
                _inventory.items[droppedItem.slot] = _inventory.empty;
                _inventory.items[id] = droppedItem.item;
                droppedItem.slot = id;
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
                item.transform.SetParent(_inventory.slots[droppedItem.slot].transform);
                item.transform.position = _inventory.slots[droppedItem.slot].transform.position;
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

                _inventory.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
                _inventory.items[id] = droppedItem.item;
            }
            
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
