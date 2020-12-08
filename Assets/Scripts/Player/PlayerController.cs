
using System.Collections;
using UnityEngine;
using RPG.Core;
using RPG.Units;

namespace RPG.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 10;
        [SerializeField] private LayerMask whatIsBlocked;
        [SerializeField] private LayerMask whatIsEnemy;

        private BoxCollider2D _collider;
        private bool isFacingRight = true;
        private Unit _unit;

        private void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
            _unit = GetComponent<Unit>();
            GameManager.Instance.player = this.gameObject;
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
                
                if (CheckForEnemy(transform.up)) return;
                
                StartCoroutine(SmoothMovement(destination,transform.up));
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Vector3 destination = new Vector3(transform.position.x, transform.position.y - 1);
                
                if (CheckForEnemy(-transform.up)) return;
                
                StartCoroutine(SmoothMovement(destination,-transform.up));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (isFacingRight)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isFacingRight = false;
                }
                Vector3 destination = new Vector3(transform.position.x - 1, transform.position.y);
                
                if (CheckForEnemy(-transform.right)) return;
                
                StartCoroutine(SmoothMovement(destination,-transform.right));
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (!isFacingRight)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    isFacingRight = true;
                }
                Vector3 destination = new Vector3(transform.position.x + 1, transform.position.y);
                
                if (CheckForEnemy(transform.right)) return;
                
                StartCoroutine(SmoothMovement(destination,transform.right));
            }
        }

        private bool CheckForEnemy(Vector3 tr)
        {
            RaycastHit2D hitEnemy = Physics2D.Raycast(transform.position, tr,1.1f, whatIsEnemy);

            if (hitEnemy.transform == null) return false;
            
            if (hitEnemy.transform.gameObject.CompareTag("Enemy"))
            {
                Unit enemy = hitEnemy.transform.gameObject.GetComponent<Unit>();
                Attack(enemy);
                return true;
            }

            return false;
        }

        private void Attack(Unit enemy)
        {
            enemy.TakeDamage(_unit.GetDamage());
            GameManager.Instance.playerTurn = false;
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
