using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Playables;

using RPG.Core;

namespace RPG.units
{
    public class EnemyController : MonoBehaviour, IComparable 
    {
        private SingleNodeBlocker _blocker;

        

        [HideInInspector] public float distanceToPlayer;
        
        public Transform target;

        private void Start()
        {
            _blocker = GetComponent<SingleNodeBlocker>();
            
            distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
            
            _blocker.BlockAtCurrentPosition();

            GameManager.Instance.AddUnitToList(this);
        }



        public void Move()
        {
            _blocker.Unblock();
            var path = GameManager.Instance.ConstuctPath(transform, target);//_seeker.GetCurrentPath();

            Vector3 destination = path.vectorPath[1];

            transform.position = destination;
            distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
            _blocker.BlockAtCurrentPosition();
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
            if (distanceToPlayer <= 1.1)
            {
                Debug.Log("Attack");
            }
            else
            {
                Move();
            }
        }
    }
}
