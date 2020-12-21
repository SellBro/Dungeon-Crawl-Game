using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventory
{
    public class InventoryUICell : MonoBehaviour
    {
        public InventoryItem item;
        public int count = 0;
        public Image image;

        private void Start()
        {
            image = GetComponent<Image>();
        }

        public void SetImage(Sprite sprite)
        {
            this.image.sprite = sprite;
        }
    }
}
