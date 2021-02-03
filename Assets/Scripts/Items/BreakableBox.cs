using System;
using Pathfinding;
using SellBro.Core;
using SellBro.Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SellBro.Items
{
    public class BreakableBox : MonoBehaviour, IDamageable
    {
        [SerializeField] private Sprite[] boxSprites;
        [SerializeField] private Sprite[] damagedBoxSprite;
        [SerializeField] private int health = 2;
        
        private SpriteRenderer _sr;
        private SingleNodeBlocker _blocker;
        
        private int _spriteNum;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _blocker = GetComponent<SingleNodeBlocker>();
        }

        private void Start()
        {
            _blocker.manager = GameManager.Instance.blockManager;
            GameManager.Instance.AddObstacleToList(_blocker);
            _blocker.BlockAtCurrentPosition();

            _spriteNum = Random.Range(0, boxSprites.Length);
            _sr.sprite = boxSprites[_spriteNum];
        }

        public void TakeDamage(int amount)
        {
            health--;
            
            if (health <= 0)
            {
                // AstarPath.active.UpdateGraphs(new Bounds(transform.position,new Vector3(0.5f,0.5f,0)));
                _blocker.Unblock();
                Destroy(gameObject);
            }
            
            _sr.sprite = damagedBoxSprite[_spriteNum];
        }
    }
}
