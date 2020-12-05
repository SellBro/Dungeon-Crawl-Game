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
                StartCoroutine(SmoothMovement(destination));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Vector3 destination = new Vector3(transform.position.x, transform.position.y - 1);
                StartCoroutine(SmoothMovement(destination));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Vector3 destination = new Vector3(transform.position.x - 1, transform.position.y);
                StartCoroutine(SmoothMovement(destination));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Vector3 destination = new Vector3(transform.position.x + 1, transform.position.y);
                StartCoroutine(SmoothMovement(destination));
            }
        }

        private IEnumerator SmoothMovement(Vector3 destination)
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
