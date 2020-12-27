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

        private Transform originalParent;
        private Inventory _inventory;

        private void Start()
        {
            _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (item !=  _inventory.empty)
            {
                originalParent = transform.parent;
                transform.SetParent(_inventory.transform);
                transform.position = eventData.position;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (item != _inventory.empty)
            {
                transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(_inventory.slots[slot].transform);
            transform.position = _inventory.slots[slot].transform.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _inventory.OpenDescription(this);
        }
    }
}
