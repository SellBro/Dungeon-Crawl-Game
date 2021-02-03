using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Pathfinding;
using SellBro.Units;
using UnityEngine;

namespace SellBro.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Manager Settings")]
        [SerializeField] private GameObject cam;
        
        [Header("Managing Components")]
        public static GameManager Instance = null;
        public BlockManager blockManager;
        public GameObject unitsParent;
        
        [Header("Level settings")]
        public bool playerTurn = true;
        public int level = 1;

        [Header("Player")]
        public GameObject player;
        
        [Header("Game Settings")]
        [SerializeField] private float turnDelay = 0.1f;
        
        private List<SingleNodeBlocker> _obstacles = new List<SingleNodeBlocker>();
        private List<EnemyController> _units = new List<EnemyController>();
        
        private BlockManager.TraversalProvider _traversalProvider;
        private bool _unitsMoving;

        private CinemachineVirtualCamera _camera;

        private void Awake()
        {
            Instance = this;

            AstarPath.active = GetComponent<AstarPath>();
            blockManager = GetComponent<BlockManager>();
            _units = new List<EnemyController>();

            InitGame();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) playerTurn = false;
            
            if (playerTurn || _unitsMoving) return;

            StartCoroutine(MoveUnits());
        }

        private void InitGame()
        {
            _units.Clear();
        }

        public void UpdateAStar()
        {
            // Create A*
            AstarPath.active.Scan();
            
            // Block Nodes under Units
            foreach (var unit in _units)
            {
                SingleNodeBlocker unitNode = unit.GetComponent<SingleNodeBlocker>();
                _obstacles.Add(unitNode);
            }
            
            // A* var
            _traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.OnlySelector, _obstacles);
        }

        public void MoveCamera(Vector3 pos)
        {
            _camera = Instantiate(cam, new Vector3(pos.x, pos.y, -10), Quaternion.identity)
                .GetComponentInChildren<CinemachineVirtualCamera>();
            _camera.Follow = player.transform;
        }

        public void AddObstacleToList(SingleNodeBlocker obstacle)
        {
            _obstacles.Add(obstacle);
        }
        
        public void AddUnitToList(EnemyController unit)
        {
            _units.Add(unit);
        }
        
        public void RemoveUnitFromList(EnemyController unit)
        {
            _units.Remove(unit);
        }

        private IEnumerator MoveUnits()
        {
            _unitsMoving = true;
            yield return new WaitForSeconds(turnDelay);
            
            if (_units.Count == 0)
            {
                yield return new WaitForSeconds(turnDelay);
            }
            
            // Priority queue based on distance to player
            _units.Sort();
            
            foreach(EnemyController unit in _units)
            {
                unit.Act();
                yield return null;
            }

            playerTurn = true;
            _unitsMoving = false;
        }
        
        
        public Path ConstuctPath(Transform position, Transform target)
        {
            var path = ABPath.Construct(position.position, target.position, null);
            _traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.OnlySelector, _obstacles);
            
            // Make the path use a specific traversal provider
            path.traversalProvider = _traversalProvider;
            
            // Calculate the path synchronously
            AstarPath.StartPath(path);
            path.BlockUntilCalculated();
            
            if (path.error) 
            {
                Debug.Log("No path was found");
            } 

            return path;
        }
    }
}
