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

        public void SetDescription(ItemData data)
        {
            buttonText.transform.parent.parent.gameObject.SetActive(true);
            
            image.sprite = data.item.sprite;
            itemName.text = data.item.name;
            description.text = data.item.description;

            if (data.item.itemType == ItemType.Equippable)
            {
                buttonText.text = "Equip";
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
    }
}
