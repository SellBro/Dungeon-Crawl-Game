using System.Collections;
using System.Collections.Generic;
using SellBro.Core;
using SellBro.Units;
using UnityEngine;

namespace SellBro.Inventory
{
    public class RandomItemGenerator : MonoBehaviour
    {
        [Header("Managing Components")]
        public static RandomItemGenerator Instance = null;

        [Header("Generation Settings")]
        [SerializeField] private string[] adjective;
        [SerializeField] private string[] noun;
        
        [Header("ItemSprites")]
        [SerializeField] private Sprite[] weaponSprites;
        [SerializeField] private Sprite[] shieldSprites;
        [SerializeField] private Sprite[] headSprites;
        [SerializeField] private Sprite[] bodySprites;
        [SerializeField] private Sprite[] legSprites;
        [SerializeField] private Sprite[] ringSprites;
        [SerializeField] private Sprite[] amuletSprites;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }
    
        public Item[] GenerateStartingLoot()
        {
            Item[] items = new Item[7];

            for (int i = 1; i < 8; i++)
            {
                Item item = ScriptableObject.CreateInstance<Item>();;
                item.name = GenerateItemName();
                item.itemType = ItemType.Equippable;
                item.equippableItemType = (EquippableItemType)i;
                if (item.equippableItemType == (EquippableItemType) 5)
                {
                    item.damage = GenerateItemStat();
                }
                else
                {
                    item.armour = GenerateItemStat();
                }

                switch (i)
                {
                    case 1:
                        item.sprite = headSprites[Random.Range(0, headSprites.Length)];
                        item.name = "Helmet of " + noun[Random.Range(0, noun.Length)];
                        break;
                    case 2:
                        item.sprite = bodySprites[Random.Range(0, bodySprites.Length)];
                        item.name = "Armour of " + noun[Random.Range(0, noun.Length)];
                        break;
                    case 3:
                        item.sprite = legSprites[Random.Range(0, legSprites.Length)];
                        item.name = "Legs of " + noun[Random.Range(0, noun.Length)];
                        break;
                    case 4:
                        item.sprite = shieldSprites[Random.Range(0, shieldSprites.Length)];
                        item.name = "Shield of " + noun[Random.Range(0, noun.Length)];
                        break;
                    case 5:
                        item.sprite = weaponSprites[Random.Range(0, weaponSprites.Length)];
                        item.name = "Weapon of " + noun[Random.Range(0, noun.Length)];
                        break;
                    case 6:
                        item.sprite = ringSprites[Random.Range(0, ringSprites.Length)];
                        item.name = "Ring of " + noun[Random.Range(0, noun.Length)];
                        break;
                    case 7:
                        item.sprite = amuletSprites[Random.Range(0, amuletSprites.Length)];
                        item.name = "Amulet of " + noun[Random.Range(0, noun.Length)];
                        break;
                }

                item.description = "TBD";
                item.isStackable = false;
                items[i-1] = item;
            }

            return items;
        }
    
        private Item GenerateRandomItem()
        {
            return null;
        }
        
        private Item GenerateItem(int amount, bool isStackable, ItemType itemType, EquippableItemType equippableItemType)
        {
            return null;
        }

        public string GenerateItemName()
        {
            return adjective[Random.Range(0, adjective.Length)] + " " + noun[Random.Range(0, noun.Length)];
        }
        
        public int GenerateItemStat()
        {
            return (int)(Random.Range(0.5f, 2f) * GameManager.Instance.level *
                   GameManager.Instance.player.GetComponent<PlayerUnit>().level) + Random.Range(1,5);
        }
    }
}
