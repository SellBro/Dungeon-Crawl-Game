using SellBro.Inventory;
using SellBro.Items;
using UnityEngine;


namespace SellBro.Player
{
    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField] private Inventory.Inventory inventory;

        private void Start()
        {
            inventory.isPlayerInv = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Item[] items = RandomItemGenerator.Instance.GenerateStartingLoot();
                for (int i = 0; i < items.Length; i++)
                {
                    Debug.Log("Item:" + items[i].name);
                    inventory.AddItem(items[i]);
                }
            }
            
            OpenInventory();
        }
        
        private void OpenInventory()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (inventory.GetComponent<CanvasGroup>().alpha == 1)
                {
                    inventory.GetComponent<CanvasGroup>().alpha = 0;
                    inventory.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
                else
                {
                    inventory.GetComponent<CanvasGroup>().alpha = 1;
                    inventory.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }
        }
        
        public Inventory.Inventory GetPlayerInventory()
        {
            return inventory;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Item"))
            {
                inventory.AddItem(other.gameObject.GetComponent<ItemPickup>().item);
            }
        }
    }
}
