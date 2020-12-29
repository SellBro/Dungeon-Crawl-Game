using UnityEngine;

namespace SellBro.Inventory
{
    public class EquippedSlot : MonoBehaviour
    {
        [SerializeField] private GameObject imageObj;
        
        private void FixedUpdate()
        {
            if (transform.childCount > 1)
            {
                imageObj.SetActive(false);
            }
            else
            {
                imageObj.SetActive(true);
            }
        }
    }
}
