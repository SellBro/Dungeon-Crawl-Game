using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SellBro.DungeonCrawler.Inventory
{
    public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public Item item;
        public int amount = 0;
        public int slot;

        public bool isEquipped = false;
        public bool isLoot = true;

        [HideInInspector]
        public Inventory inventory;
        
        private Transform originalParent;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (item != inventory.empty)
            {
                originalParent = transform.parent;
                transform.SetParent(inventory.transform);
                transform.position = eventData.position;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (item != inventory.empty)
            {
                transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            MoveItem();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isLoot)
            {
                inventory.OpenDescription(this);
                return;
            }
            
            inventory.TakeItem(item);
        }

        public void MoveItem()
        {
            transform.SetParent(inventory.slots[slot].transform);
            transform.position = inventory.slots[slot].transform.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
