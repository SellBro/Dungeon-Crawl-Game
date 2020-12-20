using RPG.Core;
using UnityEngine;

namespace RPG.Units
{
    public class EnemyUnit : Unit
    {
        public bool isPeaceful = false;

        private EnemyController _controller;

        protected override void Start()
        {
            base.Start();
            _controller = GetComponent<EnemyController>();
        }
        
        protected override void Die()
        {
            GameManager.Instance._units.Remove(_controller);
            _controller.UnblockGridNode();
            Destroy(gameObject);
        }
    }
}
