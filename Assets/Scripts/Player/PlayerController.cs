using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 10;
        [SerializeField] private LayerMask whatIsBlocked;

        private BoxCollider2D _collider;

        private void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (GameManager.Instance.playerTurn)
            {
                GetInput();
            }
        }

        private void GetInput()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Vector3 destination = new Vector3(transform.position.x, transform.position.y + 1);
                StartCoroutine(SmoothMovement(destination,transform.up));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Vector3 destination = new Vector3(transform.position.x, transform.position.y - 1);
                StartCoroutine(SmoothMovement(destination,-transform.up));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Vector3 destination = new Vector3(transform.position.x - 1, transform.position.y);
                StartCoroutine(SmoothMovement(destination,-transform.right));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Vector3 destination = new Vector3(transform.position.x + 1, transform.position.y);
                StartCoroutine(SmoothMovement(destination,transform.right));
            }
        }

        private IEnumerator SmoothMovement(Vector3 destination, Vector3 tr)
        {
            Debug.Log(destination);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, tr,1.1f, whatIsBlocked);
            Debug.DrawRay(transform.position, tr, Color.red,3);
            if (hit.transform == null)
            {
                while (transform.position != destination)
                {
                    transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                    GameManager.Instance.playerTurn = false;
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}
