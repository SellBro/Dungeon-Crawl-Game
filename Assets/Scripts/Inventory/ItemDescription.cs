using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SellBro.Inventory
{
    public class ItemDescription : MonoBehaviour
    {
        public ItemData descriptionData;
        
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI buttonText;

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
            
            descriptionData = data;

            if (image != null)
            {
                image.sprite = data.item.sprite;
            }
            itemName.text = data.item.name;
            description.text = data.item.description;

            if (buttonText == null) return;
            
            buttonText.transform.parent.parent.gameObject.SetActive(true);
            
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

        private void OpenUI()
        {
            anim.SetTrigger("Open");
        }
        
        private void CloseUI()
        {
            anim.SetTrigger("Close");
        }
    }
}
