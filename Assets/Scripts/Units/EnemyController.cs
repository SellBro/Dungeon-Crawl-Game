using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

using RPG.Core;
using RPG.Units;

namespace RPG.units
{
    [RequireComponent(typeof(Unit))]
    public class EnemyController : MonoBehaviour, IComparable
    {
        [SerializeField] private float speed = 5;
        [SerializeField] private float agroDistance = 8;
        
        private SingleNodeBlocker _blocker;
        
        private bool shouldMove = false;
        private Vector3 destination;
        
        [HideInInspector] public float distanceToPlayer;
        
        public Transform target;

        private Unit _unit;

        private void Start()
        {
            _unit = GetComponent<Unit>();
            _blocker = GetComponent<SingleNodeBlocker>();
            _blocker.manager = GameManager.Instance.blockManager;
            
            if (target != null)
            {
                distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
            }

            target = GameManager.Instance.player.transform;
            GameManager.Instance.AddUnitToList(this);
        }

        private void Update()
        {
            if (shouldMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            }
        }

        private void Move()
        {
            _blocker.Unblock();
            var path = GameManager.Instance.ConstuctPath(transform, target);//_seeker.GetCurrentPath();
            _blocker.BlockAt(path.vectorPath[1]);
            destination = path.vectorPath[1];

            StartCoroutine(SmoothMovement(destination));
            
            distanceToPlayer = CalculateTargetDistance();
        }

        private IEnumerator SmoothMovement(Vector3 point)
        {
            shouldMove = true;
            
            while (transform.position != point)
            {
                yield return new WaitForEndOfFrame();
            }
            
            shouldMove = false;
        }

        public int CompareTo(object obj)
        {
            EnemyController enemy = (EnemyController) obj;
            if (distanceToPlayer > enemy.distanceToPlayer) return 1;
            else if (distanceToPlayer == enemy.distanceToPlayer) return 0;
            else return -1;
        }

        public void Act()
        {
            if(target == null) return;

            distanceToPlayer = CalculateTargetDistance();
            
            if (distanceToPlayer <= 1.2)
            {
                Attack();
            }
            else if(distanceToPlayer <= agroDistance)
            {
                Move();
                Debug.Log(_blocker.lastBlocked);
            }
        }

        private void Attack()
        {
            GameObject player = GameManager.Instance.player;
            player.GetComponent<Unit>().TakeDamage(_unit.GetDamage());
        }

        private float CalculateTargetDistance()
        {
            return Vector3.Distance(transform.position, target.transform.position);
        }
    }
}
