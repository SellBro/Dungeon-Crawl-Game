using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SellBro.DungeonCrawler.Inventory
{
    public class EquippedSlot : MonoBehaviour
    {
        public GameObject imageObj;
        
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
