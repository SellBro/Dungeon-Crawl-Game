using System;
using UnityEngine;

namespace SellBro.Inventory
{
    public class EquippedSlot : MonoBehaviour
    {
        [SerializeField] private GameObject imageObj;

        public static Action DisplayImage;

        private void OnEnable()
        {
            DisplayImage += ShowImage;
        }

        private void OnDisable()
        {
            DisplayImage -= ShowImage;
        }

        /// <summary>
        /// Disables or enables default (BG) images of items in main slots
        /// when an item is (un)equipped.
        /// </summary>
        private void ShowImage()
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
