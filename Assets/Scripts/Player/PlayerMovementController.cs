using System;
using System.Collections;
using SellBro.Core;
using SellBro.Inventory;
using SellBro.Items;
using SellBro.Units;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace SellBro.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float speed = 10;
        [SerializeField] private float agroTriggerRange = 10;
        [SerializeField] private LayerMask whatIsBlocked;
        [SerializeField] private LayerMask whatIsCollision;
        [SerializeField] private LayerMask whatIsEnemy;
        
        private bool _isFacingRight = true;
        private Unit _unit;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        private void Start()
        {
            GameManager.Instance.player = gameObject;
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
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 destination = new Vector3(transform.position.x, transform.position.y + 1);
                
                if (CheckForCollisions(transform.up)) return;
                
                StartCoroutine(SmoothMovement(destination,transform.up));
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Vector3 destination = new Vector3(transform.position.x, transform.position.y - 1);
                
                if (CheckForCollisions(-transform.up)) return;
                
                StartCoroutine(SmoothMovement(destination,-transform.up));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (_isFacingRight)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    _isFacingRight = false;
                }
                Vector3 destination = new Vector3(transform.position.x - 1, transform.position.y);
                
                if (CheckForCollisions(-transform.right)) return;
                
                StartCoroutine(SmoothMovement(destination,-transform.right));
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (!_isFacingRight)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    _isFacingRight = true;
                }
                Vector3 destination = new Vector3(transform.position.x + 1, transform.position.y);
                
                if (CheckForCollisions(transform.right)) return;
                
                StartCoroutine(SmoothMovement(destination,transform.right));
            }
        }

        private bool CheckForCollisions(Vector3 tr)
        {
            RaycastHit2D hitEnemy = Physics2D.Raycast(transform.position, tr,1.1f, whatIsCollision);

            if (hitEnemy.transform == null) return false;

            if (CheckForChests(hitEnemy) || hitEnemy.transform.CompareTag("Obstacle")) return true;

            IDamageable enemy = hitEnemy.transform.gameObject.GetComponent<IDamageable>();
            Attack(enemy);
            return true;
        }

        private bool CheckForChests(RaycastHit2D hit)
        {
            if (hit.transform.CompareTag("Chest"))
            {
                hit.transform.gameObject.GetComponent<Chest>().ShowUI();
                return true;
            }

            return false;
        }

        private void Attack(IDamageable enemy)
        {
            enemy.TakeDamage(_unit.GetDamage());
            AgroUnits();
            GameManager.Instance.playerTurn = false;
        }

        private void AgroUnits()
        {
            Collider2D[] units = Physics2D.OverlapBoxAll(transform.position, new Vector2(agroTriggerRange,agroTriggerRange), 0, whatIsEnemy);

            Debug.Log(units.Length);
            
            foreach (var unit in units)
            {
                unit.GetComponent<EnemyUnit>().isPeaceful = false;
            }
        }

        private IEnumerator SmoothMovement(Vector3 destination, Vector3 tr)
        {
            RaycastHit2D hitBlock = Physics2D.Raycast(transform.position, tr,1.1f, whatIsBlocked);
            Debug.DrawRay(transform.position, tr, Color.red,3);
            if (hitBlock.transform == null)
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
