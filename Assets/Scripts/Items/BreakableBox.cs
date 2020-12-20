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
        
        private SpriteRenderer _sr;
        
        private int _spriteNum;

        private void Start()
        {
            _sr = GetComponent<SpriteRenderer>();

            _spriteNum = Random.Range(0, boxSprites.Length);
            _sr.sprite = boxSprites[_spriteNum];
        }

        public void TakeDamage(int amount)
        {
            health--;
            
            if (health <= 0)
            {
                AstarPath.active.UpdateGraphs(new Bounds(transform.position,new Vector3(0.5f,0.5f,0)));
                Destroy(gameObject);
            }
                

            _sr.sprite = damagedBoxSprite[_spriteNum];
        }
    }
}
