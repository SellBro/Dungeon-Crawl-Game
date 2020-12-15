using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace RPG.Core
{
    public abstract class MovingObject : MonoBehaviour
    {
        public float moveTime = 0.1f;
        
        
        [SerializeField] private LayerMask blockingLayer;
        [SerializeField] private bool isPlayer = false;
        
        
        private AIDestinationSetter _aiDestinationSetter;
        private AIPath _aiPath;
        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody;

        protected virtual void Start()
        {
            _aiDestinationSetter = GetComponent<AIDestinationSetter>();
            _aiPath = GetComponent<AIPath>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
        {
            Vector2 start = transform.position;
            Vector2 end = start + new Vector2(xDir, yDir);

            _boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, blockingLayer);
            _boxCollider.enabled = true;

            if (hit.transform == null)
            {
                StartCoroutine(SmoothMovement(end));
                return true;
            }

            return false;
        }

        protected IEnumerator SmoothMovement(Vector3 destination)
        {
            float sqrRemainingDistance = (transform.position - destination).sqrMagnitude;

            if (isPlayer)
            {
                _aiDestinationSetter.enabled = false;
                _aiPath.enabled = false;
            }
            

            while (sqrRemainingDistance > float.Epsilon)
            {
                Vector3 newPosition =
                    Vector3.MoveTowards(_rigidbody.position, destination, moveTime * Time.deltaTime);
                _rigidbody.MovePosition(newPosition);
                sqrRemainingDistance = (transform.position - destination).sqrMagnitude;
                yield return null;
            }
            
            if (isPlayer)
            {
                _aiDestinationSetter.enabled = true;
                _aiPath.enabled = true;
            }
        }

        protected virtual void AttemptMove <T> (int xDir, int yDir)
            where T : Component
        {
            RaycastHit2D hit;
            bool canMove = Move(xDir, yDir, out hit);

            if(hit.transform == null)
                return;

            T hitComponent = hit.transform.GetComponent<T>();

            if (!canMove && hitComponent != null)
            {
                OnCantMove(hitComponent);
            }
        }

        protected abstract void OnCantMove<T>(T component)
            where T : Component;
    }

}