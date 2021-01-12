using SellBro.Core;
using UnityEngine;

namespace SellBro.Units
{
    public class Unit : MonoBehaviour, IDamageable
    {
        [Header("Unit Settings")]
        [SerializeField] protected int maxHealth;
        [SerializeField] protected int damage;
        [SerializeField] protected int xpForKill = 20;

        protected int _health;

        protected virtual void Start()
        {
            _health = maxHealth;
        }

        public virtual void TakeDamage(int amount)
        {
            _health = Mathf.Max(_health - amount, 0);

            if (_health <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            GameManager.Instance.player.GetComponent<PlayerUnit>().AddXP(xpForKill);
            Destroy(gameObject);
        }

        public virtual void Heal(int amount)
        {
            _health = Mathf.Min(_health + amount, maxHealth);
        }

        public int GetHealth()
        {
            return _health;
        }
        
        public virtual int GetDamage()
        {
            return damage;
        }
    }
}
