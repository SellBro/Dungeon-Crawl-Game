using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SellBro.DungeonCrawler.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public GameObject slotPanel;
        public GameObject inventoryItem;
        public ItemDescription descriptionPanel;

        public Item pepe;

        public Item empty;
        
        public List<Item> items = new List<Item>();
        public List<Item> equippedItems = new List<Item>();
        public List<GameObject> slots = new List<GameObject>();
        public List<GameObject> equippableSlots = new List<GameObject>();
        public GameObject inventorySlot;


        private static readonly int InvenotySize = 20; 
        
        

        private void Start()
        {
            for (int i = 0; i < InvenotySize; i++)
            {
                items.Add(empty);
                slots.Add(Instantiate(inventorySlot));
                slots[i].transform.SetParent(slotPanel.transform);
                slots[i].GetComponent<Slot>().id = i;
                slots[i].GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            }
            
            AddItem(pepe);
            AddItem(pepe);
        }

        public void AddItem(Item item)
        {
            if (item.isStackable)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].name == item.name)
                    {
                        ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                        data.amount += item.amount;
                        data.transform.GetChild(0).gameObject.SetActive(true);
                        data.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.amount.ToString();
                        return;
                    }
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == empty)
                {
                    items[i] = item;
                    
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = Vector2.zero;
                    ItemData itemData = itemObj.GetComponent<ItemData>();
                    itemData.amount = 1;
                    itemData.item = item;
                    itemData.slot = i;
                    itemObj.GetComponent<Image>().sprite = item.sprite;
                    itemObj.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
                    break;
                }
            }
        }

        public void OpenDescription(ItemData data)
        {
            descriptionPanel.SetDescription(data);
            descriptionPanel.gameObject.SetActive(true);
        }
    }
}
