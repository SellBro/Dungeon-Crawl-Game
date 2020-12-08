using System;
using UnityEngine;

namespace RPG.Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int damage;

        public int _health;

        private void Start()
        {
            _health = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            _health = Mathf.Max(_health - amount, 0);
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
