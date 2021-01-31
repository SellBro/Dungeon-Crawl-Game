using System;
using SellBro.Core;

namespace SellBro.Units
{
    public class EnemyUnit : Unit
    {
        public bool isPeaceful = false;

        private EnemyController _controller;

        private void Awake()
        {
            _controller = GetComponent<EnemyController>();
        }

        protected override void Die()
        {
            // TODO: Use Action
            GameManager.Instance.player.GetComponent<PlayerUnit>().AddXP(xpForKill);
            GameManager.Instance.units.Remove(_controller);
            _controller.UnblockGridNode();
            Destroy(gameObject);
        }
    }
}
