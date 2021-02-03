using System;
using SellBro.Core;

namespace SellBro.Units
{
    public class EnemyUnit : Unit
    {
        public bool isPeaceful = false;

        public static Action<int> EnemyDie;
        
        private EnemyController _controller;

        private void Awake()
        {
            _controller = GetComponent<EnemyController>();
        }

        protected override void Die()
        {
            GameManager.Instance.RemoveUnitFromList(_controller);
            _controller.UnblockGridNode();
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            EnemyDie?.Invoke(xpForKill);
        }
    }
}
