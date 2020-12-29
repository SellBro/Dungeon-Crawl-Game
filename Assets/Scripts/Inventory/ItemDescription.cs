using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SellBro.DungeonCrawler.Inventory
{
    public class ItemDescription : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI description;
        public TextMeshProUGUI buttonText;
        public ItemData descriptionData;
        
        private Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void SetDescription(ItemData data)
        {
            if (anim != null)
            {
                OpenUI();
            }
            
            if (buttonText != null)
            {
                buttonText.transform.parent.parent.gameObject.SetActive(true);
            }
            descriptionData = data;

            if (image != null)
            {
                image.sprite = data.item.sprite;
            }
            itemName.text = data.item.name;
            description.text = data.item.description;

            if (buttonText == null) return;
            
            if (data.item.itemType == ItemType.Equippable)
            {
                if (data.isEquipped)
                {
                    buttonText.text = "Unequip";
                }
                else
                {
                    buttonText.text = "Equip";
                }
                
            }
            else if (data.item.itemType == ItemType.Usable)
            {
                buttonText.text = "Use";
            }
            else
            {
                buttonText.transform.parent.parent.gameObject.SetActive(false);
            }
        }

        public void OpenUI()
        {
            anim.SetTrigger("Open");
        }

        public void CloseUI()
        {
            anim.SetTrigger("Close");
        }
    }
}
