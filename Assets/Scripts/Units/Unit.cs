using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Units
{
    public class Unit : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int damage;


        private int _health;

        protected virtual void Start()
        {
            _health = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            _health = Mathf.Max(_health - amount, 0);

            if (_health <= 0)
            {
                Die();
            }
                
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
        }

        public void Heal(int amount)
        {
            _health = Mathf.Min(_health + amount, maxHealth);
        }

        public int GetHealth()
        {
            return _health;
        }
        
        public int GetDamage()
        {
            return damage;
        }
    }
}
