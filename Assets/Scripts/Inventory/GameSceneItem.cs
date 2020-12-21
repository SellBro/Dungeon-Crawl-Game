using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class GameSceneItem : MonoBehaviour
    {
        [SerializeField] private InventoryItem item;

        private SpriteRenderer _sr;

        private void Start()
        {
            _sr = GetComponent<SpriteRenderer>();
            _sr.sprite = item.sprite;
        }

        public InventoryItem GetInventoryItem()
        {
            return item;
        }
    }
}
