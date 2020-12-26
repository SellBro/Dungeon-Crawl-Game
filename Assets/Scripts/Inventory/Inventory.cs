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

        public Item pepe;
        public Item poggers;
        
        public List<Item> items = new List<Item>();
        public List<GameObject> slots = new List<GameObject>();
        public GameObject inventorySlot;

        private static readonly int InvenotySize = 20; 
        
        

        private void Start()
        {
            for (int i = 0; i < InvenotySize; i++)
            {
                slots.Add(Instantiate(inventorySlot));
                slots[i].transform.SetParent(slotPanel.transform);
                slots[i].GetComponent<Slot>().id = i;
                slots[i].GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            }
            
            AddItem(pepe);
            AddItem(pepe);
            AddItem(poggers);
            AddItem(poggers);
            AddItem(poggers);
            AddItem(poggers);
            AddItem(poggers);
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
            
            items.Add(item);
            GameObject itemObj = Instantiate(inventoryItem);
            itemObj.transform.SetParent(slots[items.Count-1].transform);
            itemObj.transform.position = Vector2.zero;
            ItemData itemData = itemObj.GetComponent<ItemData>();
            itemData.amount = 1;
            itemData.item = item;
            itemData.slot = items.Count - 1;
            itemObj.GetComponent<Image>().sprite = item.sprite;
            itemObj.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        }
    }
}
