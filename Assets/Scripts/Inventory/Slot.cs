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
        
        private Inventory _inventory;

        private void Start()
        {
            _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();

            if (_inventory.items[id] != null)
            {
                Transform item = transform.GetChild(0);
                item.GetComponent<ItemData>().slot = droppedItem.slot;
                item.transform.SetParent(_inventory.slots[droppedItem.slot].transform);
                item.transform.position = _inventory.slots[droppedItem.slot].transform.position;

                droppedItem.slot = id;
                droppedItem.transform.SetParent(transform);
                droppedItem.transform.position = transform.position;

                _inventory.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
                _inventory.items[id] = droppedItem.item;
            }
        }
    }
}
