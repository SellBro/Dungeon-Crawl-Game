using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using RPG.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Items
{
    public class BreakableBox : MonoBehaviour, IDamageable
    {
        [SerializeField] private Sprite[] boxSprites;
        [SerializeField] private Sprite[] damagedBoxSprite;
        [SerializeField] private int health = 2;

        private SingleNodeBlocker _blocker;
        private SpriteRenderer _sr;
        
        private int _spriteNum;

        private void Start()
        {
            _sr = GetComponent<SpriteRenderer>();
            
            _blocker = GetComponent<SingleNodeBlocker>();
            _blocker.manager = GameManager.Instance.blockManager;
            GameManager.Instance.AddSelfToObstacle(_blocker);
            
            _blocker.BlockAtCurrentPosition();
            Debug.Log(_blocker.lastBlocked);

             _spriteNum = Random.Range(0, boxSprites.Length);
            _sr.sprite = boxSprites[_spriteNum];
        }

        public void TakeDamage(int amount)
        {
            health--;
            
            if (health <= 0)
            {
                _blocker.Unblock();
            }
                

            _sr.sprite = damagedBoxSprite[_spriteNum];
        }
    }
}
