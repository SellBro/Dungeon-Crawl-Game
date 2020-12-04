using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace RPG.Core
{
    public abstract class MovingObject : MonoBehaviour
    {
        [SerializeField] private float moveTime = 0.1f;
        [SerializeField] private LayerMask blockingLayer;
        [SerializeField] private Transform movePoint;

        private AIDestinationSetter _aiDestinationSetter;
        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody;

        protected virtual void Start()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _aiDestinationSetter = GetComponent<AIDestinationSetter>();
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
            if(movePoint != null) movePoint.transform.position = destination;

            while (sqrRemainingDistance > float.Epsilon)
            {
                if(movePoint != null) _aiDestinationSetter.target = movePoint;
                Vector3 newPosition =
                    Vector3.MoveTowards(_rigidbody.position, destination, moveTime * Time.deltaTime);
                _rigidbody.MovePosition(newPosition);
                sqrRemainingDistance = (transform.position - destination).sqrMagnitude;
                yield return null;
            }
            
            if(movePoint != null) _aiDestinationSetter.target = null;
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