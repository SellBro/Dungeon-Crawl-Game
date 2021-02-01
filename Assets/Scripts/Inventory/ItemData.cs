using UnityEngine;
using UnityEngine.EventSystems;

namespace SellBro.Inventory
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

        #region Interface Implementations

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (item != inventory.empty)
            {
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
            
            EquippedSlot.DisplayImage.Invoke();
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

        #endregion
        

        public void MoveItem()
        {
            transform.SetParent(inventory.slots[slot].transform);
            transform.position = inventory.slots[slot].transform.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
