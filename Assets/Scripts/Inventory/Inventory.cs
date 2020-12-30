using System.Collections.Generic;
using SellBro.Core;
using SellBro.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SellBro.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public bool isPlayerInv;
        
        [Header("Spawning Objects")]
        public GameObject slotPanel;
        public GameObject inventoryItem;
        public ItemDescription descriptionPanel; 
        public GameObject inventorySlot;
        
        [Header("Test Items")]
        public Item pepe;
        public Item empty;
        
        [Header("List of items")]
        public List<Item> items = new List<Item>();
        public List<GameObject> slots = new List<GameObject>();
        
        [Header("Inventory Settings")]
        public int invenotySize = 20;
        public int inventoryOffset = 7;



        private void Start()
        {
            for (int i = 0; i < inventoryOffset; i++)
            {
                items.Add(empty);
                Slot s = slots[i].GetComponent<Slot>();
                s.id = i;
                s.inventory = this;
            }
            for (int i = inventoryOffset; i < invenotySize + inventoryOffset; i++)
            {
                items.Add(empty);
                
                slots.Add(Instantiate(inventorySlot));
                slots[i].transform.SetParent(slotPanel.transform);
                slots[i].GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
                
                Slot s = slots[i].GetComponent<Slot>();
                s.id = i;
                s.inventory = this;
            }
            AddItem(pepe);
        }

        public void AddItem(Item item)
        {
            if (item.isStackable)
            {
                for (int i = inventoryOffset; i < items.Count; i++)
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

            for (int i = inventoryOffset; i < items.Count; i++)
            {
                if (items[i] == empty)
                {
                    Debug.Log("AddItem");
                    // Add items to the list
                    items[i] = item;

                    // Create item GameObject
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = item.sprite;
                    itemObj.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
                    itemObj.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
                    
                    // Create ItemData to store item
                    ItemData itemData = itemObj.GetComponent<ItemData>();
                    itemData.inventory = this;
                    itemData.amount = 1;
                    itemData.item = item;
                    itemData.slot = i;
                    if (isPlayerInv)
                    {
                        itemData.isLoot = false;
                    }
                    
                    break;
                }
            }
        }

        public void OpenDescription(ItemData data)
        {
            if(descriptionPanel == null) return;
            
            descriptionPanel.SetDescription(data);
            descriptionPanel.gameObject.SetActive(true);
        }

        public void UseButton()
        {
            if (descriptionPanel.descriptionData.item.itemType == ItemType.Equippable)
            {
                if (descriptionPanel.descriptionData.isEquipped)
                {
                    for (int i = inventoryOffset; i < items.Count; i++)
                    {
                        if (items[i] == empty)
                        {
                            Debug.Log("Take Off");
                            Slot slot = slots[i].GetComponent<Slot>();
                            slot.EquipItem(descriptionPanel.descriptionData);
                            descriptionPanel.descriptionData.MoveItem();
                            descriptionPanel.gameObject.SetActive(false);
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < inventoryOffset; i++)
                    {
                        Slot slot = slots[i].GetComponent<Slot>();
                        if (slot.slotType == descriptionPanel.descriptionData.item.equippableItemType)
                        {
                            Debug.Log("Equip");
                            slot.EquipItem(descriptionPanel.descriptionData);
                            descriptionPanel.descriptionData.MoveItem();
                            descriptionPanel.gameObject.SetActive(false);
                            break;
                        }
                    }
                }
            }
            else if (descriptionPanel.descriptionData.item.itemType == ItemType.Usable)
            {
                
            }
        }

        public void TakeAll()
        {
            Inventory playerInv = GameManager.Instance.player.GetComponent<PlayerController>().GetPlayerInventory();

            for (int i = 0; i < items.Count;i++)
            {
                if (items[i] != empty)
                {
                    playerInv.AddItem(items[i]);
                    RemoveItems(i);
                }
            }
        }

        private void RemoveItems(int index)
        {
            items[index] = empty;
            Destroy(slots[index].transform.GetChild(0).gameObject);
        }

        public void TakeItem(Item item)
        {
            Inventory playerInv = GameManager.Instance.player.GetComponent<PlayerController>().GetPlayerInventory();
            playerInv.AddItem(item);
            int index = items.IndexOf(item);
            RemoveItems(index);
        }
    }
}
