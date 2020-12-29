using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SellBro.DungeonCrawler.Items
{
    public class Chest : MonoBehaviour
    {
        public GameObject uI;
        public void ShowUI()
        {
            Debug.Log("UI");
            uI.SetActive(true);
        }
    }
}
